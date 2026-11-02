using System;

namespace FlyingRush.Core
{
    /// <summary>What a hit did.</summary>
    public readonly struct DamageResult
    {
        public readonly float Dealt;
        public readonly bool Killed;
        public readonly bool Blocked;

        public DamageResult(float dealt, bool killed, bool blocked)
        {
            Dealt = dealt;
            Killed = killed;
            Blocked = blocked;
        }
    }

    /// <summary>
    /// A fighter's health: 150 hit points as in the 2021 game, a short spawn protection, and
    /// regeneration once nobody has hurt it for a while.
    /// </summary>
    public sealed class Vitals
    {
        public const float MaxHealth = 150f;
        public const float SpawnProtectionSeconds = 1.5f;
        public const float RegenDelaySeconds = 5f;
        public const float RegenPerSecond = 20f;

        float protection;
        float sinceHurt;

        public Vitals() => Revive();

        public float Health { get; private set; }
        public bool Alive => Health > 0f;
        public bool Protected => protection > 0f;
        public float Fraction => Health / MaxHealth;

        /// <summary>Seconds since the last damage taken.</summary>
        public float SinceHurt => sinceHurt;

        /// <summary>Full health and a fresh spawn protection.</summary>
        public void Revive()
        {
            Health = MaxHealth;
            protection = SpawnProtectionSeconds;
            sinceHurt = RegenDelaySeconds;
        }

        /// <summary>Firing gives up the spawn protection.</summary>
        public void DropProtection() => protection = 0f;

        public DamageResult Hurt(float amount)
        {
            if (!Alive || amount <= 0f)
            {
                return new DamageResult(0f, false, false);
            }
            if (Protected)
            {
                return new DamageResult(0f, false, true);
            }
            float dealt = Math.Min(amount, Health);
            Health -= dealt;
            sinceHurt = 0f;
            if (Health <= 0.001f)
            {
                Health = 0f;
                return new DamageResult(dealt, true, false);
            }
            return new DamageResult(dealt, false, false);
        }

        /// <summary>Falling out of the cage kills through any protection.</summary>
        public DamageResult Kill()
        {
            if (!Alive)
            {
                return new DamageResult(0f, false, false);
            }
            float dealt = Health;
            Health = 0f;
            protection = 0f;
            return new DamageResult(dealt, true, false);
        }

        public void Tick(float deltaTime)
        {
            if (!Alive)
            {
                return;
            }
            if (protection > 0f)
            {
                protection = Math.Max(0f, protection - deltaTime);
            }
            sinceHurt += deltaTime;
            if (sinceHurt >= RegenDelaySeconds && Health < MaxHealth)
            {
                Health = Math.Min(MaxHealth, Health + RegenPerSecond * deltaTime);
            }
        }
    }

    /// <summary>Damage rules shared by every weapon.</summary>
    public static class DamageRules
    {
        /// <summary>Your own rockets hurt you this much less, so rocket jumps are worth it.</summary>
        public const float SelfDamageFactor = 0.35f;

        /// <summary>The top of a fighter that counts as the head, in metres.</summary>
        public const float HeadHeight = 0.38f;

        /// <summary>Damage of one hitscan bullet at a distance, with the headshot bonus.</summary>
        public static float Bullet(WeaponStats stats, float distance, bool headshot) =>
            stats.DamageAt(distance) * (headshot ? stats.HeadshotMultiplier : 1f);

        /// <summary>
        /// Splash damage of an explosion: full at the centre, nothing at the radius, scaled down for
        /// the shooter's own body.
        /// </summary>
        public static float Splash(WeaponStats stats, float distance, bool self)
        {
            if (stats.SplashRadius <= 0f || distance >= stats.SplashRadius)
            {
                return 0f;
            }
            float falloff = 1f - Math.Max(0f, distance) / stats.SplashRadius;
            float damage = stats.SplashDamage * (0.25f + 0.75f * falloff);
            return self ? damage * SelfDamageFactor : damage;
        }

        /// <summary>Knockback speed of an explosion at a distance (m/s), strongest at the centre.</summary>
        public static float SplashKnockback(WeaponStats stats, float distance)
        {
            if (stats.SplashRadius <= 0f || distance >= stats.SplashRadius)
            {
                return 0f;
            }
            return stats.Knockback * (1f - Math.Max(0f, distance) / stats.SplashRadius * 0.6f);
        }

        /// <summary>
        /// Whether a hit point counts as a headshot: within <see cref="HeadHeight"/> of the top of a
        /// body of the given height standing on <paramref name="feetY"/>.
        /// </summary>
        public static bool IsHeadshot(float hitY, float feetY, float bodyHeight) =>
            hitY >= feetY + bodyHeight - HeadHeight;
    }

    /// <summary>
    /// Running and the multi-jump that gives the game its name: up to seven jumps before you touch
    /// the ground again, as set in the 2021 game.
    /// </summary>
    public sealed class Jumps
    {
        public const int MaxJumps = 7;
        public const float Gravity = 18f;
        public const float GroundJumpSpeed = 8f;
        public const float AirJumpSpeed = 7.5f;
        public const float RunSpeed = 7.5f;
        public const float CrouchSpeed = 3.4f;
        public const float StandHeight = 1.8f;
        public const float CrouchHeight = 1.05f;

        public int Left { get; private set; } = MaxJumps;
        public int Used => MaxJumps - Left;

        /// <summary>
        /// Uses a jump if one is left and returns the upward speed it sets: a full jump from the
        /// ground, a slightly weaker one in the air.
        /// </summary>
        public bool TryJump(bool grounded, out float upwardSpeed)
        {
            if (grounded)
            {
                Left = MaxJumps;
            }
            if (Left <= 0)
            {
                upwardSpeed = 0f;
                return false;
            }
            upwardSpeed = Left == MaxJumps && grounded ? GroundJumpSpeed : AirJumpSpeed;
            Left--;
            return true;
        }

        /// <summary>Touching the ground gives every jump back.</summary>
        public void Land() => Left = MaxJumps;

        /// <summary>Height gained by one jump at a given upward speed.</summary>
        public static float Apex(float upwardSpeed) => upwardSpeed * upwardSpeed / (2f * Gravity);

        /// <summary>
        /// The highest a fighter can climb from the ground by jumping again at every apex.
        /// </summary>
        public static float MaxClimb => Apex(GroundJumpSpeed) + (MaxJumps - 1) * Apex(AirJumpSpeed);
    }
}
