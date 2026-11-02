using System;
using System.Collections.Generic;

namespace FlyingRush.Core
{
    /// <summary>Where records and settings are kept (PlayerPrefs in the game, memory in tests).</summary>
    public interface IRecordStore
    {
        int GetInt(string key, int fallback);
        void SetInt(string key, int value);
        void Save();
    }

    public sealed class MemoryRecordStore : IRecordStore
    {
        readonly Dictionary<string, int> values = new Dictionary<string, int>();

        public int GetInt(string key, int fallback) => values.TryGetValue(key, out int value) ? value : fallback;
        public void SetInt(string key, int value) => values[key] = value;
        public void Save() => SaveCount++;

        /// <summary>How many times the store was flushed.</summary>
        public int SaveCount { get; private set; }
    }

    /// <summary>The player's line of a finished match.</summary>
    public readonly struct MatchSummary
    {
        public readonly MatchMode Mode;
        public readonly bool Won;
        public readonly int Kills;
        public readonly int Deaths;
        public readonly int BestStreak;
        public readonly int Place;

        public MatchSummary(MatchMode mode, bool won, int kills, int deaths, int bestStreak, int place)
        {
            Mode = mode;
            Won = won;
            Kills = kills;
            Deaths = deaths;
            BestStreak = bestStreak;
            Place = place;
        }

        public static MatchSummary Of(Match match)
        {
            Fighter player = match.Player;
            return new MatchSummary(match.Settings.Mode, match.PlayerWon(), player.Kills, player.Deaths,
                player.BestStreak, match.PlaceOf(player));
        }
    }

    /// <summary>What a finished match changed in the records.</summary>
    public readonly struct RecordUpdate
    {
        public readonly bool NewBestKills;
        public readonly bool NewBestStreak;
        public readonly int PreviousBestKills;

        public RecordUpdate(bool newBestKills, bool newBestStreak, int previousBestKills)
        {
            NewBestKills = newBestKills;
            NewBestStreak = newBestStreak;
            PreviousBestKills = previousBestKills;
        }
    }

    /// <summary>The player's best results on the device, for each mode.</summary>
    public sealed class Records
    {
        const string Prefix = "flyingrush.records.";

        readonly IRecordStore store;

        public Records(IRecordStore store) => this.store = store;

        static string Key(MatchMode mode, string name) => $"{Prefix}{(mode == MatchMode.FreeForAll ? "ffa" : "tdm")}.{name}";

        public int Matches(MatchMode mode) => store.GetInt(Key(mode, "matches"), 0);
        public int Wins(MatchMode mode) => store.GetInt(Key(mode, "wins"), 0);
        public int TotalKills(MatchMode mode) => store.GetInt(Key(mode, "kills"), 0);
        public int TotalDeaths(MatchMode mode) => store.GetInt(Key(mode, "deaths"), 0);
        public int BestKills(MatchMode mode) => store.GetInt(Key(mode, "bestkills"), 0);
        public int BestStreak(MatchMode mode) => store.GetInt(Key(mode, "beststreak"), 0);

        /// <summary>Kills per death over every match of a mode (kills alone when never killed).</summary>
        public float KillDeathRatio(MatchMode mode)
        {
            int deaths = TotalDeaths(mode);
            return deaths == 0 ? TotalKills(mode) : TotalKills(mode) / (float)deaths;
        }

        /// <summary>Adds a finished match and saves the store once.</summary>
        public RecordUpdate Submit(MatchSummary summary)
        {
            MatchMode mode = summary.Mode;
            int previous = BestKills(mode);
            bool newKills = summary.Kills > previous;
            if (newKills)
            {
                store.SetInt(Key(mode, "bestkills"), summary.Kills);
            }
            bool newStreak = summary.BestStreak > BestStreak(mode);
            if (newStreak)
            {
                store.SetInt(Key(mode, "beststreak"), summary.BestStreak);
            }
            store.SetInt(Key(mode, "matches"), Matches(mode) + 1);
            store.SetInt(Key(mode, "wins"), Wins(mode) + (summary.Won ? 1 : 0));
            store.SetInt(Key(mode, "kills"), TotalKills(mode) + summary.Kills);
            store.SetInt(Key(mode, "deaths"), TotalDeaths(mode) + summary.Deaths);
            store.Save();
            return new RecordUpdate(newKills, newStreak, previous);
        }
    }

    /// <summary>The settings menu and the last match setup, kept on the device between sessions.</summary>
    public sealed class GameSettings
    {
        const string Prefix = "flyingrush.settings.";
        public const int MaxVolume = 10;
        public const int MinSensitivity = 1;
        public const int MaxSensitivity = 20;
        public const int MinFov = 70;
        public const int MaxFov = 100;
        public static readonly int[] BotCounts = { 3, 5, 7 };
        /// <summary>Time limits offered, in minutes; 0 is none, as in 2021.</summary>
        public static readonly int[] TimeLimits = { 0, 5, 10 };

        /// <summary>Mouse and stick sensitivity, 1..20.</summary>
        public int Sensitivity = 8;
        public bool InvertY;
        public int Volume = 8;
        public int MusicVolume = 6;
        public int Fov = 85;
        public bool ScreenShake = true;
        public BotDifficulty Difficulty = BotDifficulty.Normal;
        public MatchMode Mode = MatchMode.FreeForAll;
        public int Bots = 7;
        public int TimeLimitMinutes = 10;

        /// <summary>Degrees of view turn per mouse count.</summary>
        public float MouseDegreesPerCount => 0.012f * Sensitivity;

        public void StepSensitivity(int step) => Sensitivity = Clamp(Sensitivity + step, MinSensitivity, MaxSensitivity);
        public void StepVolume(int step) => Volume = Clamp(Volume + step, 0, MaxVolume);
        public void StepMusic(int step) => MusicVolume = Clamp(MusicVolume + step, 0, MaxVolume);
        public void StepFov(int step) => Fov = Clamp(Fov + step * 5, MinFov, MaxFov);
        public void StepDifficulty(int step) => Difficulty = (BotDifficulty)Wrap((int)Difficulty + step, 3);
        public void StepMode(int step) => Mode = (MatchMode)Wrap((int)Mode + step, 2);
        public void StepBots(int step) => Bots = BotCounts[Wrap(Array.IndexOf(BotCounts, Bots) + step, BotCounts.Length)];
        public void StepTimeLimit(int step) =>
            TimeLimitMinutes = TimeLimits[Wrap(Array.IndexOf(TimeLimits, TimeLimitMinutes) + step, TimeLimits.Length)];

        /// <summary>The match these settings set up.</summary>
        public MatchSettings ToMatch() => MatchSettings.For(Mode, TimeLimitMinutes * 60f);

        static int Clamp(int value, int min, int max) => value < min ? min : value > max ? max : value;

        static int Wrap(int value, int count) => ((value % count) + count) % count;

        /// <summary>Reads the settings, falling back to the defaults for missing or broken values.</summary>
        public static GameSettings Load(IRecordStore store)
        {
            var d = new GameSettings();
            int bots = store.GetInt(Prefix + "bots", d.Bots);
            int limit = store.GetInt(Prefix + "timelimit", d.TimeLimitMinutes);
            int fov = store.GetInt(Prefix + "fov", d.Fov);
            return new GameSettings
            {
                Sensitivity = Clamp(store.GetInt(Prefix + "sensitivity", d.Sensitivity), MinSensitivity, MaxSensitivity),
                InvertY = store.GetInt(Prefix + "invert", 0) == 1,
                Volume = Clamp(store.GetInt(Prefix + "volume", d.Volume), 0, MaxVolume),
                MusicVolume = Clamp(store.GetInt(Prefix + "music", d.MusicVolume), 0, MaxVolume),
                Fov = Clamp(fov - fov % 5, MinFov, MaxFov),
                ScreenShake = store.GetInt(Prefix + "shake", 1) == 1,
                Difficulty = (BotDifficulty)Clamp(store.GetInt(Prefix + "difficulty", (int)d.Difficulty), 0, 2),
                Mode = (MatchMode)Clamp(store.GetInt(Prefix + "mode", (int)d.Mode), 0, 1),
                Bots = Array.IndexOf(BotCounts, bots) >= 0 ? bots : d.Bots,
                TimeLimitMinutes = Array.IndexOf(TimeLimits, limit) >= 0 ? limit : d.TimeLimitMinutes,
            };
        }

        public void Save(IRecordStore store)
        {
            store.SetInt(Prefix + "sensitivity", Sensitivity);
            store.SetInt(Prefix + "invert", InvertY ? 1 : 0);
            store.SetInt(Prefix + "volume", Volume);
            store.SetInt(Prefix + "music", MusicVolume);
            store.SetInt(Prefix + "fov", Fov);
            store.SetInt(Prefix + "shake", ScreenShake ? 1 : 0);
            store.SetInt(Prefix + "difficulty", (int)Difficulty);
            store.SetInt(Prefix + "mode", (int)Mode);
            store.SetInt(Prefix + "bots", Bots);
            store.SetInt(Prefix + "timelimit", TimeLimitMinutes);
            store.Save();
        }
    }
}
