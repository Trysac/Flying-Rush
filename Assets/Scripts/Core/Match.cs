using System;
using System.Collections.Generic;
using System.Linq;

namespace FlyingRush.Core
{
    public enum MatchMode
    {
        /// <summary>Everyone against everyone, the 2021 game's mode.</summary>
        FreeForAll,
        /// <summary>Two teams of bots and you, one base each.</summary>
        TeamDeathmatch,
    }

    public enum MatchPhase
    {
        Countdown,
        Playing,
        Ended,
    }

    /// <summary>The rules of a match. The defaults are the 2021 game's settings.</summary>
    public sealed class MatchSettings
    {
        public const int FreeForAllKillLimit = 15;
        public const int TeamKillLimit = 30;

        public MatchMode Mode = MatchMode.FreeForAll;
        /// <summary>Kills (or team kills) that win the match.</summary>
        public int KillLimit = FreeForAllKillLimit;
        /// <summary>Seconds of play before the leader wins; 0 for no limit, as in 2021.</summary>
        public float TimeLimit;
        public float CountdownSeconds = 3f;
        public float RespawnSeconds = 3f;
        /// <summary>The pause after the last kill before the results show.</summary>
        public float RoundEndSeconds = 5f;

        public static MatchSettings For(MatchMode mode, float timeLimitSeconds = 0f) => new MatchSettings
        {
            Mode = mode,
            KillLimit = mode == MatchMode.FreeForAll ? FreeForAllKillLimit : TeamKillLimit,
            TimeLimit = timeLimitSeconds,
        };
    }

    /// <summary>One player or bot in the match.</summary>
    public sealed class Fighter
    {
        internal float respawnLeft;

        public Fighter(int id, string name, int team, bool isPlayer)
        {
            Id = id;
            Name = name;
            Team = team;
            IsPlayer = isPlayer;
        }

        public int Id { get; }
        public string Name { get; }
        /// <summary>0 in free for all, 1 or 2 in team deathmatch.</summary>
        public int Team { get; }
        public bool IsPlayer { get; }
        public Vitals Vitals { get; } = new Vitals();
        public int Kills { get; internal set; }
        public int Deaths { get; internal set; }
        public int Streak { get; internal set; }
        public int BestStreak { get; internal set; }
        public bool Alive => Vitals.Alive;

        /// <summary>Seconds until this fighter comes back; 0 while alive.</summary>
        public float RespawnLeft => Alive ? 0f : Math.Max(0f, respawnLeft);
    }

    /// <summary>A line of the kill feed.</summary>
    public readonly struct KillEvent
    {
        /// <summary>-1 when nobody gets the kill (a fall out of the cage).</summary>
        public readonly int KillerId;
        public readonly int VictimId;
        public readonly WeaponKind Weapon;
        public readonly bool Headshot;
        public readonly float Time;

        public KillEvent(int killerId, int victimId, WeaponKind weapon, bool headshot, float time)
        {
            KillerId = killerId;
            VictimId = victimId;
            Weapon = weapon;
            Headshot = headshot;
            Time = time;
        }

        public bool Suicide => KillerId < 0 || KillerId == VictimId;
    }

    /// <summary>
    /// A deathmatch round: a countdown, the fight, and its end when someone (or a team) reaches
    /// the kill limit or the time runs out. It keeps the scores, streaks, kill feed and respawn
    /// timers; the game moves the bodies.
    /// </summary>
    public sealed class Match
    {
        public const int FeedLength = 5;

        readonly List<Fighter> fighters = new List<Fighter>();
        readonly List<KillEvent> feed = new List<KillEvent>();
        float countdown;
        float endTimer;

        public Match(MatchSettings settings)
        {
            Settings = settings;
            countdown = settings.CountdownSeconds;
            Phase = countdown > 0f ? MatchPhase.Countdown : MatchPhase.Playing;
        }

        public MatchSettings Settings { get; }
        public MatchPhase Phase { get; private set; }
        public IReadOnlyList<Fighter> Fighters => fighters;
        /// <summary>The latest kills, oldest first, at most <see cref="FeedLength"/>.</summary>
        public IReadOnlyList<KillEvent> Feed => feed;
        /// <summary>Seconds of play so far (the countdown does not count).</summary>
        public float Elapsed { get; private set; }
        public float CountdownLeft => Math.Max(0f, countdown);
        /// <summary>The winning fighter in free for all, once ended (null for a draw).</summary>
        public Fighter Winner { get; private set; }
        /// <summary>The winning team in team deathmatch, once ended (0 for a draw).</summary>
        public int WinningTeam { get; private set; }
        public bool TimeRanOut { get; private set; }
        /// <summary>True from the moment the limit is reached; the results show after the round end pause.</summary>
        public bool ResultsReady => Phase == MatchPhase.Ended && endTimer <= 0f;

        public float TimeLeft => Settings.TimeLimit > 0f ? Math.Max(0f, Settings.TimeLimit - Elapsed) : float.PositiveInfinity;

        public event Action<KillEvent> Killed;
        public event Action<Fighter> Respawned;
        public event Action Started;
        public event Action Ended;

        public Fighter Add(string name, int team, bool isPlayer)
        {
            var fighter = new Fighter(fighters.Count, name, Settings.Mode == MatchMode.FreeForAll ? 0 : team, isPlayer);
            fighters.Add(fighter);
            return fighter;
        }

        public Fighter Get(int id) => id >= 0 && id < fighters.Count ? fighters[id] : null;

        public Fighter Player => fighters.FirstOrDefault(f => f.IsPlayer);

        public int TeamKills(int team) => fighters.Where(f => f.Team == team).Sum(f => f.Kills);

        /// <summary>True when two fighters may hurt each other.</summary>
        public bool AreEnemies(Fighter a, Fighter b) =>
            a != b && (Settings.Mode == MatchMode.FreeForAll || a.Team != b.Team);

        public bool AreEnemies(int a, int b) => AreEnemies(Get(a), Get(b));

        /// <summary>Kills hurt only while playing; friendly fire is off, your own splash is on.</summary>
        public DamageResult Damage(int attackerId, int victimId, float amount, WeaponKind weapon, bool headshot)
        {
            Fighter victim = Get(victimId);
            Fighter attacker = Get(attackerId);
            if (Phase != MatchPhase.Playing || victim == null || !victim.Alive)
            {
                return new DamageResult(0f, false, false);
            }
            if (attacker != null && attacker != victim && !AreEnemies(attacker, victim))
            {
                return new DamageResult(0f, false, false);
            }
            DamageResult result = victim.Vitals.Hurt(amount);
            if (result.Killed)
            {
                RecordKill(attackerId, victim, weapon, headshot);
            }
            return result;
        }

        /// <summary>A fighter fell out of the cage.</summary>
        public void Fell(int victimId)
        {
            Fighter victim = Get(victimId);
            if (Phase == MatchPhase.Ended || victim == null || !victim.Alive)
            {
                return;
            }
            victim.Vitals.Kill();
            RecordKill(-1, victim, WeaponKind.Rifle, false);
        }

        void RecordKill(int killerId, Fighter victim, WeaponKind weapon, bool headshot)
        {
            var kill = new KillEvent(killerId, victim.Id, weapon, headshot, Elapsed);
            victim.Deaths++;
            victim.Streak = 0;
            victim.respawnLeft = Settings.RespawnSeconds;
            Fighter killer = Get(killerId);
            if (!kill.Suicide && killer != null)
            {
                killer.Kills++;
                killer.Streak++;
                killer.BestStreak = Math.Max(killer.BestStreak, killer.Streak);
            }
            feed.Add(kill);
            if (feed.Count > FeedLength)
            {
                feed.RemoveAt(0);
            }
            Killed?.Invoke(kill);
            if (Phase == MatchPhase.Playing && LimitReached())
            {
                End(false);
            }
        }

        bool LimitReached()
        {
            if (Settings.Mode == MatchMode.FreeForAll)
            {
                return fighters.Any(f => f.Kills >= Settings.KillLimit);
            }
            return TeamKills(1) >= Settings.KillLimit || TeamKills(2) >= Settings.KillLimit;
        }

        public void Tick(float deltaTime)
        {
            switch (Phase)
            {
                case MatchPhase.Countdown:
                    countdown -= deltaTime;
                    if (countdown <= 0f)
                    {
                        Phase = MatchPhase.Playing;
                        Started?.Invoke();
                    }
                    return;
                case MatchPhase.Ended:
                    endTimer -= deltaTime;
                    return;
            }

            Elapsed += deltaTime;
            foreach (Fighter fighter in fighters)
            {
                if (fighter.Alive)
                {
                    fighter.Vitals.Tick(deltaTime);
                }
                else
                {
                    fighter.respawnLeft -= deltaTime;
                    if (fighter.respawnLeft <= 0f)
                    {
                        fighter.respawnLeft = 0f;
                        fighter.Vitals.Revive();
                        Respawned?.Invoke(fighter);
                    }
                }
            }
            if (Settings.TimeLimit > 0f && Elapsed >= Settings.TimeLimit)
            {
                End(true);
            }
        }

        void End(bool timeUp)
        {
            Phase = MatchPhase.Ended;
            TimeRanOut = timeUp;
            endTimer = Settings.RoundEndSeconds;
            if (Settings.Mode == MatchMode.FreeForAll)
            {
                List<Fighter> order = Standings().ToList();
                bool tie = order.Count > 1 && order[0].Kills == order[1].Kills && order[0].Deaths == order[1].Deaths;
                Winner = tie ? null : order[0];
            }
            else
            {
                int red = TeamKills(1), blue = TeamKills(2);
                WinningTeam = red == blue ? 0 : red > blue ? 1 : 2;
            }
            Ended?.Invoke();
        }

        /// <summary>Most kills first; fewer deaths break a tie, then whoever joined first.</summary>
        public IEnumerable<Fighter> Standings() =>
            fighters.OrderByDescending(f => f.Kills).ThenBy(f => f.Deaths).ThenBy(f => f.Id);

        /// <summary>1-based place of a fighter in the standings.</summary>
        public int PlaceOf(Fighter fighter) => Standings().TakeWhile(f => f != fighter).Count() + 1;

        /// <summary>Whether the player won: first place, or on the winning team.</summary>
        public bool PlayerWon()
        {
            Fighter player = Player;
            if (Phase != MatchPhase.Ended || player == null)
            {
                return false;
            }
            return Settings.Mode == MatchMode.FreeForAll ? Winner == player : WinningTeam == player.Team;
        }

        /// <summary>The name of a streak worth a banner, or null.</summary>
        public static string StreakTitle(int streak) => streak switch
        {
            3 => "KILLING SPREE",
            5 => "RAMPAGE",
            8 => "UNSTOPPABLE",
            12 => "GODLIKE",
            _ => null,
        };
    }

    /// <summary>Splits fighters into two teams of equal size (one more on team 1 if odd).</summary>
    public static class TeamBalance
    {
        /// <summary>The team (1 or 2) of each of <paramref name="count"/> fighters, in order; the first is the player.</summary>
        public static int[] Assign(int count)
        {
            var teams = new int[count];
            for (int i = 0; i < count; i++)
            {
                teams[i] = i % 2 == 0 ? 1 : 2;
            }
            return teams;
        }

        /// <summary>The team a new fighter should join: the smaller one, team 1 on a tie.</summary>
        public static int Smaller(IEnumerable<int> teams)
        {
            int one = 0, two = 0;
            foreach (int team in teams)
            {
                if (team == 1) one++;
                else if (team == 2) two++;
            }
            return two < one ? 2 : 1;
        }
    }
}
