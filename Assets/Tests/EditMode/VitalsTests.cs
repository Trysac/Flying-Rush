using FlyingRush.Core;
using NUnit.Framework;

namespace FlyingRush.Core.Tests
{
    public class VitalsTests
    {
        static Vitals Unprotected()
        {
            var vitals = new Vitals();
            vitals.Tick(Vitals.SpawnProtectionSeconds + 0.01f);
            return vitals;
        }

        [Test]
        public void FightersHaveOneHundredAndFiftyHitPointsAsIn2021()
        {
            var vitals = new Vitals();

            Assert.That(vitals.Health, Is.EqualTo(150f));
            Assert.That(vitals.Alive, Is.True);
            Assert.That(vitals.Fraction, Is.EqualTo(1f));
        }

        [Test]
        public void SpawnProtectionBlocksDamageUntilItRunsOutOrTheFighterShoots()
        {
            var vitals = new Vitals();
            DamageResult blocked = vitals.Hurt(50f);
            Assert.That(blocked.Blocked, Is.True);
            Assert.That(blocked.Dealt, Is.EqualTo(0f));
            Assert.That(vitals.Health, Is.EqualTo(150f));

            vitals.DropProtection();
            DamageResult hit = vitals.Hurt(50f);
            Assert.That(hit.Blocked, Is.False);
            Assert.That(hit.Dealt, Is.EqualTo(50f));
            Assert.That(vitals.Health, Is.EqualTo(100f));
        }

        [Test]
        public void DamagePastZeroKillsOnceAndDealsOnlyWhatWasLeft()
        {
            Vitals vitals = Unprotected();
            vitals.Hurt(120f);

            DamageResult lethal = vitals.Hurt(100f);
            DamageResult after = vitals.Hurt(100f);

            Assert.That(lethal.Killed, Is.True);
            Assert.That(lethal.Dealt, Is.EqualTo(30f));
            Assert.That(after.Killed, Is.False, "a dead fighter cannot be killed again");
            Assert.That(after.Dealt, Is.EqualTo(0f));
            Assert.That(vitals.Alive, Is.False);
        }

        [Test]
        public void HealthRegeneratesOnlyAfterFiveSecondsWithoutDamage()
        {
            Vitals vitals = Unprotected();
            vitals.Hurt(100f);

            vitals.Tick(4.9f);
            Assert.That(vitals.Health, Is.EqualTo(50f));
            vitals.Tick(0.2f);
            Assert.That(vitals.Health, Is.EqualTo(54f).Within(0.01f), "20 a second once regenerating");
            vitals.Tick(1f);
            Assert.That(vitals.Health, Is.EqualTo(74f).Within(0.01f));
            for (int i = 0; i < 20; i++)
            {
                vitals.Tick(1f);
            }
            Assert.That(vitals.Health, Is.EqualTo(150f), "never above the maximum");
        }

        [Test]
        public void FallingKillsThroughSpawnProtection()
        {
            var vitals = new Vitals();

            DamageResult fall = vitals.Kill();

            Assert.That(fall.Killed, Is.True);
            Assert.That(vitals.Alive, Is.False);
        }

        [Test]
        public void ReviveRestoresFullHealthWithProtection()
        {
            Vitals vitals = Unprotected();
            vitals.Kill();

            vitals.Revive();

            Assert.That(vitals.Health, Is.EqualTo(150f));
            Assert.That(vitals.Protected, Is.True);
        }

        [Test]
        public void SevenJumpsBeforeTouchingTheGroundAgain()
        {
            var jumps = new Jumps();

            Assert.That(jumps.TryJump(true, out float first), Is.True);
            Assert.That(first, Is.EqualTo(Jumps.GroundJumpSpeed));
            for (int i = 0; i < 6; i++)
            {
                Assert.That(jumps.TryJump(false, out float air), Is.True);
                Assert.That(air, Is.EqualTo(Jumps.AirJumpSpeed));
            }
            Assert.That(jumps.Used, Is.EqualTo(7));
            Assert.That(jumps.TryJump(false, out float none), Is.False);
            Assert.That(none, Is.EqualTo(0f));

            jumps.Land();
            Assert.That(jumps.Left, Is.EqualTo(Jumps.MaxJumps));
        }

        [Test]
        public void WalkingOffALedgeLeavesSevenAirJumps()
        {
            var jumps = new Jumps();
            for (int i = 0; i < 7; i++)
            {
                Assert.That(jumps.TryJump(false, out float speed), Is.True);
                Assert.That(speed, Is.EqualTo(Jumps.AirJumpSpeed));
            }
            Assert.That(jumps.TryJump(false, out _), Is.False);
        }

        [Test]
        public void ChainedJumpsClimbMoreThanElevenMetres()
        {
            Assert.That(Jumps.Apex(Jumps.GroundJumpSpeed), Is.EqualTo(64f / 36f).Within(0.0001f));
            Assert.That(Jumps.MaxClimb, Is.GreaterThan(11f).And.LessThan(12f));
        }
    }
}
