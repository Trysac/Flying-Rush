using System;

namespace FlyingRush.Core
{
    public enum BotDifficulty
    {
        Easy,
        Normal,
        Hard,
    }

    /// <summary>How well a bot plays. Every number is worse on easier levels.</summary>
    public sealed class BotProfile
    {
        public BotDifficulty Difficulty;
        /// <summary>Seconds between first seeing an enemy and the first shot.</summary>
        public float Reaction;
        /// <summary>Aim error in degrees right after acquiring a target...</summary>
        public float InitialError;
        /// <summary>...settling to this after <see cref="SettleSeconds"/> of tracking.</summary>
        public float SteadyError;
        public float SettleSeconds;
        /// <summary>Extra error in degrees per metre a second the target moves sideways.</summary>
        public float MovementPenalty;
        /// <summary>How fast the bot turns its view, degrees a second.</summary>
        public float TurnSpeed;
        /// <summary>It only pulls the trigger when the aim is within this many degrees.</summary>
        public float FireCone;
        /// <summary>Below this share of health it backs off to cover.</summary>
        public float RetreatHealth;
        /// <summary>How far it notices enemies, in metres.</summary>
        public float SightRange;
        /// <summary>Seconds it holds the rifle's trigger before a short pause.</summary>
        public float Burst;
        /// <summary>Its hits deal this share of the weapon's damage, so easier bots are beatable.</summary>
        public float DamageScale;

        public static BotProfile For(BotDifficulty difficulty) => difficulty switch
        {
            BotDifficulty.Easy => new BotProfile
            {
                Difficulty = difficulty, Reaction = 0.75f, InitialError = 11f, SteadyError = 4.5f, SettleSeconds = 1.8f,
                MovementPenalty = 0.6f, TurnSpeed = 150f, FireCone = 9f, RetreatHealth = 0.2f, SightRange = 45f, Burst = 0.3f,
                DamageScale = 0.5f,
            },
            BotDifficulty.Hard => new BotProfile
            {
                Difficulty = difficulty, Reaction = 0.26f, InitialError = 5f, SteadyError = 1.4f, SettleSeconds = 0.8f,
                MovementPenalty = 0.2f, TurnSpeed = 400f, FireCone = 4f, RetreatHealth = 0.35f, SightRange = 90f, Burst = 0.7f,
                DamageScale = 0.9f,
            },
            _ => new BotProfile
            {
                Difficulty = BotDifficulty.Normal, Reaction = 0.45f, InitialError = 8f, SteadyError = 2.8f, SettleSeconds = 1.2f,
                MovementPenalty = 0.4f, TurnSpeed = 260f, FireCone = 6f, RetreatHealth = 0.3f, SightRange = 65f, Burst = 0.45f,
                DamageScale = 0.7f,
            },
        };
    }

    public enum BotGoal
    {
        /// <summary>Wander between waypoints, looking for a fight.</summary>
        Roam,
        /// <summary>Fight a visible enemy.</summary>
        Engage,
        /// <summary>Go where an enemy was last seen.</summary>
        Hunt,
        /// <summary>Climb to a weapon pad.</summary>
        CollectWeapon,
        /// <summary>Break line of sight and wait for health to come back.</summary>
        Retreat,
    }

    /// <summary>What a bot knows when it decides.</summary>
    public struct BotSenses
    {
        public float HealthFraction;
        public bool HasPowerWeapon;
        public bool EnemyVisible;
        public float EnemyDistance;
        /// <summary>Seconds since an enemy was last seen; infinity when never.</summary>
        public float SinceEnemySeen;
        /// <summary>Distance to the nearest available weapon pad; infinity when none.</summary>
        public float NearestPadDistance;
        /// <summary>Seconds since it was last hurt.</summary>
        public float SinceHurt;
        /// <summary>The effective range of the weapon in hand.</summary>
        public float WeaponRange;
    }

    /// <summary>
    /// The bots' decisions, without the engine: what to do next, where to aim and when to shoot.
    /// Bots make mistakes on purpose (a reaction delay and an aim error that shrinks while they
    /// track you), so easy bots are beatable and hard ones are not hopeless.
    /// </summary>
    public sealed class BotBrain
    {
        /// <summary>Enemies closer than this are fought even when hurt: running would not help.</summary>
        public const float CornerDistance = 7f;
        public const float HuntMemory = 5f;
        public const float PadInterest = 35f;

        readonly Rng rng;
        float tracked;
        float burstLeft;
        float pauseLeft;
        float strafeLeft;

        public BotBrain(BotProfile profile, Rng rng)
        {
            Profile = profile;
            this.rng = rng;
            StrafeDirection = 1;
        }

        public BotProfile Profile { get; }
        public BotGoal Goal { get; private set; } = BotGoal.Roam;
        /// <summary>Seconds the current target has been tracked.</summary>
        public float Tracked => tracked;
        /// <summary>-1 or 1: which way to sidestep while fighting.</summary>
        public int StrafeDirection { get; private set; }

        public BotGoal Decide(BotSenses senses)
        {
            if (senses.EnemyVisible)
            {
                bool hurt = senses.HealthFraction < Profile.RetreatHealth && senses.SinceHurt < Vitals.RegenDelaySeconds + 3f;
                Goal = hurt && senses.EnemyDistance > CornerDistance ? BotGoal.Retreat : BotGoal.Engage;
            }
            else if (Goal == BotGoal.Retreat && senses.HealthFraction < 0.8f)
            {
                Goal = BotGoal.Retreat;
            }
            else if (!senses.HasPowerWeapon && senses.NearestPadDistance < PadInterest)
            {
                Goal = BotGoal.CollectWeapon;
            }
            else if (senses.SinceEnemySeen < HuntMemory)
            {
                Goal = BotGoal.Hunt;
            }
            else if (!senses.HasPowerWeapon && !float.IsInfinity(senses.NearestPadDistance))
            {
                Goal = BotGoal.CollectWeapon;
            }
            else
            {
                Goal = BotGoal.Roam;
            }
            return Goal;
        }

        /// <summary>A new target: the reaction delay and the aim error start over.</summary>
        public void Acquire()
        {
            tracked = 0f;
            burstLeft = Profile.Burst;
            pauseLeft = 0f;
        }

        public void Track(float deltaTime) => tracked += deltaTime;

        /// <summary>True once the reaction time since acquiring the target has passed.</summary>
        public bool Reacted => tracked >= Profile.Reaction;

        /// <summary>
        /// Aim error in degrees right now: large on acquiring, shrinking towards the steady error,
        /// plus a penalty for a target moving sideways.
        /// </summary>
        public float AimError(float targetSidewaysSpeed)
        {
            float settle = Math.Min(1f, tracked / Profile.SettleSeconds);
            float error = Profile.InitialError + (Profile.SteadyError - Profile.InitialError) * settle;
            return error + Math.Abs(targetSidewaysSpeed) * Profile.MovementPenalty;
        }

        /// <summary>A random aim offset (yaw, pitch) in degrees inside the current error.</summary>
        public (float yaw, float pitch) AimOffset(float targetSidewaysSpeed) =>
            Spread.Sample(rng, AimError(targetSidewaysSpeed));

        /// <summary>
        /// Whether to hold the trigger this frame: after reacting, when the view is close enough
        /// to the target, in bursts for automatic weapons.
        /// </summary>
        public bool ShouldFire(float degreesOffTarget, bool automatic, float deltaTime)
        {
            if (!Reacted || degreesOffTarget > Profile.FireCone)
            {
                return false;
            }
            if (!automatic)
            {
                return true;
            }
            if (pauseLeft > 0f)
            {
                pauseLeft -= deltaTime;
                if (pauseLeft <= 0f)
                {
                    burstLeft = Profile.Burst;
                }
                return false;
            }
            burstLeft -= deltaTime;
            if (burstLeft <= 0f)
            {
                pauseLeft = rng.Range(0.25f, 0.5f);
            }
            return true;
        }

        /// <summary>Sidesteps change direction every so often while fighting.</summary>
        public void UpdateStrafe(float deltaTime)
        {
            strafeLeft -= deltaTime;
            if (strafeLeft <= 0f)
            {
                StrafeDirection = rng.Value() < 0.5f ? -1 : 1;
                strafeLeft = rng.Range(0.6f, 1.6f);
            }
        }

        /// <summary>Whether to throw in a jump while fighting (bots fly too).</summary>
        public bool WantsCombatJump() => rng.Value() < (Profile.Difficulty == BotDifficulty.Easy ? 0.004f : 0.012f);

        /// <summary>A random waypoint index to roam to.</summary>
        public int PickRoamTarget(int nodeCount) => rng.Range(0, nodeCount);

        /// <summary>Turns a heading towards a target by at most the profile's turn speed.</summary>
        public float TurnTowards(float current, float target, float deltaTime)
        {
            float delta = DeltaAngle(current, target);
            float step = Profile.TurnSpeed * deltaTime;
            return Math.Abs(delta) <= step ? target : current + Math.Sign(delta) * step;
        }

        /// <summary>The shortest signed difference between two angles in degrees.</summary>
        public static float DeltaAngle(float from, float to)
        {
            float delta = (to - from) % 360f;
            if (delta > 180f) delta -= 360f;
            if (delta < -180f) delta += 360f;
            return delta;
        }
    }
}
