using System.Collections.Generic;
using System.Linq;
using FlyingRush.Core;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace FlyingRush.Game
{
    public enum GameScreen
    {
        Title,
        Playing,
        Paused,
        Results,
    }

    /// <summary>
    /// Runs the game: builds the cage, keeps a bots-only demo match running behind the title,
    /// starts matches from the menu settings, spawns and respawns the fighters, picks up weapons,
    /// turns hits and kills into HUD feedback, sounds and screen shake, and shows the results and
    /// records at the end.
    /// </summary>
    public sealed class GameDirector : MonoBehaviour
    {
        static readonly string[] BotNames = { "VOLT", "RAZOR", "NOVA", "TALON", "BLITZ", "ECHO", "HEX", "ONYX", "VIPER" };
        const int DemoBots = 6;
        const float EndSlowMotionSeconds = 1.4f;

        [SerializeField] GameArt art;
        [SerializeField] Hud hud;

        readonly GameInput input = new GameInput();
        readonly SpawnPicker spawns = new SpawnPicker();
        readonly List<BotController> bots = new List<BotController>();
        readonly Stack<string> menuStack = new Stack<string>();
        IRecordStore store;
        Camera view;
        Effects effects;
        AudioDirector audioDirector;
        Transform fighterParent;
        Arena arena;
        PlayerController player;
        int seed = 12345;
        bool demo;
        float demoAngle;
        float slowMotionLeft;
        bool resultsShown;
        int lastCountdown;
        FighterBody lastKiller;
        RecordUpdate lastRecord;
        bool hadLock;

        public GameScreen Screen { get; private set; } = GameScreen.Title;
        public Match Match => arena?.Match;
        public Arena Arena => arena;
        public PlayerController Player => player;
        public IReadOnlyList<BotController> Bots => bots;
        public Hud Hud => hud;
        public GameSettings Settings { get; private set; }
        public Records Records { get; private set; }
        public Effects Effects => effects;
        public AudioDirector Audio => audioDirector;
        public Combat Combat => arena.Combat;
        /// <summary>The demo match behind the title is running.</summary>
        public bool InDemo => demo;
        /// <summary>When true, the player's body is not driven by the devices (tests and screenshots).</summary>
        public bool ScriptedInput { get; set; }
        public RecordUpdate LastRecord => lastRecord;
        /// <summary>Bots stand still (screenshots).</summary>
        public bool BotsFrozen { get; set; }
        /// <summary>The player aims down the sights without the button (screenshots).</summary>
        public bool ScriptedAim { get; set; }
        /// <summary>Shows the scoreboard as if Tab were held (screenshots and tests).</summary>
        public bool ShowScoreboard { get; set; }

        void Awake()
        {
            view = Camera.main;
            store = new PlayerPrefsStore();
            LoadPreferences();
            effects = new GameObject("Effects").AddComponent<Effects>();
            effects.Init(art);
            audioDirector = new GameObject("Audio").AddComponent<AudioDirector>();
            audioDirector.Init(Settings.Volume / 10f, Settings.MusicVolume / 10f);
            fighterParent = new GameObject("Fighters").transform;

            MapLayout layout = MapLayout.Cage();
            var mapView = new GameObject("Cage").AddComponent<MapView>();
            mapView.Build(layout, art);
            arena = new Arena { Map = layout, Graph = WaypointGraph.Build(layout), View = mapView };
            arena.Combat = new Combat(effects, audioDirector, art, () => arena.Match, BodyOf, new Rng(seed), transform);
            arena.Combat.Hit += OnHit;
            arena.Combat.Fired += OnFired;
            arena.Combat.Explosion += OnExplosion;
            hud.ItemClicked += index => { if (hud.Menu != null) { hud.Menu.Selected = index; Activate(hud.Menu.Items[index].Id); } };
            hud.ItemHovered += index => { if (hud.Menu != null && hud.Menu.Selected != index) { hud.Menu.Selected = index; hud.RefreshMenu(); } };
        }

        void Start() => ToTitle();

        void LoadPreferences()
        {
            Settings = GameSettings.Load(store);
            Records = new Records(store);
        }

        /// <summary>Keeps records and settings in memory only (tests and screenshots).</summary>
        public void UseMemoryStore()
        {
            store = new MemoryRecordStore();
            LoadPreferences();
            audioDirector.SetVolume(Settings.Volume / 10f, Settings.MusicVolume / 10f);
        }

        /// <summary>Seeds every random choice of the next match.</summary>
        public void Seed(int value) => seed = value;

        FighterBody BodyOf(Collider collider) => collider != null ? collider.GetComponent<FighterBody>() : null;

        // ---- Matches ----

        /// <summary>Starts a match with the menu's mode, bots, difficulty and time limit.</summary>
        public void StartMatch() => StartMatch(Settings.ToMatch(), Settings.Bots, Settings.Difficulty);

        public void StartMatch(MatchSettings rules, int botCount, BotDifficulty difficulty)
        {
            BotsFrozen = false;
            ScriptedAim = false;
            ShowScoreboard = false;
            BeginMatch(rules, botCount, difficulty, false);
            Screen = GameScreen.Playing;
            hud.HideMenu();
            hud.ShowPlay(true);
            hud.HideBanner();
            audioDirector.Silent = false;
            audioDirector.PlayMusic(art.BattleMusic);
            audioDirector.SetDucked(false);
            Time.timeScale = 1f;
            menuStack.Clear();
        }

        void BeginMatch(MatchSettings rules, int botCount, BotDifficulty difficulty, bool demoMatch)
        {
            ClearFighters();
            demo = demoMatch;
            resultsShown = false;
            lastKiller = null;
            slowMotionLeft = 0f;
            lastCountdown = -1;
            arena.View.ResetPads();
            arena.Combat.ClearRockets();
            var match = new Match(rules);
            arena.Match = match;
            int count = botCount + (demoMatch ? 0 : 1);
            int[] teams = TeamBalance.Assign(count);
            int teamIndex = 0;
            if (!demoMatch)
            {
                Fighter you = match.Add("YOU", teams[teamIndex++], true);
                FighterBody body = CreateBody(you, true);
                player = body.gameObject.AddComponent<PlayerController>();
                player.Init(body, view, art);
                player.BaseFov = Settings.Fov;
                player.ShakeEnabled = Settings.ScreenShake;
            }
            BotProfile profile = BotProfile.For(difficulty);
            for (int i = 0; i < botCount; i++)
            {
                Fighter fighter = match.Add(BotNames[i % BotNames.Length], teams[teamIndex++], false);
                FighterBody body = CreateBody(fighter, false);
                body.DamageScale = profile.DamageScale;
                BotController bot = body.gameObject.AddComponent<BotController>();
                bot.Init(body, profile, arena, seed * 31 + i * 977 + 1);
                bots.Add(bot);
            }
            foreach (FighterBody body in arena.Bodies)
            {
                Spawn(body);
            }
            match.Killed += OnKilled;
            match.Respawned += fighter => Spawn(BodyFor(fighter));
            match.Ended += OnMatchEnded;
            if (player != null)
            {
                player.SnapCamera();
            }
        }

        FighterBody CreateBody(Fighter fighter, bool firstPerson)
        {
            var host = new GameObject(fighter.Name);
            host.transform.SetParent(fighterParent, false);
            host.AddComponent<CharacterController>();
            FighterBody body = host.AddComponent<FighterBody>();
            body.Init(fighter, art, arena.Map, firstPerson);
            arena.Bodies.Add(body);
            return body;
        }

        FighterBody BodyFor(Fighter fighter) => arena.Bodies.FirstOrDefault(body => body.Fighter == fighter);

        void ClearFighters()
        {
            foreach (FighterBody body in arena.Bodies)
            {
                if (body != null)
                {
                    Destroy(body.gameObject);
                }
            }
            arena.Bodies.Clear();
            bots.Clear();
            player = null;
        }

        void Spawn(FighterBody body)
        {
            Match match = arena.Match;
            var enemies = new List<Vec3>();
            foreach (FighterBody other in arena.Bodies)
            {
                if (other != body && other.Fighter.Alive && other.gameObject.activeSelf && match.AreEnemies(body.Fighter, other.Fighter))
                {
                    enemies.Add(other.transform.position.ToCore());
                }
            }
            int team = match.Settings.Mode == MatchMode.TeamDeathmatch ? body.Fighter.Team : 0;
            int index = spawns.Pick(arena.Map.Spawns, team, enemies);
            SpawnPoint point = arena.Map.Spawns[Mathf.Max(0, index)];
            body.SetVisible(true);
            body.Place(point.Position.ToUnity() + Vector3.up * 0.05f, point.Yaw);
            body.ResetLoadout();
            body.ShowWeapon(WeaponKind.Rifle);
            body.GetComponent<BotController>()?.Reset();
            if (body.IsPlayer)
            {
                player.Killer = null;
                player.SnapCamera();
                hud.SetRespawn(false, string.Empty, string.Empty);
            }
        }

        public void ToTitle()
        {
            Time.timeScale = 1f;
            Screen = GameScreen.Title;
            var rules = new MatchSettings { Mode = MatchMode.FreeForAll, KillLimit = 9999, CountdownSeconds = 0f };
            BeginMatch(rules, DemoBots, BotDifficulty.Normal, true);
            audioDirector.Silent = true;
            audioDirector.PlayMusic(art.TitleMusic);
            audioDirector.SetDucked(false);
            hud.ShowPlay(false);
            hud.HideBanner();
            menuStack.Clear();
            OpenMenu("title");
        }

        public void TogglePause()
        {
            if (Screen == GameScreen.Playing)
            {
                Screen = GameScreen.Paused;
                Time.timeScale = 0f;
                audioDirector.SetDucked(true);
                audioDirector.Play(art.Pause, 0.7f);
                hud.ShowBoard(false, null, null);
                hud.ShowPlay(false);
                menuStack.Clear();
                OpenMenu("pause");
            }
            else if (Screen == GameScreen.Paused)
            {
                Screen = GameScreen.Playing;
                Time.timeScale = slowMotionLeft > 0f ? 0.35f : 1f;
                audioDirector.SetDucked(false);
                audioDirector.Play(art.Pause, 0.7f, 1.2f);
                menuStack.Clear();
                hud.HideMenu();
                hud.ShowPlay(true);
            }
        }

        public void Restart() => StartMatch();

        // ---- Frame ----

        void Update()
        {
            float dt = Time.deltaTime;
            input.Sensitivity = Settings.Sensitivity;
            input.InvertY = Settings.InvertY;
            bool locked = Screen == GameScreen.Playing && !ScriptedInput;
            // A browser gives up the pointer lock on Esc without passing the key on: pause then.
            if (locked && hadLock && Cursor.lockState != CursorLockMode.Locked && !Application.isBatchMode)
            {
                hadLock = false;
                TogglePause();
                return;
            }
            UpdateCursor(locked);
            hadLock = locked && Cursor.lockState == CursorLockMode.Locked;
            input.MouseLook = hadLock;
            input.Read();

            if (hud.MenuVisible)
            {
                NavigateMenu();
            }
            else if (Screen == GameScreen.Playing && input.Pause)
            {
                TogglePause();
                return;
            }

            Match match = arena.Match;
            if (match == null)
            {
                return;
            }
            bool fighting = match.Phase == MatchPhase.Playing && Screen != GameScreen.Paused;
            match.Tick(dt);
            arena.View.Tick(dt);
            foreach (BotController bot in bots)
            {
                bot.Tick(fighting && !BotsFrozen, dt);
                bot.Body.ShowWeapon(bot.Body.Loadout.Current.Kind);
                bot.Body.Loadout.Tick(dt);
            }
            if (player != null)
            {
                bool canAct = fighting && !ScriptedInput;
                if (ScriptedInput)
                {
                    player.Body.Drive(Vector3.zero, false, false, dt);
                }
                player.ForceAim = ScriptedAim;
                player.ShowViewmodel = Screen == GameScreen.Playing;
                player.Tick(input, arena.Combat, audioDirector, canAct, dt);
                player.Body.Loadout.Tick(dt);
            }
            CheckPadsAndBounds();

            if (demo)
            {
                OrbitCamera(dt);
            }
            else if (Screen == GameScreen.Playing || Screen == GameScreen.Paused)
            {
                UpdateCountdown(match);
                UpdateHud(match);
                if (slowMotionLeft > 0f)
                {
                    slowMotionLeft -= Time.unscaledDeltaTime;
                    if (slowMotionLeft <= 0f && Screen == GameScreen.Playing)
                    {
                        Time.timeScale = 1f;
                    }
                }
                if (match.ResultsReady && !resultsShown && Screen == GameScreen.Playing)
                {
                    ShowResults();
                }
            }
        }

        void UpdateCursor(bool locked)
        {
            if (Application.isBatchMode)
            {
                return;
            }
            CursorLockMode wanted = locked ? CursorLockMode.Locked : CursorLockMode.None;
            if (Cursor.lockState != wanted)
            {
                Cursor.lockState = wanted;
            }
            Cursor.visible = !locked;
        }

        void OrbitCamera(float dt)
        {
            demoAngle += dt * 6f;
            Quaternion turn = Quaternion.Euler(0f, demoAngle, 0f);
            Vector3 position = turn * new Vector3(0f, 17f, -34f);
            view.transform.position = position;
            view.transform.rotation = Quaternion.LookRotation(new Vector3(0f, 4f, 0f) - position);
            view.fieldOfView = 62f;
        }

        void CheckPadsAndBounds()
        {
            Match match = arena.Match;
            foreach (FighterBody body in arena.Bodies)
            {
                if (!body.Fighter.Alive)
                {
                    continue;
                }
                Vector3 feet = body.transform.position;
                if (MapLayout.OutOfBounds(feet.ToCore()))
                {
                    match.Fell(body.Fighter.Id);
                    continue;
                }
                foreach (PadView pad in arena.View.Pads)
                {
                    Vector3 at = pad.transform.position;
                    if (!pad.Pad.Available || Mathf.Abs(feet.y - at.y) > 1.6f ||
                        new Vector2(feet.x - at.x, feet.z - at.z).magnitude > PadView.PickupRadius)
                    {
                        continue;
                    }
                    pad.Pad.Take();
                    body.Loadout.PickUp(pad.Pad.Kind);
                    body.ShowWeapon(body.Loadout.Current.Kind);
                    if (body.IsPlayer)
                    {
                        audioDirector.Play(art.Pickup, 0.8f);
                        hud.Notice("PICKED UP THE " + WeaponCatalog.Of(pad.Pad.Kind).Name);
                    }
                    else
                    {
                        audioDirector.PlayAt(art.Pickup, at, 0.5f);
                    }
                }
            }
        }

        void UpdateCountdown(Match match)
        {
            if (match.Phase != MatchPhase.Countdown)
            {
                if (lastCountdown > 0 && match.Phase == MatchPhase.Playing)
                {
                    lastCountdown = 0;
                    hud.Banner("FIGHT!", "ELIMINATE THE ENEMIES", 1.2f, Palette.Amber);
                    audioDirector.Play(art.Go, 0.9f);
                }
                return;
            }
            int second = Mathf.CeilToInt(match.CountdownLeft);
            if (second != lastCountdown && second > 0)
            {
                lastCountdown = second;
                string mode = match.Settings.Mode == MatchMode.FreeForAll ? "FREE FOR ALL" : "TEAM DEATHMATCH";
                hud.Banner(second.ToString(), $"{mode}  -  FIRST TO {match.Settings.KillLimit}", 1.1f, Palette.Text);
                audioDirector.Play(art.Countdown, 0.8f);
            }
        }

        void UpdateHud(Match match)
        {
            Fighter you = player.Body.Fighter;
            Gun gun = player.Body.Loadout.Current;
            WeaponStats stats = gun.Stats;
            hud.SetVitals(you.Vitals.Health, you.Vitals.Fraction, player.Body.Jumps.Left);
            string reserve = gun.Reserve < 0 ? "--" : gun.Reserve.ToString();
            Gun other = player.Body.Loadout.HasPower
                ? (gun == player.Body.Loadout.Rifle ? player.Body.Loadout.Power : player.Body.Loadout.Rifle)
                : null;
            string alt = other != null ? $"[Q] {other.Stats.Name}" : string.Empty;
            hud.SetWeapon(stats.Name, Palette.Of(stats.Kind), $"{gun.Magazine} / {reserve}", gun.Magazine <= stats.Magazine / 4,
                gun.Reloading ? gun.ReloadProgress : 0f, gun.Reloading ? "RELOADING" : alt);

            bool scoped = player.Aiming && stats.Kind == WeaponKind.Sniper;
            hud.SetScope(scoped && you.Alive);
            float spread = (player.Aiming ? stats.AimSpread : stats.Spread) + (player.Body.Grounded ? 0f : 0.6f);
            float gap = Mathf.Tan(spread * Mathf.Deg2Rad) / Mathf.Tan(view.fieldOfView * 0.5f * Mathf.Deg2Rad) * 360f;
            hud.SetCrosshair(you.Alive && !scoped && Screen == GameScreen.Playing, gap + 5f);
            float hurt = you.Alive ? Mathf.Clamp01((0.5f - you.Vitals.Fraction) * 2f) : 0f;
            hud.SetVignette(hurt * (0.7f + 0.3f * Mathf.Sin(Time.unscaledTime * 6f)));

            // Scores.
            string clock = match.Settings.TimeLimit > 0f ? Clock(match.TimeLeft) : Clock(match.Elapsed);
            if (match.Settings.Mode == MatchMode.FreeForAll)
            {
                Fighter leader = match.Standings().First(f => f != you);
                bool leading = match.PlaceOf(you) == 1;
                hud.SetTopBar("FREE FOR ALL", "YOU", you.Kills.ToString(), Palette.Cyan, leading ? "2ND" : "LEADER", leader.Kills.ToString(),
                    Palette.FighterColor(leader), clock, $"FIRST TO {match.Settings.KillLimit}");
            }
            else
            {
                int ours = you.Team, theirs = 3 - you.Team;
                hud.SetTopBar("TEAM DEATHMATCH", "CYAN", match.TeamKills(ours).ToString(), Palette.TeamColor(ours), "CRIMSON",
                    match.TeamKills(theirs).ToString(), Palette.TeamColor(theirs), clock, $"FIRST TEAM TO {match.Settings.KillLimit}");
            }

            // Name of the enemy under the crosshair, teammate markers.
            string name = null;
            Color nameColor = Palette.Danger;
            if (you.Alive && Physics.Raycast(player.Body.Eye, player.Body.AimDirection, out RaycastHit hit, 70f, Layers.ShotMask, QueryTriggerInteraction.Ignore))
            {
                FighterBody seen = BodyOf(hit.collider);
                if (seen != null && seen != player.Body)
                {
                    name = seen.Fighter.Name;
                    nameColor = match.AreEnemies(you, seen.Fighter) ? Palette.Danger : Palette.Cyan;
                }
            }
            hud.SetTargetName(name, nameColor);
            var marks = new List<(Vector2, Color)>();
            if (match.Settings.Mode == MatchMode.TeamDeathmatch)
            {
                foreach (FighterBody body in arena.Bodies)
                {
                    if (body == player.Body || !body.Fighter.Alive || body.Fighter.Team != you.Team)
                    {
                        continue;
                    }
                    Vector3 above = body.transform.position + Vector3.up * (body.Height + 0.5f);
                    Vector3 local = view.WorldToViewportPoint(above);
                    if (local.z > 0f && local.x > 0f && local.x < 1f && local.y > 0f && local.y < 1f)
                    {
                        marks.Add((new Vector2(local.x, local.y), Palette.Cyan));
                    }
                }
            }
            hud.SetMarkers(marks);

            // Feed.
            var lines = new List<(string, Color, string, string, Color, bool)>();
            foreach (KillEvent kill in match.Feed.Reverse())
            {
                Fighter victim = match.Get(kill.VictimId);
                Fighter killer = match.Get(kill.KillerId);
                string weapon = kill.Suicide ? (kill.KillerId < 0 ? "FELL" : "SELF") : WeaponTag(kill.Weapon) + (kill.Headshot ? " +HS" : "");
                bool involved = victim.IsPlayer || (killer != null && killer.IsPlayer);
                lines.Add((kill.Suicide ? null : killer.Name, kill.Suicide ? Color.white : Palette.FighterColor(killer), weapon, victim.Name,
                    Palette.FighterColor(victim), involved));
            }
            hud.SetFeed(lines);

            // Respawn panel and scoreboard.
            if (!you.Alive && match.Phase == MatchPhase.Playing)
            {
                string by = lastKiller != null && lastKiller != player.Body ? $"KILLED BY {lastKiller.Fighter.Name}" : "YOU FELL";
                hud.SetRespawn(true, $"RESPAWNING IN {Mathf.CeilToInt(you.RespawnLeft)}", by);
            }
            else
            {
                hud.SetRespawn(false, string.Empty, string.Empty);
            }
            bool board = ShowScoreboard || (input.Scoreboard && !ScriptedInput) || (!you.Alive && match.Phase == MatchPhase.Playing);
            hud.ShowBoard(board && Screen == GameScreen.Playing, "SCOREBOARD", BoardRows(match));
        }

        static string WeaponTag(WeaponKind kind) => kind switch
        {
            WeaponKind.Shotgun => "SHOTGUN",
            WeaponKind.Sniper => "SNIPER",
            WeaponKind.Rockets => "ROCKET",
            WeaponKind.Railgun => "RAILGUN",
            _ => "RIFLE",
        };

        static string Clock(float seconds)
        {
            int total = Mathf.Max(0, Mathf.CeilToInt(seconds));
            return $"{total / 60}:{total % 60:00}";
        }

        public List<BoardRow> BoardRows(Match match)
        {
            var rows = new List<BoardRow>();
            int place = 1;
            foreach (Fighter fighter in match.Standings())
            {
                rows.Add(new BoardRow
                {
                    Place = place++,
                    Name = fighter.Name,
                    Kills = fighter.Kills,
                    Deaths = fighter.Deaths,
                    BestStreak = fighter.BestStreak,
                    Color = Palette.FighterColor(fighter),
                    You = fighter.IsPlayer,
                });
            }
            return rows;
        }

        // ---- Events ----

        void OnFired(FighterBody shooter, WeaponKind weapon)
        {
            AudioClip clip = weapon switch
            {
                WeaponKind.Shotgun => art.Shotgun,
                WeaponKind.Sniper => art.Sniper,
                WeaponKind.Railgun => art.Railgun,
                WeaponKind.Rockets => null,
                _ => art.Rifle,
            };
            if (shooter.IsPlayer)
            {
                audioDirector.Play(clip, weapon == WeaponKind.Rifle ? 0.45f : 0.75f);
            }
            else
            {
                audioDirector.PlayAt(clip, shooter.Eye, weapon == WeaponKind.Rifle ? 0.5f : 0.8f);
            }
        }

        void OnExplosion(Vector3 point, float radius)
        {
            if (player != null && player.Body.Fighter.Alive)
            {
                float distance = Vector3.Distance(point, player.Body.Chest);
                player.Shake(Mathf.Clamp01(1f - distance / (radius * 4f)) * 0.7f);
            }
        }

        void OnHit(HitInfo hit)
        {
            if (demo)
            {
                return;
            }
            if (hit.Attacker != null && hit.Attacker.IsPlayer && hit.Victim != hit.Attacker)
            {
                hud.HitMarker(hit.Killed, hit.Headshot);
                audioDirector.Play(hit.Headshot ? art.Headshot : art.Hit, hit.Headshot ? 0.7f : 0.45f, hit.Killed ? 0.8f : 1f);
            }
            if (hit.Victim.IsPlayer && hit.Damage > 0f)
            {
                Vector3 from = hit.Attacker != null && hit.Attacker != hit.Victim ? hit.Attacker.Eye : hit.Point;
                Vector3 to = from - player.Body.Eye;
                float angle = Vector3.SignedAngle(Quaternion.Euler(0f, player.Body.Yaw, 0f) * Vector3.forward, new Vector3(to.x, 0f, to.z), Vector3.up);
                hud.DamageFrom(angle);
                audioDirector.Play(art.Hurt, 0.6f);
                player.Shake(Mathf.Clamp01(hit.Damage / 120f) * 0.6f);
            }
            if (!hit.Victim.IsPlayer)
            {
                hit.Victim.GetComponent<BotController>()?.HurtBy(hit.Attacker);
            }
        }

        void OnKilled(KillEvent kill)
        {
            Match match = arena.Match;
            FighterBody victim = BodyFor(match.Get(kill.VictimId));
            FighterBody killer = kill.KillerId >= 0 ? BodyFor(match.Get(kill.KillerId)) : null;
            Vector3 push = killer != null ? (victim.Chest - killer.Eye).normalized * 6f : Vector3.up;
            effects.Burst(victim.Chest, victim.BodyMaterial, push + victim.Velocity);
            audioDirector.PlayAt(art.Death, victim.Chest, 0.8f);
            victim.SetVisible(false);
            if (demo)
            {
                return;
            }
            if (victim.IsPlayer)
            {
                lastKiller = killer;
                player.Killer = killer;
                hud.SetScope(false);
                player.Shake(0.5f);
            }
            if (killer != null && killer.IsPlayer && !kill.Suicide)
            {
                Fighter you = killer.Fighter;
                audioDirector.Play(art.Kill, 0.8f);
                hud.KillNote($"YOU ELIMINATED {victim.Fighter.Name}" + (kill.Headshot ? "  -  HEADSHOT" : ""),
                    Palette.FighterColor(victim.Fighter));
                string streak = Match.StreakTitle(you.Streak);
                if (streak != null && match.Phase == MatchPhase.Playing)
                {
                    hud.Banner(streak, $"{you.Streak} KILLS WITHOUT DYING", 1.6f, Palette.Gold);
                    audioDirector.Play(art.Streak, 0.8f);
                }
            }
        }

        void OnMatchEnded()
        {
            if (demo)
            {
                return;
            }
            Match match = arena.Match;
            bool won = match.PlayerWon();
            bool draw = match.Settings.Mode == MatchMode.FreeForAll ? match.Winner == null : match.WinningTeam == 0;
            string title = won ? "VICTORY" : draw ? "DRAW" : "DEFEAT";
            string sub = match.TimeRanOut ? "TIME IS UP" : match.Settings.Mode == MatchMode.FreeForAll
                ? $"{match.Winner?.Name ?? "NOBODY"} REACHED {match.Settings.KillLimit} KILLS"
                : $"{(match.WinningTeam == player.Body.Fighter.Team ? "CYAN" : "CRIMSON")} TEAM REACHED {match.Settings.KillLimit}";
            hud.Banner(title, sub, match.Settings.RoundEndSeconds, won ? Palette.Good : draw ? Palette.Amber : Palette.Danger);
            audioDirector.Play(won ? art.Win : art.Lose, 0.9f);
            Time.timeScale = 0.35f;
            slowMotionLeft = EndSlowMotionSeconds;
        }

        void ShowResults()
        {
            resultsShown = true;
            Time.timeScale = 1f;
            Screen = GameScreen.Results;
            hud.ShowBoard(false, null, null);
            hud.SetScope(false);
            hud.ShowPlay(false);
            Match match = arena.Match;
            lastRecord = Records.Submit(MatchSummary.Of(match));
            audioDirector.PlayMusic(art.TitleMusic);
            OpenMenu("results");
        }

        // ---- Menus ----

        void OpenMenu(string id)
        {
            menuStack.Push(id);
            hud.ShowMenu(BuildMenu(id));
        }

        void Back()
        {
            if (menuStack.Count <= 1)
            {
                if (Screen == GameScreen.Paused)
                {
                    TogglePause();
                }
                return;
            }
            menuStack.Pop();
            Settings.Save(store);
            hud.ShowMenu(BuildMenu(menuStack.Peek()));
            audioDirector.Play(art.Move, 0.5f, 0.9f);
        }

        MenuModel BuildMenu(string id)
        {
            var model = new MenuModel();
            string hint = input.Source == InputSource.Gamepad
                ? "D-PAD  CHOOSE     LEFT / RIGHT  CHANGE     A  SELECT     B  BACK"
                : "ARROWS / MOUSE  CHOOSE     LEFT / RIGHT  CHANGE     ENTER  SELECT     ESC  BACK";
            model.Hint = hint;
            switch (id)
            {
                case "title":
                    model.Logo = true;
                    model.Add("play", "PLAY");
                    model.Add("mode", "MODE", ModeName(Settings.Mode));
                    model.Add("bots", "BOTS", Settings.Bots.ToString());
                    model.Add("difficulty", "DIFFICULTY", Settings.Difficulty.ToString().ToUpperInvariant());
                    model.Add("timelimit", "TIME LIMIT", Settings.TimeLimitMinutes == 0 ? "NONE" : $"{Settings.TimeLimitMinutes} MIN");
                    model.Add("settings", "SETTINGS");
                    model.Add("records", "RECORDS");
                    model.Add("howto", "HOW TO PLAY");
                    if (!Application.isEditor && Application.platform != RuntimePlatform.WebGLPlayer)
                    {
                        model.Add("quit", "QUIT");
                    }
                    break;
                case "settings":
                    model.Title = "SETTINGS";
                    model.Add("sensitivity", "LOOK SENSITIVITY", Settings.Sensitivity.ToString());
                    model.Add("invert", "INVERT LOOK", Settings.InvertY ? "ON" : "OFF");
                    model.Add("fov", "FIELD OF VIEW", Settings.Fov.ToString());
                    model.Add("volume", "VOLUME", Settings.Volume.ToString());
                    model.Add("music", "MUSIC", Settings.MusicVolume.ToString());
                    model.Add("shake", "SCREEN SHAKE", Settings.ScreenShake ? "ON" : "OFF");
                    model.Add("back", "BACK");
                    model.LightDim = Screen == GameScreen.Paused;
                    break;
                case "records":
                    model.Title = "RECORDS";
                    foreach (MatchMode mode in new[] { MatchMode.FreeForAll, MatchMode.TeamDeathmatch })
                    {
                        string tag = mode == MatchMode.FreeForAll ? "FFA" : "TDM";
                        model.Body.Add(($"{tag}  MATCHES / WINS", $"{Records.Matches(mode)} / {Records.Wins(mode)}", false));
                        model.Body.Add(($"{tag}  MOST KILLS IN A MATCH", Records.BestKills(mode).ToString(), true));
                        model.Body.Add(($"{tag}  LONGEST STREAK", Records.BestStreak(mode).ToString(), true));
                        model.Body.Add(($"{tag}  KILLS PER DEATH", Records.KillDeathRatio(mode).ToString("0.00"), false));
                    }
                    model.Add("back", "BACK");
                    break;
                case "howto":
                    model.Title = "HOW TO PLAY";
                    model.Subtitle = "Free for all: first to 15 kills, as in the 2021 game. Team deathmatch: first team to 30.";
                    model.Body.Add(("MOVE / LOOK", "WASD + MOUSE  -  LEFT + RIGHT STICK", false));
                    model.Body.Add(("JUMP, UP TO 7 TIMES IN THE AIR", "SPACE  -  A / LB", true));
                    model.Body.Add(("CROUCH (THE TUNNELS)", "CTRL OR C  -  B", false));
                    model.Body.Add(("SHOOT / AIM", "MOUSE BUTTONS  -  RT / LT", false));
                    model.Body.Add(("RELOAD / SWITCH WEAPON", "R / Q OR WHEEL  -  X / Y OR RB", false));
                    model.Body.Add(("SCOREBOARD / PAUSE", "TAB / ESC  -  VIEW / START", false));
                    model.Body.Add(("POWER WEAPONS", "ON PADS UP HIGH; SHOTGUNS IN THE TUNNELS", true));
                    model.Body.Add(("HEALTH", "150, COMES BACK AFTER 5 S WITHOUT DAMAGE", false));
                    model.Add("back", "BACK");
                    break;
                case "pause":
                    model.Title = "PAUSED";
                    model.LightDim = true;
                    model.Add("resume", "RESUME");
                    model.Add("restart", "RESTART MATCH");
                    model.Add("settings", "SETTINGS");
                    model.Add("title", "QUIT TO TITLE");
                    break;
                case "results":
                {
                    Match match = arena.Match;
                    bool won = match.PlayerWon();
                    bool draw = match.Settings.Mode == MatchMode.FreeForAll ? match.Winner == null : match.WinningTeam == 0;
                    model.Title = won ? "VICTORY" : draw ? "DRAW" : "DEFEAT";
                    model.TitleTone = won ? 1 : draw ? 0 : -1;
                    Fighter you = match.Player;
                    string place = match.Settings.Mode == MatchMode.FreeForAll ? $"{Ordinal(match.PlaceOf(you))} PLACE  -  " : string.Empty;
                    string best = lastRecord.NewBestKills ? "  -  NEW BEST!" : string.Empty;
                    model.Subtitle = $"{place}{you.Kills} KILLS, {you.Deaths} DEATHS, BEST STREAK {you.BestStreak}{best}";
                    model.Board = BoardRows(match);
                    model.LightDim = true;
                    model.Add("again", "PLAY AGAIN");
                    model.Add("title", "MAIN MENU");
                    break;
                }
            }
            return model;
        }

        static string ModeName(MatchMode mode) => mode == MatchMode.FreeForAll ? "FREE FOR ALL" : "TEAM DEATHMATCH";

        static string Ordinal(int place) => place switch
        {
            1 => "1ST",
            2 => "2ND",
            3 => "3RD",
            _ => place + "TH",
        };

        void NavigateMenu()
        {
            MenuModel menu = hud.Menu;
            if (menu == null || menu.Items.Count == 0)
            {
                return;
            }
            int vertical = input.VerticalStep();
            if (vertical != 0)
            {
                menu.Selected = (menu.Selected + vertical + menu.Items.Count) % menu.Items.Count;
                hud.RefreshMenu();
                audioDirector.Play(art.Move, 0.5f);
            }
            int horizontal = input.HorizontalStep();
            if (horizontal != 0 && Change(menu.Items[menu.Selected].Id, horizontal))
            {
                audioDirector.Play(art.Move, 0.5f, 1.1f);
            }
            if (input.Confirm)
            {
                Activate(menu.Items[menu.Selected].Id);
            }
            else if (input.Back || (input.Pause && Screen == GameScreen.Paused && menuStack.Count == 1))
            {
                Back();
            }
        }

        /// <summary>Steps a setting left or right; false when the row is not a setting.</summary>
        bool Change(string id, int step)
        {
            switch (id)
            {
                case "mode": Settings.StepMode(step); break;
                case "bots": Settings.StepBots(step); break;
                case "difficulty": Settings.StepDifficulty(step); break;
                case "timelimit": Settings.StepTimeLimit(step); break;
                case "sensitivity": Settings.StepSensitivity(step); break;
                case "invert": Settings.InvertY = !Settings.InvertY; break;
                case "fov": Settings.StepFov(step); break;
                case "volume": Settings.StepVolume(step); break;
                case "music": Settings.StepMusic(step); break;
                case "shake": Settings.ScreenShake = !Settings.ScreenShake; break;
                default: return false;
            }
            ApplySettings();
            MenuModel fresh = BuildMenu(menuStack.Peek());
            fresh.Selected = hud.Menu.Selected;
            hud.ShowMenu(fresh);
            return true;
        }

        void ApplySettings()
        {
            audioDirector.SetVolume(Settings.Volume / 10f, Settings.MusicVolume / 10f);
            if (player != null)
            {
                player.BaseFov = Settings.Fov;
                player.ShakeEnabled = Settings.ScreenShake;
            }
            Settings.Save(store);
        }

        /// <summary>Runs a menu row's action, as if it had been chosen.</summary>
        public void Activate(string id)
        {
            audioDirector.Play(art.Select, 0.6f);
            switch (id)
            {
                case "play":
                case "again":
                case "restart":
                    Settings.Save(store);
                    StartMatch();
                    break;
                case "settings":
                case "records":
                case "howto":
                    OpenMenu(id);
                    break;
                case "back":
                    Back();
                    break;
                case "resume":
                    TogglePause();
                    break;
                case "title":
                    ToTitle();
                    break;
                case "quit":
                    Application.Quit();
                    break;
                default:
                    Change(id, 1);
                    break;
            }
        }
    }
}
