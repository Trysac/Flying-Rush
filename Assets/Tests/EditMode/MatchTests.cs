using System.Collections.Generic;
using System.Linq;
using FlyingRush.Core;
using NUnit.Framework;

namespace FlyingRush.Core.Tests
{
    public class MatchTests
    {
        /// <summary>A match already past its countdown with the player and some bots, spawn protection gone.</summary>
        static Match Playing(MatchMode mode, int bots, float timeLimit = 0f)
        {
            var match = new Match(MatchSettings.For(mode, timeLimit));
            int[] teams = TeamBalance.Assign(bots + 1);
            match.Add("YOU", teams[0], true);
            for (int i = 0; i < bots; i++)
            {
                match.Add("BOT " + (i + 1), teams[i + 1], false);
            }
            match.Tick(match.Settings.CountdownSeconds + 0.01f);
            match.Tick(Vitals.SpawnProtectionSeconds + 0.01f);
            return match;
        }

        static void Kill(Match match, int killer, int victim, WeaponKind weapon = WeaponKind.Rifle, bool headshot = false)
        {
            DamageResult result = match.Damage(killer, victim, 1000f, weapon, headshot);
            Assert.That(result.Killed, Is.True, $"{killer} should kill {victim}");
            // Bring the victim back so it can die again.
            match.Tick(match.Settings.RespawnSeconds + 0.01f);
            match.Tick(Vitals.SpawnProtectionSeconds + 0.01f);
        }

        [Test]
        public void TheDefaultsAreThe2021Settings()
        {
            MatchSettings ffa = MatchSettings.For(MatchMode.FreeForAll);

            Assert.That(ffa.KillLimit, Is.EqualTo(15));
            Assert.That(ffa.TimeLimit, Is.EqualTo(0f), "no round time limit");
            Assert.That(ffa.RoundEndSeconds, Is.EqualTo(5f));
            Assert.That(MatchSettings.For(MatchMode.TeamDeathmatch).KillLimit, Is.EqualTo(30));
        }

        [Test]
        public void NobodyCanBeHurtDuringTheCountdown()
        {
            var match = new Match(MatchSettings.For(MatchMode.FreeForAll));
            match.Add("YOU", 0, true);
            match.Add("BOT", 0, false);
            bool started = false;
            match.Started += () => started = true;

            Assert.That(match.Phase, Is.EqualTo(MatchPhase.Countdown));
            Assert.That(match.Damage(0, 1, 500f, WeaponKind.Rifle, false).Dealt, Is.EqualTo(0f));
            match.Tick(2.9f);
            Assert.That(started, Is.False);
            Assert.That(match.CountdownLeft, Is.EqualTo(0.1f).Within(0.001f));
            match.Tick(0.2f);
            Assert.That(started, Is.True);
            Assert.That(match.Phase, Is.EqualTo(MatchPhase.Playing));
        }

        [Test]
        public void AKillScoresForTheKillerAndADeathForTheVictimAndGoesInTheFeed()
        {
            Match match = Playing(MatchMode.FreeForAll, 3);
            var heard = new List<KillEvent>();
            match.Killed += heard.Add;

            match.Damage(0, 2, 1000f, WeaponKind.Sniper, true);

            Assert.That(match.Get(0).Kills, Is.EqualTo(1));
            Assert.That(match.Get(2).Deaths, Is.EqualTo(1));
            Assert.That(match.Get(2).Alive, Is.False);
            Assert.That(heard.Count, Is.EqualTo(1));
            KillEvent kill = match.Feed.Single();
            Assert.That(kill.KillerId, Is.EqualTo(0));
            Assert.That(kill.VictimId, Is.EqualTo(2));
            Assert.That(kill.Weapon, Is.EqualTo(WeaponKind.Sniper));
            Assert.That(kill.Headshot, Is.True);
            Assert.That(kill.Suicide, Is.False);
        }

        [Test]
        public void TheFeedKeepsTheLatestFiveKills()
        {
            Match match = Playing(MatchMode.FreeForAll, 3);
            for (int i = 0; i < 7; i++)
            {
                Kill(match, 0, 1 + i % 3);
            }

            Assert.That(match.Feed.Count, Is.EqualTo(Match.FeedLength));
            Assert.That(match.Feed.Last().VictimId, Is.EqualTo(1 + 6 % 3));
        }

        [Test]
        public void TheVictimRespawnsAfterThreeSecondsWithFullHealth()
        {
            Match match = Playing(MatchMode.FreeForAll, 1);
            var respawned = new List<Fighter>();
            match.Respawned += respawned.Add;
            match.Damage(0, 1, 1000f, WeaponKind.Rifle, false);

            match.Tick(2.5f);
            Assert.That(match.Get(1).Alive, Is.False);
            Assert.That(match.Get(1).RespawnLeft, Is.EqualTo(0.5f).Within(0.001f));
            Assert.That(respawned, Is.Empty);
            match.Tick(0.6f);

            Assert.That(match.Get(1).Alive, Is.True);
            Assert.That(match.Get(1).Vitals.Health, Is.EqualTo(Vitals.MaxHealth));
            Assert.That(match.Get(1).Vitals.Protected, Is.True);
            Assert.That(respawned.Single(), Is.SameAs(match.Get(1)));
        }

        [Test]
        public void FallingOutCountsAsADeathWithoutGivingAnyoneAKill()
        {
            Match match = Playing(MatchMode.FreeForAll, 2);

            match.Fell(1);

            Assert.That(match.Get(1).Deaths, Is.EqualTo(1));
            Assert.That(match.Fighters.Sum(f => f.Kills), Is.EqualTo(0));
            Assert.That(match.Feed.Single().Suicide, Is.True);
        }

        [Test]
        public void YourOwnRocketHurtsYouButDoesNotScore()
        {
            Match match = Playing(MatchMode.FreeForAll, 1);

            DamageResult result = match.Damage(0, 0, 1000f, WeaponKind.Rockets, false);

            Assert.That(result.Killed, Is.True);
            Assert.That(match.Get(0).Kills, Is.EqualTo(0));
            Assert.That(match.Get(0).Deaths, Is.EqualTo(1));
        }

        [Test]
        public void FifteenKillsWinAFreeForAllAndTheResultsFollowTheRoundEndPause()
        {
            Match match = Playing(MatchMode.FreeForAll, 3);
            bool ended = false;
            match.Ended += () => ended = true;
            for (int i = 0; i < 14; i++)
            {
                Kill(match, 2, i % 2 == 0 ? 1 : 3);
            }
            Assert.That(match.Phase, Is.EqualTo(MatchPhase.Playing));

            match.Damage(2, 0, 1000f, WeaponKind.Rifle, false);

            Assert.That(ended, Is.True);
            Assert.That(match.Phase, Is.EqualTo(MatchPhase.Ended));
            Assert.That(match.Winner, Is.SameAs(match.Get(2)));
            Assert.That(match.PlayerWon(), Is.False);
            Assert.That(match.ResultsReady, Is.False);
            match.Tick(5.01f);
            Assert.That(match.ResultsReady, Is.True);
            Assert.That(match.Damage(1, 0, 10f, WeaponKind.Rifle, false).Dealt, Is.EqualTo(0f), "no damage after the end");
        }

        [Test]
        public void ThePlayerWinsByReachingTheLimitFirst()
        {
            Match match = Playing(MatchMode.FreeForAll, 1);
            for (int i = 0; i < 15; i++)
            {
                match.Damage(0, 1, 1000f, WeaponKind.Rifle, false);
                match.Tick(3.1f);
                match.Tick(1.6f);
            }

            Assert.That(match.PlayerWon(), Is.True);
            Assert.That(match.PlaceOf(match.Player), Is.EqualTo(1));
        }

        [Test]
        public void ATimeLimitEndsTheMatchWithTheLeaderWinning()
        {
            Match match = Playing(MatchMode.FreeForAll, 2, 60f);
            Kill(match, 1, 0);
            Assert.That(match.TimeLeft, Is.LessThan(60f));

            match.Tick(70f);

            Assert.That(match.Phase, Is.EqualTo(MatchPhase.Ended));
            Assert.That(match.TimeRanOut, Is.True);
            Assert.That(match.Winner, Is.SameAs(match.Get(1)));
        }

        [Test]
        public void ATieWhenTheTimeRunsOutIsADraw()
        {
            Match match = Playing(MatchMode.FreeForAll, 1, 30f);

            match.Tick(31f);

            Assert.That(match.Winner, Is.Null);
            Assert.That(match.PlayerWon(), Is.False);
        }

        [Test]
        public void WithoutATimeLimitTheMatchGoesOn()
        {
            Match match = Playing(MatchMode.FreeForAll, 1);

            match.Tick(3600f);

            Assert.That(match.Phase, Is.EqualTo(MatchPhase.Playing));
            Assert.That(float.IsPositiveInfinity(match.TimeLeft), Is.True);
        }

        [Test]
        public void StandingsRankKillsThenFewerDeaths()
        {
            Match match = Playing(MatchMode.FreeForAll, 3);
            Kill(match, 1, 2);
            Kill(match, 3, 2);
            Kill(match, 2, 3);
            Kill(match, 3, 0);

            Fighter[] order = match.Standings().ToArray();

            Assert.That(order.Select(f => f.Id), Is.EqualTo(new[] { 3, 1, 2, 0 }));
            Assert.That(match.PlaceOf(match.Get(0)), Is.EqualTo(4));
        }

        [Test]
        public void TeammatesCannotHurtEachOtherInTeamDeathmatch()
        {
            Match match = Playing(MatchMode.TeamDeathmatch, 3);
            Fighter player = match.Player;
            Fighter mate = match.Fighters.First(f => f != player && f.Team == player.Team);
            Fighter enemy = match.Fighters.First(f => f.Team != player.Team);

            Assert.That(match.Damage(player.Id, mate.Id, 50f, WeaponKind.Rifle, false).Dealt, Is.EqualTo(0f));
            Assert.That(match.Damage(player.Id, enemy.Id, 50f, WeaponKind.Rifle, false).Dealt, Is.EqualTo(50f));
            Assert.That(match.AreEnemies(player, mate), Is.False);
            Assert.That(match.AreEnemies(player, enemy), Is.True);
        }

        [Test]
        public void ThirtyTeamKillsWinTeamDeathmatchForTheWholeTeam()
        {
            Match match = Playing(MatchMode.TeamDeathmatch, 3);
            Fighter player = match.Player;
            Fighter mate = match.Fighters.First(f => f != player && f.Team == player.Team);
            Fighter enemy = match.Fighters.First(f => f.Team != player.Team);
            for (int i = 0; i < 30; i++)
            {
                Assert.That(match.Phase, Is.EqualTo(MatchPhase.Playing));
                match.Damage(i % 2 == 0 ? player.Id : mate.Id, enemy.Id, 1000f, WeaponKind.Rifle, false);
                match.Tick(3.1f);
                match.Tick(1.6f);
            }

            Assert.That(match.TeamKills(player.Team), Is.EqualTo(30));
            Assert.That(match.Phase, Is.EqualTo(MatchPhase.Ended));
            Assert.That(match.WinningTeam, Is.EqualTo(player.Team));
            Assert.That(match.PlayerWon(), Is.True);
        }

        [Test]
        public void StreaksCountKillsWithoutDyingAndEarnBanners()
        {
            Match match = Playing(MatchMode.FreeForAll, 3);
            for (int i = 0; i < 5; i++)
            {
                Kill(match, 0, 1 + i % 3);
            }
            Assert.That(match.Player.Streak, Is.EqualTo(5));

            Kill(match, 1, 0);

            Assert.That(match.Player.Streak, Is.EqualTo(0));
            Assert.That(match.Player.BestStreak, Is.EqualTo(5));
            Assert.That(Match.StreakTitle(3), Is.EqualTo("KILLING SPREE"));
            Assert.That(Match.StreakTitle(5), Is.EqualTo("RAMPAGE"));
            Assert.That(Match.StreakTitle(4), Is.Null);
        }

        [Test]
        public void FreeForAllHasNoTeams()
        {
            Match match = Playing(MatchMode.FreeForAll, 4);

            Assert.That(match.Fighters.All(f => f.Team == 0), Is.True);
            Assert.That(match.AreEnemies(1, 2), Is.True);
        }
    }

    public class TeamBalanceTests
    {
        [Test]
        public void TeamsNeverDifferByMoreThanOneAndThePlayerIsOnTeamOne([Range(2, 10)] int count)
        {
            int[] teams = TeamBalance.Assign(count);

            int one = teams.Count(t => t == 1);
            int two = teams.Count(t => t == 2);
            Assert.That(one + two, Is.EqualTo(count));
            Assert.That(one - two, Is.InRange(0, 1));
            Assert.That(teams[0], Is.EqualTo(1));
        }

        [Test]
        public void ANewcomerJoinsTheSmallerTeam()
        {
            Assert.That(TeamBalance.Smaller(new[] { 1, 1, 2 }), Is.EqualTo(2));
            Assert.That(TeamBalance.Smaller(new[] { 1, 2, 2 }), Is.EqualTo(1));
            Assert.That(TeamBalance.Smaller(new[] { 1, 2 }), Is.EqualTo(1));
        }
    }
}
