using FlyingRush.Core;
using NUnit.Framework;

namespace FlyingRush.Core.Tests
{
    public class WeaponTests
    {
        [Test]
        public void TheRifleFiresOnceEveryFireIntervalWhileTheTriggerIsHeld()
        {
            var gun = new Gun(WeaponKind.Rifle);

            Assert.That(gun.Pull(), Is.EqualTo(FireResult.Fired));
            Assert.That(gun.Pull(), Is.EqualTo(FireResult.Cooling));
            gun.Tick(0.05f);
            Assert.That(gun.Pull(), Is.EqualTo(FireResult.Cooling));
            gun.Tick(0.06f);
            Assert.That(gun.Pull(), Is.EqualTo(FireResult.Fired));
            Assert.That(gun.Magazine, Is.EqualTo(28));
        }

        [Test]
        public void EmptyingTheMagazineStartsAReloadThatRefillsItAfterTheReloadTime()
        {
            var gun = new Gun(WeaponKind.Sniper);
            for (int i = 0; i < 5; i++)
            {
                Assert.That(gun.Pull(), Is.EqualTo(FireResult.Fired));
                if (i < 4)
                {
                    gun.Tick(1.2f);
                }
            }

            Assert.That(gun.Magazine, Is.EqualTo(0));
            Assert.That(gun.Pull(), Is.EqualTo(FireResult.Reloading));
            gun.Tick(2.0f);
            Assert.That(gun.Reloading, Is.True);
            Assert.That(gun.ReloadProgress, Is.GreaterThan(0.7f).And.LessThan(1f));
            gun.Tick(0.5f);
            Assert.That(gun.Reloading, Is.False);
            Assert.That(gun.Magazine, Is.EqualTo(5));
            Assert.That(gun.Reserve, Is.EqualTo(10));
        }

        [Test]
        public void TheRifleNeverRunsOutOfSpareAmmunition()
        {
            var gun = new Gun(WeaponKind.Rifle);
            for (int round = 0; round < 300; round++)
            {
                if (gun.Pull() != FireResult.Fired)
                {
                    gun.Tick(2f);
                    Assert.That(gun.Pull(), Is.EqualTo(FireResult.Fired));
                }
                gun.Tick(0.1f);
            }

            Assert.That(gun.Spent, Is.False);
            Assert.That(gun.Reserve, Is.LessThan(0));
        }

        [Test]
        public void APowerWeaponRunsDryAndReportsEmpty()
        {
            var gun = new Gun(WeaponKind.Railgun);
            int shots = 0;
            for (int i = 0; i < 100; i++)
            {
                if (gun.Pull() == FireResult.Fired)
                {
                    shots++;
                }
                gun.Tick(3f);
            }

            Assert.That(shots, Is.EqualTo(12), "3 in the magazine and 9 spare");
            Assert.That(gun.Spent, Is.True);
            Assert.That(gun.Pull(), Is.EqualTo(FireResult.Empty));
        }

        [Test]
        public void AManualReloadOnlyStartsWhenTheMagazineIsNotFull()
        {
            var gun = new Gun(WeaponKind.Shotgun);
            Assert.That(gun.StartReload(), Is.False);

            gun.Pull();
            Assert.That(gun.StartReload(), Is.True);
            Assert.That(gun.Pull(), Is.EqualTo(FireResult.Reloading));
        }

        [Test]
        public void ShotgunPelletsLoseDamageWithDistance()
        {
            WeaponStats shotgun = WeaponCatalog.Of(WeaponKind.Shotgun);

            Assert.That(shotgun.DamageAt(5f), Is.EqualTo(13f));
            Assert.That(shotgun.DamageAt(20f), Is.EqualTo(13f * 0.65f).Within(0.001f));
            Assert.That(shotgun.DamageAt(80f), Is.EqualTo(13f * 0.3f).Within(0.001f));
            Assert.That(shotgun.MaxBodyDamagePerShot, Is.EqualTo(117f), "a point-blank shot nearly kills");
        }

        [Test]
        public void HeadshotsMultiplyBulletDamage()
        {
            WeaponStats sniper = WeaponCatalog.Of(WeaponKind.Sniper);

            Assert.That(DamageRules.Bullet(sniper, 50f, false), Is.EqualTo(100f));
            Assert.That(DamageRules.Bullet(sniper, 50f, true), Is.EqualTo(200f), "a sniper headshot kills at full health");
            Assert.That(DamageRules.IsHeadshot(1.6f, 0f, 1.8f), Is.True);
            Assert.That(DamageRules.IsHeadshot(1.3f, 0f, 1.8f), Is.False);
        }

        [Test]
        public void SplashDamageFadesOutAtTheRadiusAndHurtsTheShooterLess()
        {
            WeaponStats rockets = WeaponCatalog.Of(WeaponKind.Rockets);

            Assert.That(DamageRules.Splash(rockets, 0f, false), Is.EqualTo(90f));
            Assert.That(DamageRules.Splash(rockets, 4.5f, false), Is.EqualTo(0f));
            Assert.That(DamageRules.Splash(rockets, 2.25f, false), Is.EqualTo(90f * 0.625f).Within(0.001f));
            Assert.That(DamageRules.Splash(rockets, 0f, true), Is.EqualTo(90f * DamageRules.SelfDamageFactor).Within(0.001f));
            Assert.That(DamageRules.SplashKnockback(rockets, 0f), Is.GreaterThan(DamageRules.SplashKnockback(rockets, 3f)));
        }

        [Test]
        public void SpreadStaysInsideTheConeAndRepeatsForTheSameSeed()
        {
            var first = new Rng(42);
            var second = new Rng(42);
            for (int i = 0; i < 500; i++)
            {
                (float yaw, float pitch) a = Spread.Sample(first, 6f);
                (float yaw, float pitch) b = Spread.Sample(second, 6f);
                Assert.That(a, Is.EqualTo(b));
                Assert.That(a.yaw * a.yaw + a.pitch * a.pitch, Is.LessThanOrEqualTo(36.0001f));
            }
            Assert.That(Spread.Sample(first, 0f), Is.EqualTo((0f, 0f)));
        }

        [Test]
        public void AWeaponPadComesBackTwentySecondsAfterItIsTaken()
        {
            var pad = new WeaponPad(WeaponKind.Railgun, Vec3.Zero);

            Assert.That(pad.Take(), Is.True);
            Assert.That(pad.Available, Is.False);
            Assert.That(pad.Take(), Is.False);
            pad.Tick(19f);
            Assert.That(pad.Available, Is.False);
            Assert.That(pad.SecondsUntilBack, Is.EqualTo(1f).Within(0.001f));
            pad.Tick(1.01f);
            Assert.That(pad.Available, Is.True);
        }

        [Test]
        public void ALoadoutStartsWithTheRifleAndDrawsAPickedUpPowerWeapon()
        {
            var loadout = new Loadout();
            Assert.That(loadout.Current.Kind, Is.EqualTo(WeaponKind.Rifle));
            Assert.That(loadout.Switch(), Is.False, "nothing to switch to");

            loadout.PickUp(WeaponKind.Sniper);
            Assert.That(loadout.Current.Kind, Is.EqualTo(WeaponKind.Sniper));
            Assert.That(loadout.Switch(), Is.True);
            Assert.That(loadout.Current.Kind, Is.EqualTo(WeaponKind.Rifle));

            loadout.PickUp(WeaponKind.Rockets);
            Assert.That(loadout.Power.Kind, Is.EqualTo(WeaponKind.Rockets), "a new power weapon replaces the old one");
            Assert.That(loadout.Current, Is.SameAs(loadout.Power));
        }

        [Test]
        public void PickingUpTheSameWeaponRestoresItsAmmunition()
        {
            var loadout = new Loadout();
            loadout.PickUp(WeaponKind.Shotgun);
            loadout.Current.Pull();
            loadout.Current.StartReload();
            loadout.Tick(3f);
            Assert.That(loadout.Current.Reserve, Is.EqualTo(17));

            loadout.PickUp(WeaponKind.Shotgun);

            Assert.That(loadout.Current.Magazine, Is.EqualTo(6));
            Assert.That(loadout.Current.Reserve, Is.EqualTo(18));
        }

        [Test]
        public void ASpentPowerWeaponIsDroppedAndTheRifleComesBack()
        {
            var loadout = new Loadout();
            loadout.PickUp(WeaponKind.Railgun);
            for (int i = 0; i < 40; i++)
            {
                loadout.Current.Pull();
                loadout.Tick(3f);
            }

            Assert.That(loadout.HasPower, Is.False);
            Assert.That(loadout.Current, Is.SameAs(loadout.Rifle));
        }

        [Test]
        public void SwitchingWeaponsCancelsAReload()
        {
            var loadout = new Loadout();
            loadout.PickUp(WeaponKind.Sniper);
            loadout.Switch();
            loadout.Current.Pull();
            loadout.Current.StartReload();
            Assert.That(loadout.Rifle.Reloading, Is.True);

            loadout.Switch();

            Assert.That(loadout.Rifle.Reloading, Is.False);
            Assert.That(loadout.Rifle.Magazine, Is.EqualTo(29));
        }
    }
}
