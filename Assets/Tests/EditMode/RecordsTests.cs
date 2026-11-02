using FlyingRush.Core;
using NUnit.Framework;

namespace FlyingRush.Core.Tests
{
    public class RecordsTests
    {
        [Test]
        public void ANewStoreHasNoRecords()
        {
            var records = new Records(new MemoryRecordStore());

            Assert.That(records.Matches(MatchMode.FreeForAll), Is.EqualTo(0));
            Assert.That(records.BestKills(MatchMode.FreeForAll), Is.EqualTo(0));
            Assert.That(records.KillDeathRatio(MatchMode.FreeForAll), Is.EqualTo(0f));
        }

        [Test]
        public void SubmittingAMatchAddsItAndSavesTheStoreOnce()
        {
            var store = new MemoryRecordStore();
            var records = new Records(store);

            RecordUpdate update = records.Submit(new MatchSummary(MatchMode.FreeForAll, true, 15, 6, 4, 1));

            Assert.That(store.SaveCount, Is.EqualTo(1));
            Assert.That(update.NewBestKills, Is.True);
            Assert.That(update.NewBestStreak, Is.True);
            Assert.That(update.PreviousBestKills, Is.EqualTo(0));
            Assert.That(records.Matches(MatchMode.FreeForAll), Is.EqualTo(1));
            Assert.That(records.Wins(MatchMode.FreeForAll), Is.EqualTo(1));
            Assert.That(records.BestKills(MatchMode.FreeForAll), Is.EqualTo(15));
            Assert.That(records.BestStreak(MatchMode.FreeForAll), Is.EqualTo(4));
            Assert.That(records.KillDeathRatio(MatchMode.FreeForAll), Is.EqualTo(2.5f));
        }

        [Test]
        public void AWorseMatchKeepsTheBestsButAddsToTheTotals()
        {
            var records = new Records(new MemoryRecordStore());
            records.Submit(new MatchSummary(MatchMode.FreeForAll, true, 15, 5, 6, 1));

            RecordUpdate update = records.Submit(new MatchSummary(MatchMode.FreeForAll, false, 9, 10, 2, 3));

            Assert.That(update.NewBestKills, Is.False);
            Assert.That(update.NewBestStreak, Is.False);
            Assert.That(update.PreviousBestKills, Is.EqualTo(15));
            Assert.That(records.BestKills(MatchMode.FreeForAll), Is.EqualTo(15));
            Assert.That(records.Matches(MatchMode.FreeForAll), Is.EqualTo(2));
            Assert.That(records.Wins(MatchMode.FreeForAll), Is.EqualTo(1));
            Assert.That(records.TotalKills(MatchMode.FreeForAll), Is.EqualTo(24));
            Assert.That(records.TotalDeaths(MatchMode.FreeForAll), Is.EqualTo(15));
        }

        [Test]
        public void EachModeKeepsItsOwnRecords()
        {
            var records = new Records(new MemoryRecordStore());

            records.Submit(new MatchSummary(MatchMode.TeamDeathmatch, true, 12, 3, 5, 2));

            Assert.That(records.Matches(MatchMode.TeamDeathmatch), Is.EqualTo(1));
            Assert.That(records.Matches(MatchMode.FreeForAll), Is.EqualTo(0));
            Assert.That(records.BestKills(MatchMode.FreeForAll), Is.EqualTo(0));
        }

        [Test]
        public void RecordsSurviveANewRecordsObjectOnTheSameStore()
        {
            var store = new MemoryRecordStore();
            new Records(store).Submit(new MatchSummary(MatchMode.FreeForAll, false, 7, 9, 3, 4));

            var reloaded = new Records(store);

            Assert.That(reloaded.BestKills(MatchMode.FreeForAll), Is.EqualTo(7));
            Assert.That(reloaded.Matches(MatchMode.FreeForAll), Is.EqualTo(1));
        }

        [Test]
        public void ASummaryIsReadFromAFinishedMatch()
        {
            var match = new Match(MatchSettings.For(MatchMode.FreeForAll));
            match.Add("YOU", 0, true);
            match.Add("BOT", 0, false);
            match.Tick(3.1f);
            match.Tick(1.6f);
            for (int i = 0; i < 15; i++)
            {
                match.Damage(0, 1, 1000f, WeaponKind.Rifle, false);
                match.Tick(3.1f);
                match.Tick(1.6f);
            }

            MatchSummary summary = MatchSummary.Of(match);

            Assert.That(summary.Won, Is.True);
            Assert.That(summary.Kills, Is.EqualTo(15));
            Assert.That(summary.Deaths, Is.EqualTo(0));
            Assert.That(summary.BestStreak, Is.EqualTo(15));
            Assert.That(summary.Place, Is.EqualTo(1));
            Assert.That(summary.Mode, Is.EqualTo(MatchMode.FreeForAll));
        }
    }

    public class SettingsTests
    {
        [Test]
        public void SettingsRoundTripThroughTheStore()
        {
            var store = new MemoryRecordStore();
            var settings = new GameSettings
            {
                Sensitivity = 14, InvertY = true, Volume = 3, MusicVolume = 2, Fov = 95, ScreenShake = false,
                Difficulty = BotDifficulty.Hard, Mode = MatchMode.TeamDeathmatch, Bots = 5, TimeLimitMinutes = 5,
            };

            settings.Save(store);
            GameSettings loaded = GameSettings.Load(store);

            Assert.That(loaded.Sensitivity, Is.EqualTo(14));
            Assert.That(loaded.InvertY, Is.True);
            Assert.That(loaded.Volume, Is.EqualTo(3));
            Assert.That(loaded.MusicVolume, Is.EqualTo(2));
            Assert.That(loaded.Fov, Is.EqualTo(95));
            Assert.That(loaded.ScreenShake, Is.False);
            Assert.That(loaded.Difficulty, Is.EqualTo(BotDifficulty.Hard));
            Assert.That(loaded.Mode, Is.EqualTo(MatchMode.TeamDeathmatch));
            Assert.That(loaded.Bots, Is.EqualTo(5));
            Assert.That(loaded.TimeLimitMinutes, Is.EqualTo(5));
            Assert.That(store.SaveCount, Is.EqualTo(1));
        }

        [Test]
        public void BrokenStoredValuesFallBackToSafeOnes()
        {
            var store = new MemoryRecordStore();
            store.SetInt("flyingrush.settings.sensitivity", 99);
            store.SetInt("flyingrush.settings.fov", 12);
            store.SetInt("flyingrush.settings.bots", 4);
            store.SetInt("flyingrush.settings.difficulty", 7);
            store.SetInt("flyingrush.settings.timelimit", 3);

            GameSettings loaded = GameSettings.Load(store);

            Assert.That(loaded.Sensitivity, Is.EqualTo(GameSettings.MaxSensitivity));
            Assert.That(loaded.Fov, Is.EqualTo(GameSettings.MinFov));
            Assert.That(loaded.Bots, Is.EqualTo(7));
            Assert.That(loaded.Difficulty, Is.EqualTo(BotDifficulty.Hard));
            Assert.That(loaded.TimeLimitMinutes, Is.EqualTo(10));
        }

        [Test]
        public void StepsClampOrWrapThroughTheirValues()
        {
            var settings = new GameSettings();

            settings.StepFov(10);
            Assert.That(settings.Fov, Is.EqualTo(GameSettings.MaxFov));
            settings.StepSensitivity(-50);
            Assert.That(settings.Sensitivity, Is.EqualTo(GameSettings.MinSensitivity));
            settings.StepDifficulty(1);
            Assert.That(settings.Difficulty, Is.EqualTo(BotDifficulty.Hard));
            settings.StepDifficulty(1);
            Assert.That(settings.Difficulty, Is.EqualTo(BotDifficulty.Easy), "wraps round");
            settings.StepBots(1);
            Assert.That(settings.Bots, Is.EqualTo(3));
            settings.StepTimeLimit(1);
            Assert.That(settings.TimeLimitMinutes, Is.EqualTo(0));
            settings.StepMode(1);
            Assert.That(settings.Mode, Is.EqualTo(MatchMode.TeamDeathmatch));
        }

        [Test]
        public void TheSettingsSetUpTheMatch()
        {
            var settings = new GameSettings { Mode = MatchMode.TeamDeathmatch, TimeLimitMinutes = 5 };

            MatchSettings match = settings.ToMatch();

            Assert.That(match.Mode, Is.EqualTo(MatchMode.TeamDeathmatch));
            Assert.That(match.KillLimit, Is.EqualTo(30));
            Assert.That(match.TimeLimit, Is.EqualTo(300f));
        }
    }
}
