using FlyingRush.Core;
using NUnit.Framework;

namespace FlyingRush.Core.Tests
{
    public class BotBrainTests
    {
        static BotSenses Calm => new BotSenses
        {
            HealthFraction = 1f,
            HasPowerWeapon = true,
            EnemyVisible = false,
            EnemyDistance = float.PositiveInfinity,
            SinceEnemySeen = float.PositiveInfinity,
            NearestPadDistance = float.PositiveInfinity,
            SinceHurt = 100f,
            WeaponRange = 100f,
        };

        static BotBrain Brain(BotDifficulty difficulty = BotDifficulty.Normal, int seed = 1) =>
            new BotBrain(BotProfile.For(difficulty), new Rng(seed));

        [Test]
        public void ABotWithNothingToDoRoams()
        {
            Assert.That(Brain().Decide(Calm), Is.EqualTo(BotGoal.Roam));
        }

        [Test]
        public void AVisibleEnemyIsFought()
        {
            BotSenses senses = Calm;
            senses.EnemyVisible = true;
            senses.EnemyDistance = 30f;

            Assert.That(Brain().Decide(senses), Is.EqualTo(BotGoal.Engage));
        }

        [Test]
        public void AHurtBotUnderFireRetreatsUnlessTheEnemyIsTooClose()
        {
            BotSenses senses = Calm;
            senses.EnemyVisible = true;
            senses.EnemyDistance = 30f;
            senses.HealthFraction = 0.2f;
            senses.SinceHurt = 1f;
            BotBrain brain = Brain();

            Assert.That(brain.Decide(senses), Is.EqualTo(BotGoal.Retreat));
            senses.EnemyDistance = 4f;
            Assert.That(brain.Decide(senses), Is.EqualTo(BotGoal.Engage), "cornered");
        }

        [Test]
        public void ARetreatingBotKeepsAwayUntilItHasHealedAndThenRoams()
        {
            BotSenses senses = Calm;
            senses.EnemyVisible = true;
            senses.EnemyDistance = 30f;
            senses.HealthFraction = 0.2f;
            senses.SinceHurt = 1f;
            BotBrain brain = Brain();
            brain.Decide(senses);

            senses.EnemyVisible = false;
            senses.HealthFraction = 0.5f;
            Assert.That(brain.Decide(senses), Is.EqualTo(BotGoal.Retreat));
            senses.HealthFraction = 0.9f;
            senses.SinceEnemySeen = 10f;
            Assert.That(brain.Decide(senses), Is.EqualTo(BotGoal.Roam));
        }

        [Test]
        public void ABotWithOnlyTheRifleGoesForANearbyWeapon()
        {
            BotSenses senses = Calm;
            senses.HasPowerWeapon = false;
            senses.NearestPadDistance = 20f;
            senses.SinceEnemySeen = 1f;

            Assert.That(Brain().Decide(senses), Is.EqualTo(BotGoal.CollectWeapon), "a weapon close by beats the hunt");
            senses.NearestPadDistance = 60f;
            Assert.That(Brain().Decide(senses), Is.EqualTo(BotGoal.Hunt), "a far weapon waits until the trail goes cold");
            senses.SinceEnemySeen = 20f;
            Assert.That(Brain().Decide(senses), Is.EqualTo(BotGoal.CollectWeapon));
        }

        [Test]
        public void ABotHuntsWhereItLastSawAnEnemyForAFewSeconds()
        {
            BotSenses senses = Calm;
            senses.SinceEnemySeen = BotBrain.HuntMemory - 0.1f;
            Assert.That(Brain().Decide(senses), Is.EqualTo(BotGoal.Hunt));
            senses.SinceEnemySeen = BotBrain.HuntMemory + 0.1f;
            Assert.That(Brain().Decide(senses), Is.EqualTo(BotGoal.Roam));
        }

        [Test]
        public void ABotDoesNotShootBeforeItsReactionTime()
        {
            BotBrain brain = Brain(BotDifficulty.Normal);
            brain.Acquire();

            brain.Track(0.3f);
            Assert.That(brain.Reacted, Is.False);
            Assert.That(brain.ShouldFire(0f, false, 0.016f), Is.False);
            brain.Track(0.2f);
            Assert.That(brain.Reacted, Is.True);
            Assert.That(brain.ShouldFire(0f, false, 0.016f), Is.True);
            Assert.That(brain.ShouldFire(20f, false, 0.016f), Is.False, "not while the aim is far off");
        }

        [Test]
        public void AimErrorShrinksWhileTrackingAndGrowsWithTargetSpeed()
        {
            BotBrain brain = Brain(BotDifficulty.Normal);
            brain.Acquire();
            float first = brain.AimError(0f);
            brain.Track(0.5f);
            float later = brain.AimError(0f);
            brain.Track(5f);
            float settled = brain.AimError(0f);

            Assert.That(first, Is.EqualTo(BotProfile.For(BotDifficulty.Normal).InitialError));
            Assert.That(later, Is.LessThan(first));
            Assert.That(settled, Is.EqualTo(BotProfile.For(BotDifficulty.Normal).SteadyError));
            Assert.That(brain.AimError(7f), Is.GreaterThan(settled));
        }

        [Test]
        public void AcquiringANewTargetResetsTheReaction()
        {
            BotBrain brain = Brain();
            brain.Acquire();
            brain.Track(2f);

            brain.Acquire();

            Assert.That(brain.Reacted, Is.False);
            Assert.That(brain.Tracked, Is.EqualTo(0f));
        }

        [Test]
        public void HarderBotsReactFasterAimBetterAndTurnQuicker()
        {
            BotProfile easy = BotProfile.For(BotDifficulty.Easy);
            BotProfile normal = BotProfile.For(BotDifficulty.Normal);
            BotProfile hard = BotProfile.For(BotDifficulty.Hard);

            Assert.That(easy.Reaction, Is.GreaterThan(normal.Reaction));
            Assert.That(normal.Reaction, Is.GreaterThan(hard.Reaction));
            Assert.That(easy.SteadyError, Is.GreaterThan(normal.SteadyError));
            Assert.That(normal.SteadyError, Is.GreaterThan(hard.SteadyError));
            Assert.That(easy.TurnSpeed, Is.LessThan(normal.TurnSpeed));
            Assert.That(normal.TurnSpeed, Is.LessThan(hard.TurnSpeed));
            Assert.That(easy.SightRange, Is.LessThan(hard.SightRange));
            Assert.That(easy.DamageScale, Is.LessThan(normal.DamageScale));
            Assert.That(normal.DamageScale, Is.LessThan(hard.DamageScale));
            Assert.That(hard.DamageScale, Is.LessThanOrEqualTo(1f));
        }

        [Test]
        public void EvenHardBotsMissSometimes()
        {
            BotProfile hard = BotProfile.For(BotDifficulty.Hard);

            Assert.That(hard.SteadyError, Is.GreaterThan(0.5f));
            Assert.That(hard.Reaction, Is.GreaterThan(0.2f), "slower than a quick human");
        }

        [Test]
        public void AutomaticFireComesInBurstsWithPauses()
        {
            BotBrain brain = Brain(BotDifficulty.Normal, 3);
            brain.Acquire();
            brain.Track(1f);
            int firing = 0, frames = 300;
            bool paused = false;
            for (int i = 0; i < frames; i++)
            {
                bool fire = brain.ShouldFire(0f, true, 1f / 60f);
                firing += fire ? 1 : 0;
                paused |= !fire;
            }

            Assert.That(paused, Is.True);
            Assert.That(firing, Is.GreaterThan(frames / 3));
            Assert.That(firing, Is.LessThan(frames));
        }

        [Test]
        public void AimOffsetsRepeatForTheSameSeedAndStayInsideTheError()
        {
            BotBrain a = Brain(BotDifficulty.Easy, 9);
            BotBrain b = Brain(BotDifficulty.Easy, 9);
            a.Acquire();
            b.Acquire();
            for (int i = 0; i < 100; i++)
            {
                (float yaw, float pitch) first = a.AimOffset(2f);
                Assert.That(first, Is.EqualTo(b.AimOffset(2f)));
                float error = a.AimError(2f);
                Assert.That(first.yaw * first.yaw + first.pitch * first.pitch, Is.LessThanOrEqualTo(error * error + 0.001f));
            }
        }

        [Test]
        public void TurningIsLimitedByTheTurnSpeedAndTakesTheShortWayRound()
        {
            BotBrain brain = Brain(BotDifficulty.Normal);

            Assert.That(brain.TurnTowards(0f, 90f, 0.1f), Is.EqualTo(26f).Within(0.001f));
            Assert.That(brain.TurnTowards(10f, 350f, 0.05f), Is.EqualTo(-3f).Within(0.001f));
            Assert.That(brain.TurnTowards(0f, 5f, 0.1f), Is.EqualTo(5f));
            Assert.That(BotBrain.DeltaAngle(170f, -170f), Is.EqualTo(20f).Within(0.001f));
        }

        [Test]
        public void StrafeDirectionChangesOverTime()
        {
            BotBrain brain = Brain(BotDifficulty.Normal, 5);
            bool left = false, right = false;
            for (int i = 0; i < 200; i++)
            {
                brain.UpdateStrafe(0.1f);
                left |= brain.StrafeDirection < 0;
                right |= brain.StrafeDirection > 0;
            }

            Assert.That(left && right, Is.True);
        }
    }
}
