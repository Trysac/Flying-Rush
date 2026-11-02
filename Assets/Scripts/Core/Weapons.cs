using System;

namespace FlyingRush.Core
{
    public enum WeaponKind
    {
        Rifle,
        Shotgun,
        Sniper,
        Rockets,
        Railgun,
    }

    /// <summary>How a weapon behaves. Distances in metres, times in seconds, angles in degrees.</summary>
    public sealed class WeaponStats
    {
        public WeaponKind Kind;
        public string Name;
        /// <summary>Damage of one bullet or pellet, or of a rocket's direct hit.</summary>
        public float Damage;
        public int Pellets = 1;
        public float FireInterval;
        public int Magazine;
        /// <summary>Spare rounds the weapon comes with; negative means endless.</summary>
        public int Reserve;
        public float ReloadSeconds;
        /// <summary>Half-angle of the random spread when firing from the hip.</summary>
        public float Spread;
        /// <summary>Half-angle of the spread while aiming down the sights.</summary>
        public float AimSpread;
        public float Range;
        /// <summary>How far a bot will stand and fight with it; beyond this it closes in first.</summary>
        public float EngageRange;
        public float HeadshotMultiplier = 1f;
        /// <summary>Damage is full up to here, then falls off linearly...</summary>
        public float FalloffStart;
        /// <summary>...to <see cref="FalloffMinimum"/> of it here and beyond.</summary>
        public float FalloffEnd;
        public float FalloffMinimum = 1f;
        public bool Automatic;
        /// <summary>A projectile instead of a hitscan shot when greater than zero.</summary>
        public float ProjectileSpeed;
        public float SplashRadius;
        public float SplashDamage;
        public float Knockback;
        /// <summary>The shot goes through every fighter on its line.</summary>
        public bool Pierces;
        /// <summary>Field of view while aiming.</summary>
        public float AimFov;
        /// <summary>How far the view kicks up per shot.</summary>
        public float Recoil;

        public bool EndlessAmmo => Reserve < 0;

        /// <summary>Damage of one bullet at a distance, with the falloff (not the headshot).</summary>
        public float DamageAt(float distance)
        {
            if (FalloffEnd <= FalloffStart || distance <= FalloffStart)
            {
                return Damage;
            }
            float t = Math.Min(1f, (distance - FalloffStart) / (FalloffEnd - FalloffStart));
            return Damage * (1f - t * (1f - FalloffMinimum));
        }

        /// <summary>The most a single trigger pull can deal (every pellet on target, no falloff).</summary>
        public float MaxBodyDamagePerShot => Damage * Pellets;
    }

    /// <summary>
    /// The five weapons. The assault rifle is everyone's starting weapon, as in the 2021 game; the
    /// others lie on pads high up the map and come with limited ammunition.
    /// </summary>
    public static class WeaponCatalog
    {
        static readonly WeaponStats[] All =
        {
            new WeaponStats
            {
                Kind = WeaponKind.Rifle, Name = "ASSAULT RIFLE", Damage = 15f, FireInterval = 0.1f, Magazine = 30,
                Reserve = -1, ReloadSeconds = 1.6f, Spread = 1.4f, AimSpread = 0.45f, Range = 140f, EngageRange = 38f,
                HeadshotMultiplier = 1.5f, FalloffStart = 30f, FalloffEnd = 70f, FalloffMinimum = 0.6f,
                Automatic = true, AimFov = 60f, Recoil = 0.35f,
            },
            new WeaponStats
            {
                Kind = WeaponKind.Shotgun, Name = "SHOTGUN", Damage = 13f, Pellets = 9, FireInterval = 0.85f,
                Magazine = 6, Reserve = 18, ReloadSeconds = 2.2f, Spread = 6f, AimSpread = 4.5f, Range = 45f, EngageRange = 16f,
                HeadshotMultiplier = 1.25f, FalloffStart = 10f, FalloffEnd = 30f, FalloffMinimum = 0.3f,
                AimFov = 72f, Recoil = 2.4f,
            },
            new WeaponStats
            {
                Kind = WeaponKind.Sniper, Name = "SNIPER RIFLE", Damage = 100f, FireInterval = 1.2f, Magazine = 5,
                Reserve = 15, ReloadSeconds = 2.4f, Spread = 4f, AimSpread = 0f, Range = 220f, EngageRange = 120f,
                HeadshotMultiplier = 2f, AimFov = 22f, Recoil = 3.5f,
            },
            new WeaponStats
            {
                Kind = WeaponKind.Rockets, Name = "ROCKET LAUNCHER", Damage = 40f, FireInterval = 0.9f, Magazine = 4,
                Reserve = 8, ReloadSeconds = 2.5f, Spread = 0f, AimSpread = 0f, Range = 160f, EngageRange = 45f,
                ProjectileSpeed = 32f, SplashRadius = 4.5f, SplashDamage = 90f, Knockback = 11f,
                AimFov = 65f, Recoil = 2.8f,
            },
            new WeaponStats
            {
                Kind = WeaponKind.Railgun, Name = "RAILGUN", Damage = 130f, FireInterval = 1.5f, Magazine = 3,
                Reserve = 9, ReloadSeconds = 2.8f, Spread = 0.6f, AimSpread = 0f, Range = 220f, EngageRange = 100f,
                HeadshotMultiplier = 1.5f, Pierces = true, Knockback = 4f, AimFov = 40f, Recoil = 4f,
            },
        };

        public static WeaponStats Of(WeaponKind kind) => All[(int)kind];

        public static int Count => All.Length;
    }

    /// <summary>What happened when the trigger was pulled.</summary>
    public enum FireResult
    {
        Fired,
        Cooling,
        Reloading,
        Empty,
    }

    /// <summary>
    /// One carried weapon: its magazine, spare ammunition, fire cooldown and reload. The game calls
    /// <see cref="Tick"/> every frame and <see cref="Pull"/> when the trigger is held.
    /// </summary>
    public sealed class Gun
    {
        float cooldown;
        float reloadLeft;

        public Gun(WeaponKind kind)
        {
            Stats = WeaponCatalog.Of(kind);
            Magazine = Stats.Magazine;
            Reserve = Stats.Reserve;
        }

        public WeaponStats Stats { get; }
        public WeaponKind Kind => Stats.Kind;
        public int Magazine { get; private set; }
        /// <summary>Spare rounds; negative when endless.</summary>
        public int Reserve { get; private set; }
        public bool Reloading => reloadLeft > 0f;

        /// <summary>0 when a reload starts, 1 when it is done (1 when not reloading).</summary>
        public float ReloadProgress => Reloading ? 1f - reloadLeft / Stats.ReloadSeconds : 1f;

        /// <summary>Nothing in the magazine and nothing to reload with.</summary>
        public bool Spent => Magazine == 0 && Reserve == 0;

        public bool CanReload => !Reloading && Magazine < Stats.Magazine && Reserve != 0;

        public void Tick(float deltaTime)
        {
            if (cooldown > 0f)
            {
                cooldown = Math.Max(0f, cooldown - deltaTime);
            }
            if (reloadLeft > 0f)
            {
                reloadLeft -= deltaTime;
                if (reloadLeft <= 0f)
                {
                    reloadLeft = 0f;
                    FinishReload();
                }
            }
        }

        /// <summary>
        /// Fires one round when the cooldown allows it. An empty magazine starts a reload on its own
        /// when there is spare ammunition.
        /// </summary>
        public FireResult Pull()
        {
            if (Reloading)
            {
                return FireResult.Reloading;
            }
            if (Magazine == 0)
            {
                return StartReload() ? FireResult.Reloading : FireResult.Empty;
            }
            if (cooldown > 0f)
            {
                return FireResult.Cooling;
            }
            Magazine--;
            cooldown = Stats.FireInterval;
            if (Magazine == 0)
            {
                StartReload();
            }
            return FireResult.Fired;
        }

        public bool StartReload()
        {
            if (!CanReload)
            {
                return false;
            }
            reloadLeft = Stats.ReloadSeconds;
            return true;
        }

        /// <summary>Switching away cancels a reload; the rounds stay where they were.</summary>
        public void CancelReload() => reloadLeft = 0f;

        /// <summary>Picking up the same weapon again restores its full ammunition.</summary>
        public void Refill()
        {
            reloadLeft = 0f;
            Magazine = Stats.Magazine;
            Reserve = Stats.Reserve;
        }

        void FinishReload()
        {
            int needed = Stats.Magazine - Magazine;
            int taken = Stats.EndlessAmmo ? needed : Math.Min(needed, Reserve);
            Magazine += taken;
            if (!Stats.EndlessAmmo)
            {
                Reserve -= taken;
            }
        }
    }

    /// <summary>Random spread for a shot: a direction offset inside a cone, from a seeded generator.</summary>
    public static class Spread
    {
        /// <summary>
        /// Yaw and pitch offsets in degrees, uniformly inside a circle of radius
        /// <paramref name="halfAngle"/>.
        /// </summary>
        public static (float yaw, float pitch) Sample(Rng rng, float halfAngle)
        {
            if (halfAngle <= 0f)
            {
                return (0f, 0f);
            }
            float angle = rng.Range(0f, MathF.PI * 2f);
            float radius = halfAngle * MathF.Sqrt(rng.Value());
            return (MathF.Cos(angle) * radius, MathF.Sin(angle) * radius);
        }
    }

    /// <summary>A weapon waiting on a pad; it comes back a while after someone takes it.</summary>
    public sealed class WeaponPad
    {
        public const float RespawnSeconds = 20f;

        float timer;

        public WeaponPad(WeaponKind kind, Vec3 position)
        {
            Kind = kind;
            Position = position;
        }

        public WeaponKind Kind { get; }
        public Vec3 Position { get; }
        public bool Available => timer <= 0f;
        public float SecondsUntilBack => Math.Max(0f, timer);

        public bool Take()
        {
            if (!Available)
            {
                return false;
            }
            timer = RespawnSeconds;
            return true;
        }

        public void Tick(float deltaTime)
        {
            if (timer > 0f)
            {
                timer -= deltaTime;
            }
        }

        public void Reset() => timer = 0f;
    }

    /// <summary>
    /// What a fighter carries: the rifle in the first slot and at most one power weapon in the
    /// second. A spent power weapon is dropped and the rifle comes back.
    /// </summary>
    public sealed class Loadout
    {
        public Loadout()
        {
            Rifle = new Gun(WeaponKind.Rifle);
            Current = Rifle;
        }

        public Gun Rifle { get; }
        public Gun Power { get; private set; }
        public Gun Current { get; private set; }

        public bool HasPower => Power != null;

        /// <summary>Takes a weapon from a pad: a new one replaces the power weapon and is drawn.</summary>
        public void PickUp(WeaponKind kind)
        {
            if (kind == WeaponKind.Rifle)
            {
                Rifle.Refill();
                return;
            }
            if (Power != null && Power.Kind == kind)
            {
                Power.Refill();
            }
            else
            {
                Power = new Gun(kind);
            }
            Draw(Power);
        }

        /// <summary>Swaps between the two slots; false when there is only the rifle.</summary>
        public bool Switch()
        {
            if (Power == null)
            {
                return false;
            }
            Draw(Current == Rifle ? Power : Rifle);
            return true;
        }

        public void Tick(float deltaTime)
        {
            Rifle.Tick(deltaTime);
            Power?.Tick(deltaTime);
            if (Power != null && Power.Spent)
            {
                bool drawn = Current == Power;
                Power = null;
                if (drawn)
                {
                    Current = Rifle;
                }
            }
        }

        void Draw(Gun gun)
        {
            if (Current != gun)
            {
                Current.CancelReload();
            }
            Current = gun;
        }
    }
}
