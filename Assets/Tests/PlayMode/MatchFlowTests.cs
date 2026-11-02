using System.Collections;
using System.Linq;
using FlyingRush.Core;
using FlyingRush.Game;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace FlyingRush.Tests
{
    /// <summary>
    /// The game's flow in the real scene: the title, starting a match, shooting and killing a
    /// bot, dying and respawning, weapon pads, the kill limit and the results, pause, restart,
    /// team deathmatch, settings and the bots finding their way round the cage.
    /// </summary>
    public class MatchFlowTests
    {
        GameDirector director;
        VisualElement ui;

        IEnumerator Load()
        {
            SceneManager.LoadScene("Main");
            yield return null;
            yield return null;
            director = Object.FindAnyObjectByType<GameDirector>();
            Assert.That(director, Is.Not.Null);
            director.UseMemoryStore();
            director.Seed(1);
            director.ScriptedInput = true;
            ui = director.Hud.GetComponent<UIDocument>().rootVisualElement;
        }

        [TearDown]
        public void RestoreTime() => Time.timeScale = 1f;

        /// <summary>A short-countdown match, past its countdown and spawn protection.</summary>
        IEnumerator Fight(MatchMode mode = MatchMode.FreeForAll, int bots = 7, float roundEnd = 5f)
        {
            var rules = MatchSettings.For(mode);
            rules.CountdownSeconds = 0.5f;
            rules.RoundEndSeconds = roundEnd;
            director.StartMatch(rules, bots, BotDifficulty.Normal);
            director.ScriptedInput = true;
            yield return new WaitForSeconds(0.7f);
            Assert.That(director.Match.Phase, Is.EqualTo(MatchPhase.Playing));
            director.Match.Tick(Vitals.SpawnProtectionSeconds);
        }

        FighterBody You => director.Player.Body;

        static bool Visible(VisualElement element) => !element.ClassListContains("hidden");

        void Aim(FighterBody shooter, Vector3 point)
        {
            Vector3 to = point - shooter.Eye;
            shooter.Look(Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg, Mathf.Atan2(to.y, new Vector2(to.x, to.z).magnitude) * Mathf.Rad2Deg);
        }

        /// <summary>The first bot, standing alone in the open 10 m in front of the player.</summary>
        FighterBody Lineup()
        {
            director.BotsFrozen = true;
            You.Place(new Vector3(-31f, 0f, 3f), 90f);
            FighterBody bot = director.Bots[0].Body;
            bot.Place(new Vector3(-21f, 0f, 3f), 270f);
            for (int i = 1; i < director.Bots.Count; i++)
            {
                director.Bots[i].Body.Place(new Vector3(30f - i * 2f, 0f, 30f), 0f);
            }
            Aim(You, bot.Chest);
            return bot;
        }

        [UnityTest]
        public IEnumerator TheGameOpensOnTheTitleWithBotsFightingBehindTheMenu()
        {
            yield return Load();
            yield return new WaitForSeconds(1f);

            Assert.That(director.Screen, Is.EqualTo(GameScreen.Title));
            Assert.That(director.InDemo, Is.True);
            Assert.That(director.Player, Is.Null);
            Assert.That(director.Bots.Count, Is.EqualTo(6));
            Assert.That(Visible(ui.Q("menu")), Is.True);
            Assert.That(Visible(ui.Q("logo")), Is.True);
            Assert.That(Visible(ui.Q("play-hud")), Is.False);
            Assert.That(director.Hud.Menu.Items.Select(item => item.Id).Take(5),
                Is.EqualTo(new[] { "play", "mode", "bots", "difficulty", "timelimit" }));
        }

        [UnityTest]
        public IEnumerator PlayStartsAFreeForAllWithACountdownThenTheFight()
        {
            yield return Load();
            director.Activate("play");

            Assert.That(director.Screen, Is.EqualTo(GameScreen.Playing));
            Assert.That(director.InDemo, Is.False);
            Assert.That(director.Match.Settings.Mode, Is.EqualTo(MatchMode.FreeForAll));
            Assert.That(director.Match.Settings.KillLimit, Is.EqualTo(15));
            Assert.That(director.Match.Fighters.Count, Is.EqualTo(8), "you and seven bots");
            Assert.That(director.Match.Phase, Is.EqualTo(MatchPhase.Countdown));
            yield return null;
            Assert.That(director.Hud.BannerText, Is.EqualTo("3"));
            Assert.That(Visible(ui.Q("menu")), Is.False);
            Assert.That(Visible(ui.Q("play-hud")), Is.True);

            yield return new WaitForSeconds(3.2f);

            Assert.That(director.Match.Phase, Is.EqualTo(MatchPhase.Playing));
            Assert.That(director.Hud.BannerText, Is.EqualTo("FIGHT!"));
            Assert.That(director.Hud.WeaponName, Is.EqualTo("ASSAULT RIFLE"));
            Assert.That(director.Hud.AmmoText, Is.EqualTo("30 / --"));
            Assert.That(director.Hud.JumpPipsLit, Is.EqualTo(Jumps.MaxJumps));
            Assert.That(ui.Q<Label>("limit").text, Is.EqualTo("FIRST TO 15"));
        }

        [UnityTest]
        public IEnumerator ShootingABotHitsItThenKillsItAndTheFeedSaysSo()
        {
            yield return Load();
            yield return Fight();
            FighterBody bot = Lineup();
            yield return null;
            Aim(You, bot.Chest);

            director.Combat.Fire(You, You.AimDirection, true);
            yield return null;

            Assert.That(bot.Fighter.Vitals.Health, Is.LessThan(Vitals.MaxHealth));
            Assert.That(director.Hud.HitMarkerVisible, Is.True);
            Assert.That(You.Loadout.Current.Magazine, Is.EqualTo(29));

            for (int i = 0; i < 40 && bot.Fighter.Alive; i++)
            {
                yield return new WaitForSeconds(0.11f);
                Aim(You, bot.Chest);
                director.Combat.Fire(You, You.AimDirection, true);
            }
            yield return null;

            Assert.That(bot.Fighter.Alive, Is.False);
            Assert.That(director.Match.Player.Kills, Is.EqualTo(1));
            Assert.That(bot.Fighter.Deaths, Is.EqualTo(1));
            Assert.That(bot.gameObject.GetComponent<CharacterController>().enabled, Is.False, "the body is gone");
            Assert.That(director.Hud.FeedLines, Is.EqualTo(1));
            Assert.That(director.Hud.FeedText(0), Does.StartWith("YOU RIFLE").And.EndWith(bot.Fighter.Name));
            Assert.That(director.Hud.KillNoteText, Does.Contain("YOU ELIMINATED " + bot.Fighter.Name));
            Assert.That(ui.Q<Label>("left-score").text, Is.EqualTo("1"));

            yield return new WaitForSeconds(director.Match.Settings.RespawnSeconds + 0.3f);

            Assert.That(bot.Fighter.Alive, Is.True);
            Assert.That(bot.Fighter.Vitals.Health, Is.EqualTo(Vitals.MaxHealth));
            Assert.That(bot.GetComponent<CharacterController>().enabled, Is.True);
            Assert.That(director.Arena.Map.Spawns.Any(s => Vector3.Distance(s.Position.ToUnity(), bot.transform.position) < 2f), Is.True,
                "back at a spawn point");
        }

        [UnityTest]
        public IEnumerator ThePlayerDiesSeesTheRespawnTimerAndComesBack()
        {
            yield return Load();
            yield return Fight();
            int killer = director.Bots[0].Body.Fighter.Id;

            director.Match.Damage(killer, You.Fighter.Id, 1000f, WeaponKind.Sniper, true);
            yield return null;

            Assert.That(You.Fighter.Alive, Is.False);
            Assert.That(director.Hud.RespawnVisible, Is.True);
            Assert.That(ui.Q("respawn").Q<Label>(className: "respawn__by").text, Is.EqualTo("KILLED BY " + director.Bots[0].Body.Fighter.Name));
            Assert.That(director.Hud.BoardVisible, Is.True, "the scoreboard shows while you wait");
            Assert.That(director.Match.Player.Deaths, Is.EqualTo(1));

            yield return new WaitForSeconds(3.3f);

            Assert.That(You.Fighter.Alive, Is.True);
            Assert.That(director.Hud.HealthText, Is.EqualTo("150"));
            Assert.That(director.Hud.RespawnVisible, Is.False);
            Assert.That(director.Hud.BoardVisible, Is.False);
            Assert.That(You.Loadout.Current.Kind, Is.EqualTo(WeaponKind.Rifle));
        }

        [UnityTest]
        public IEnumerator BeingShotShowsADamageIndicatorAndLowersTheHealth()
        {
            yield return Load();
            yield return Fight();
            FighterBody bot = Lineup();
            Aim(bot, You.Chest);
            yield return null;

            director.Combat.Fire(bot, bot.AimDirection, true);
            yield return null;

            Assert.That(You.Fighter.Vitals.Health, Is.LessThan(Vitals.MaxHealth));
            Assert.That(director.Hud.VisibleIndicators, Is.EqualTo(1));
            Assert.That(int.Parse(director.Hud.HealthText), Is.LessThan(150));
        }

        [UnityTest]
        public IEnumerator WalkingOntoAWeaponPadPicksUpItsWeapon()
        {
            yield return Load();
            yield return Fight();
            PadView railgun = director.Arena.View.Pads.First(pad => pad.Pad.Kind == WeaponKind.Railgun);

            You.Place(railgun.transform.position + Vector3.up * 0.1f, 0f);
            yield return null;
            yield return null;

            Assert.That(You.Loadout.Current.Kind, Is.EqualTo(WeaponKind.Railgun));
            Assert.That(railgun.Pad.Available, Is.False);
            Assert.That(director.Hud.NoticeText, Is.EqualTo("PICKED UP THE RAILGUN"));
            Assert.That(director.Hud.WeaponName, Is.EqualTo("RAILGUN"));
            Assert.That(director.Hud.AmmoText, Is.EqualTo("3 / 9"));
        }

        [UnityTest]
        public IEnumerator FifteenKillsWinTheMatchAndTheResultsAreRecorded()
        {
            yield return Load();
            yield return Fight(roundEnd: 0.5f);
            director.BotsFrozen = true;
            Match match = director.Match;
            int you = You.Fighter.Id;
            for (int i = 0; i < 15; i++)
            {
                int victim = director.Bots[i % director.Bots.Count].Body.Fighter.Id;
                if (!match.Get(victim).Alive)
                {
                    yield return new WaitForSeconds(match.Get(victim).RespawnLeft + 0.1f);
                    match.Tick(Vitals.SpawnProtectionSeconds);
                }
                match.Damage(you, victim, 1000f, WeaponKind.Rifle, false);
            }
            yield return null;

            Assert.That(match.Phase, Is.EqualTo(MatchPhase.Ended));
            Assert.That(director.Hud.BannerText, Is.EqualTo("VICTORY"));
            Assert.That(Time.timeScale, Is.LessThan(1f), "a moment of slow motion");

            yield return new WaitForSecondsRealtime(2.2f);

            Assert.That(director.Screen, Is.EqualTo(GameScreen.Results));
            Assert.That(Time.timeScale, Is.EqualTo(1f));
            Assert.That(ui.Q<Label>("menu-title").text, Is.EqualTo("VICTORY"));
            Assert.That(ui.Q<Label>("menu-subtitle").text, Does.StartWith("1ST PLACE  -  15 KILLS"));
            Assert.That(director.Hud.Menu.Items.Select(item => item.Id), Is.EqualTo(new[] { "again", "title" }));
            Assert.That(director.Records.Matches(MatchMode.FreeForAll), Is.EqualTo(1));
            Assert.That(director.Records.Wins(MatchMode.FreeForAll), Is.EqualTo(1));
            Assert.That(director.Records.BestKills(MatchMode.FreeForAll), Is.EqualTo(15));
            Assert.That(director.LastRecord.NewBestKills, Is.True);
        }

        [UnityTest]
        public IEnumerator LeavingFromPauseDuringTheRoundEndStillRecordsTheMatch()
        {
            yield return Load();
            var rules = MatchSettings.For(MatchMode.FreeForAll);
            rules.KillLimit = 1;
            director.StartMatch(rules, 3, BotDifficulty.Normal);
            director.ScriptedInput = true;
            yield return new WaitForSeconds(rules.CountdownSeconds + 0.2f);
            Match match = director.Match;
            Fighter bot = match.Fighters.First(f => !f.IsPlayer);
            match.Tick(Vitals.SpawnProtectionSeconds);
            match.Damage(match.Player.Id, bot.Id, 999f, WeaponKind.Rifle, false);
            Assert.That(match.Phase, Is.EqualTo(MatchPhase.Ended));
            yield return null;

            director.TogglePause();
            Assert.That(director.Screen, Is.EqualTo(GameScreen.Paused));
            director.Activate("title");
            yield return null;

            Assert.That(director.Records.Matches(MatchMode.FreeForAll), Is.EqualTo(1), "the finished match is recorded");
            Assert.That(director.Records.Wins(MatchMode.FreeForAll), Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator PauseFreezesTheMatchAndResumeCarriesOn()
        {
            yield return Load();
            yield return Fight();

            director.TogglePause();
            float elapsed = director.Match.Elapsed;
            yield return new WaitForSecondsRealtime(0.5f);

            Assert.That(director.Screen, Is.EqualTo(GameScreen.Paused));
            Assert.That(Time.timeScale, Is.EqualTo(0f));
            Assert.That(director.Match.Elapsed, Is.EqualTo(elapsed));
            Assert.That(ui.Q<Label>("menu-title").text, Is.EqualTo("PAUSED"));
            Assert.That(director.Hud.Menu.Items.Select(item => item.Id), Is.EqualTo(new[] { "resume", "restart", "settings", "title" }));
            Assert.That(Visible(ui.Q("play-hud")), Is.False);

            director.Activate("resume");
            yield return new WaitForSeconds(0.3f);

            Assert.That(director.Screen, Is.EqualTo(GameScreen.Playing));
            Assert.That(Time.timeScale, Is.EqualTo(1f));
            Assert.That(director.Match.Elapsed, Is.GreaterThan(elapsed));
            Assert.That(Visible(ui.Q("menu")), Is.False);
        }

        [UnityTest]
        public IEnumerator RestartStartsAFreshMatch()
        {
            yield return Load();
            yield return Fight();
            Match first = director.Match;
            first.Damage(You.Fighter.Id, director.Bots[0].Body.Fighter.Id, 1000f, WeaponKind.Rifle, false);
            director.TogglePause();

            director.Activate("restart");
            yield return null;

            Assert.That(director.Match, Is.Not.SameAs(first));
            Assert.That(director.Screen, Is.EqualTo(GameScreen.Playing));
            Assert.That(Time.timeScale, Is.EqualTo(1f));
            Assert.That(director.Match.Fighters.Sum(f => f.Kills), Is.EqualTo(0));
            Assert.That(director.Match.Fighters.All(f => f.Alive), Is.True);
            Assert.That(director.Arena.Bodies.Count, Is.EqualTo(director.Match.Fighters.Count));
            Assert.That(Object.FindObjectsByType<FighterBody>(FindObjectsSortMode.None).Length, Is.EqualTo(director.Match.Fighters.Count),
                "the old bodies are gone");
        }

        [UnityTest]
        public IEnumerator TeamDeathmatchSplitsTheTeamsAtTheirBasesWithoutFriendlyFire()
        {
            yield return Load();
            yield return Fight(MatchMode.TeamDeathmatch);
            Match match = director.Match;

            Assert.That(match.Fighters.Count(f => f.Team == 1), Is.EqualTo(4));
            Assert.That(match.Fighters.Count(f => f.Team == 2), Is.EqualTo(4));
            Assert.That(match.Player.Team, Is.EqualTo(1));
            Assert.That(ui.Q<Label>("limit").text, Is.EqualTo("FIRST TEAM TO 30"));

            FighterBody mate = director.Arena.Bodies.First(b => b != You && b.Fighter.Team == 1);
            director.BotsFrozen = true;
            You.Place(new Vector3(-31f, 0f, 3f), 90f);
            mate.Place(new Vector3(-24f, 0f, 3f), 0f);
            Aim(You, mate.Chest);
            director.Combat.Fire(You, You.AimDirection, true);
            yield return null;

            Assert.That(mate.Fighter.Vitals.Health, Is.EqualTo(Vitals.MaxHealth));
        }

        [UnityTest]
        public IEnumerator TeamsSpawnAtTheirOwnBase()
        {
            yield return Load();
            director.Settings.Mode = MatchMode.TeamDeathmatch;
            director.StartMatch();
            yield return null;

            foreach (FighterBody body in director.Arena.Bodies)
            {
                float z = body.transform.position.z;
                Assert.That(body.Fighter.Team == 1 ? z < -28f : z > 28f, Is.True, $"{body.Fighter.Name} of team {body.Fighter.Team} at {z}");
            }
        }

        [UnityTest]
        public IEnumerator ChangingASettingAppliesAndKeepsIt()
        {
            yield return Load();
            director.Activate("settings");
            int volume = director.Settings.Volume;

            director.Activate("volume");
            director.Activate("shake");

            Assert.That(director.Settings.Volume, Is.EqualTo(volume + 1));
            Assert.That(director.Audio.Volume, Is.EqualTo((volume + 1) / 10f).Within(0.001f));
            Assert.That(director.Settings.ScreenShake, Is.False);
            Assert.That(director.Hud.Menu.Items.First(item => item.Id == "volume").Value, Is.EqualTo((volume + 1).ToString()));
            director.Activate("back");
            Assert.That(ui.Q("logo").ClassListContains("hidden"), Is.False, "back on the title");
        }

        [UnityTest]
        public IEnumerator BotsFindTheirWayRoundTheCageAndFight()
        {
            yield return Load();
            yield return Fight();
            director.BotsFrozen = false;
            You.Place(new Vector3(0f, 13f, 9.5f), 0f);
            var starts = director.Bots.Select(bot => bot.Body.transform.position).ToArray();

            yield return new WaitForSeconds(8f);

            int moved = director.Bots.Where((bot, i) => Vector3.Distance(bot.Body.transform.position, starts[i]) > 5f).Count();
            Assert.That(moved, Is.GreaterThanOrEqualTo(4), "bots roam, hunt and collect weapons");
            Assert.That(director.Bots.All(bot => !MapLayout.OutOfBounds(bot.Body.transform.position.ToCore())), Is.True);
            int shots = director.Bots.Count(bot => bot.Body.Loadout.Rifle.Magazine < 30 || bot.Body.Loadout.HasPower ||
                bot.Body.Fighter.Kills > 0 || bot.Body.Fighter.Deaths > 0);
            Assert.That(shots, Is.GreaterThan(0), "someone has fought or picked up a weapon");
        }

        [UnityTest]
        public IEnumerator FallingOutOfTheCageCountsAsADeath()
        {
            yield return Load();
            yield return Fight();
            FighterBody bot = director.Bots[0].Body;
            director.BotsFrozen = true;

            bot.Place(new Vector3(0f, -20f, 0f), 0f);
            yield return null;

            Assert.That(bot.Fighter.Alive, Is.False);
            Assert.That(bot.Fighter.Deaths, Is.EqualTo(1));
            Assert.That(director.Match.Feed.Last().Suicide, Is.True);
        }
    }
}
