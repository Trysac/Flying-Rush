using System;
using System.Collections.Generic;
using FlyingRush.Core;
using UnityEngine;

namespace FlyingRush.Game
{
    /// <summary>One hit, for the HUD, the sounds and the bots' awareness.</summary>
    public readonly struct HitInfo
    {
        public readonly FighterBody Attacker;
        public readonly FighterBody Victim;
        public readonly float Damage;
        public readonly bool Headshot;
        public readonly bool Killed;
        public readonly Vector3 Point;
        public readonly WeaponKind Weapon;

        public HitInfo(FighterBody attacker, FighterBody victim, float damage, bool headshot, bool killed, Vector3 point, WeaponKind weapon)
        {
            Attacker = attacker;
            Victim = victim;
            Damage = damage;
            Headshot = headshot;
            Killed = killed;
            Point = point;
            Weapon = weapon;
        }
    }

    /// <summary>
    /// Shooting: hitscan bullets and pellets with spread from a seeded generator, the piercing
    /// railgun, rockets and their splash, all scored through the core match.
    /// </summary>
    public sealed class Combat
    {
        readonly Effects effects;
        readonly AudioDirector audio;
        readonly GameArt art;
        readonly Func<Match> match;
        readonly Func<Collider, FighterBody> bodyOf;
        readonly Rng rng;
        readonly RaycastHit[] hits = new RaycastHit[32];
        readonly Collider[] nearby = new Collider[32];
        readonly List<Rocket> rockets = new List<Rocket>();
        readonly Transform rocketParent;

        public Combat(Effects effects, AudioDirector audio, GameArt art, Func<Match> match, Func<Collider, FighterBody> bodyOf, Rng rng,
            Transform rocketParent)
        {
            this.effects = effects;
            this.audio = audio;
            this.art = art;
            this.match = match;
            this.bodyOf = bodyOf;
            this.rng = rng;
            this.rocketParent = rocketParent;
        }

        /// <summary>Raised for every hit that dealt damage or was blocked.</summary>
        public event Action<HitInfo> Hit;

        /// <summary>Raised when a fighter fires, with its weapon (for sounds and recoil).</summary>
        public event Action<FighterBody, WeaponKind> Fired;

        public int RocketsInFlight => rockets.Count;

        /// <summary>
        /// Pulls the trigger of the body's current weapon toward <paramref name="direction"/> from its eye.
        /// </summary>
        public FireResult Fire(FighterBody shooter, Vector3 direction, bool aiming)
        {
            Gun gun = shooter.Loadout.Current;
            FireResult result = gun.Pull();
            if (result != FireResult.Fired)
            {
                return result;
            }
            shooter.Fighter.Vitals.DropProtection();
            WeaponStats stats = gun.Stats;
            Vector3 eye = shooter.Eye;
            Vector3 muzzle = shooter.Muzzle;
            Color color = Palette.Of(stats.Kind);
            effects.MuzzleFlash(muzzle, Color.Lerp(color, new Color(1f, 0.8f, 0.4f), 0.5f), stats.Kind == WeaponKind.Rockets ? 0.6f : 0.35f);
            Fired?.Invoke(shooter, stats.Kind);

            float spread = aiming ? stats.AimSpread : stats.Spread;
            if (!shooter.Grounded)
            {
                spread += 0.6f;
            }
            if (stats.ProjectileSpeed > 0f)
            {
                LaunchRocket(shooter, eye, direction, stats);
                return result;
            }
            for (int pellet = 0; pellet < stats.Pellets; pellet++)
            {
                (float yaw, float pitch) offset = Spread.Sample(rng, spread);
                Vector3 side = Vector3.Cross(direction, Vector3.up);
                side = side.sqrMagnitude < 1e-4f ? Vector3.right : side.normalized;
                Vector3 shot = Quaternion.AngleAxis(offset.yaw, Vector3.up) * Quaternion.AngleAxis(offset.pitch, side) * direction;
                Vector3 end = Trace(shooter, eye, shot.normalized, stats);
                if (stats.Kind == WeaponKind.Railgun)
                {
                    effects.Beam(muzzle, end, color);
                }
                else if (pellet % 3 == 0)
                {
                    effects.Tracer(muzzle, end, Color.Lerp(color, Color.white, 0.4f), stats.Kind == WeaponKind.Sniper ? 0.07f : 0.035f);
                }
            }
            return result;
        }

        /// <summary>One hitscan ray: damages the first fighter it meets (every one for the railgun).</summary>
        Vector3 Trace(FighterBody shooter, Vector3 origin, Vector3 direction, WeaponStats stats)
        {
            int count = Physics.RaycastNonAlloc(origin, direction, hits, stats.Range, Layers.ShotMask, QueryTriggerInteraction.Ignore);
            Array.Sort(hits, 0, count, HitDistance.Instance);
            Vector3 end = origin + direction * stats.Range;
            for (int i = 0; i < count; i++)
            {
                RaycastHit hit = hits[i];
                FighterBody victim = bodyOf(hit.collider);
                if (victim == shooter)
                {
                    continue;
                }
                if (victim == null)
                {
                    effects.Impact(hit.point, hit.normal, new Color(1f, 0.8f, 0.5f), 6);
                    return hit.point;
                }
                bool headshot = victim.IsHeadshot(hit.point);
                float damage = DamageRules.Bullet(stats, hit.distance, headshot);
                Apply(shooter, victim, damage, headshot, hit.point, stats.Kind);
                if (stats.Knockback > 0f)
                {
                    victim.AddImpulse(direction * stats.Knockback);
                }
                if (!stats.Pierces)
                {
                    return hit.point;
                }
            }
            return end;
        }

        void Apply(FighterBody attacker, FighterBody victim, float damage, bool headshot, Vector3 point, WeaponKind weapon)
        {
            Match current = match();
            if (current == null || !victim.Fighter.Alive)
            {
                return;
            }
            if (attacker != null && attacker != victim && !current.AreEnemies(attacker.Fighter, victim.Fighter))
            {
                return;
            }
            if (attacker != null && attacker != victim)
            {
                damage *= attacker.DamageScale;
            }
            DamageResult result = current.Damage(attacker != null ? attacker.Fighter.Id : -1, victim.Fighter.Id, damage, weapon, headshot);
            if (result.Dealt > 0f || result.Blocked)
            {
                effects.Impact(point, Vector3.up, victim.Color, result.Blocked ? 4 : 10);
                Hit?.Invoke(new HitInfo(attacker, victim, result.Dealt, headshot, result.Killed, point, weapon));
            }
        }

        void LaunchRocket(FighterBody shooter, Vector3 eye, Vector3 direction, WeaponStats stats)
        {
            var host = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            UnityEngine.Object.Destroy(host.GetComponent<Collider>());
            host.name = "Rocket";
            host.layer = Layers.Effect;
            host.transform.SetParent(rocketParent, false);
            host.transform.localScale = new Vector3(0.16f, 0.25f, 0.16f);
            host.GetComponent<Renderer>().sharedMaterial = art.Pad;
            var rocket = host.AddComponent<Rocket>();
            // Start at the eye so a rocket fired point-blank into a wall cannot skip it.
            rocket.Launch(shooter, eye + direction * 0.2f, direction * stats.ProjectileSpeed, stats, this);
            rockets.Add(rocket);
            audio.PlayAt(art.RocketFire, eye, 0.6f);
        }

        /// <summary>Moves a rocket along its path; true when it hit something and exploded.</summary>
        internal bool StepRocket(Rocket rocket, float deltaTime)
        {
            Vector3 from = rocket.transform.position;
            Vector3 step = rocket.Velocity * deltaTime;
            int count = Physics.RaycastNonAlloc(from, step.normalized, hits, step.magnitude, Layers.ShotMask, QueryTriggerInteraction.Ignore);
            Array.Sort(hits, 0, count, HitDistance.Instance);
            for (int i = 0; i < count; i++)
            {
                FighterBody body = bodyOf(hits[i].collider);
                if (body == rocket.Owner)
                {
                    continue;
                }
                if (body != null)
                {
                    Apply(rocket.Owner, body, rocket.Stats.Damage, false, hits[i].point, WeaponKind.Rockets);
                }
                Explode(rocket, hits[i].point + hits[i].normal * 0.1f);
                return true;
            }
            rocket.transform.position = from + step;
            rocket.Age += deltaTime;
            if (rocket.Age > rocket.Stats.Range / rocket.Stats.ProjectileSpeed)
            {
                Explode(rocket, rocket.transform.position);
                return true;
            }
            effects.Glow(rocket.transform.position, new Color(1f, 0.55f, 0.2f), 0.5f, 0.12f);
            return false;
        }

        void Explode(Rocket rocket, Vector3 point)
        {
            WeaponStats stats = rocket.Stats;
            effects.Explosion(point, stats.SplashRadius);
            audio.PlayAt(art.Explosion, point, 1f);
            Explosion?.Invoke(point, stats.SplashRadius);
            int count = Physics.OverlapSphereNonAlloc(point, stats.SplashRadius + 1f, nearby, 1 << Layers.Fighter, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < count; i++)
            {
                FighterBody body = bodyOf(nearby[i]);
                if (body == null || !body.Fighter.Alive)
                {
                    continue;
                }
                Vector3 closest = nearby[i].ClosestPoint(point);
                float distance = Vector3.Distance(closest, point);
                if (distance >= stats.SplashRadius || Physics.Linecast(point, body.Chest, Layers.WorldMask, QueryTriggerInteraction.Ignore))
                {
                    continue;
                }
                bool self = body == rocket.Owner;
                float damage = DamageRules.Splash(stats, distance, self);
                Vector3 push = (body.Chest - point).normalized;
                body.AddImpulse((push + Vector3.up * 0.6f).normalized * DamageRules.SplashKnockback(stats, distance));
                Apply(rocket.Owner, body, damage, false, closest, WeaponKind.Rockets);
            }
            rockets.Remove(rocket);
            UnityEngine.Object.Destroy(rocket.gameObject);
        }

        /// <summary>Raised when a rocket explodes (position, radius), for the screen shake.</summary>
        public event Action<Vector3, float> Explosion;

        public void ClearRockets()
        {
            foreach (Rocket rocket in rockets)
            {
                if (rocket != null)
                {
                    UnityEngine.Object.Destroy(rocket.gameObject);
                }
            }
            rockets.Clear();
        }

        sealed class HitDistance : IComparer<RaycastHit>
        {
            public static readonly HitDistance Instance = new HitDistance();
            public int Compare(RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance);
        }
    }

    /// <summary>A rocket in flight; <see cref="Combat"/> moves it and makes it explode.</summary>
    public sealed class Rocket : MonoBehaviour
    {
        Combat combat;

        public FighterBody Owner { get; private set; }
        public Vector3 Velocity { get; private set; }
        public WeaponStats Stats { get; private set; }
        public float Age { get; set; }

        public void Launch(FighterBody owner, Vector3 position, Vector3 velocity, WeaponStats stats, Combat owningCombat)
        {
            Owner = owner;
            Velocity = velocity;
            Stats = stats;
            combat = owningCombat;
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(velocity) * Quaternion.Euler(90f, 0f, 0f);
        }

        void Update()
        {
            if (Time.deltaTime > 0f)
            {
                combat.StepRocket(this, Time.deltaTime);
            }
        }
    }
}
