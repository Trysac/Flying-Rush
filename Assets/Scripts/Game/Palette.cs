using FlyingRush.Core;
using UnityEngine;

namespace FlyingRush.Game
{
    /// <summary>
    /// The cage's colours, after the 2021 map: rusty red containers and walls under a dusky violet
    /// sky, with neon violet trims, and the HUD's cyan and amber.
    /// </summary>
    public static class Palette
    {
        public static readonly Color Sky = Hex("#2a1838");
        public static readonly Color Fog = Hex("#3b2147");
        public static readonly Color Floor = Hex("#4a4250");
        public static readonly Color Wall = Hex("#6b2a2f");
        public static readonly Color Concrete = Hex("#7a6e78");
        public static readonly Color Metal = Hex("#5a5866");
        public static readonly Color Neon = Hex("#a45cff");
        public static readonly Color Teal = Hex("#3df2c8");
        public static readonly Color Amber = Hex("#ffb238");
        public static readonly Color Text = Hex("#f6f0ff");
        public static readonly Color Muted = Hex("#b8a8c8");
        public static readonly Color Danger = Hex("#ff3b4f");
        public static readonly Color Good = Hex("#5dffa8");
        public static readonly Color Gold = Hex("#ffd84a");
        public static readonly Color Cyan = Hex("#38d9ff");
        public static readonly Color Crimson = Hex("#ff4a5e");

        static readonly Color[] Containers =
        {
            Hex("#a3392f"), Hex("#2f6f8f"), Hex("#c47a2c"), Hex("#3c7a4a"), Hex("#8a2f4f"), Hex("#6a6a72"), Hex("#7b3a2a"),
        };

        /// <summary>Colours for free for all fighters; the player is always the first.</summary>
        static readonly Color[] Fighters =
        {
            Hex("#38d9ff"), Hex("#ff4a5e"), Hex("#ffb238"), Hex("#5dffa8"), Hex("#c77dff"),
            Hex("#ff7ad9"), Hex("#f2f25a"), Hex("#ff8a3d"), Hex("#9fb4ff"), Hex("#e8e8e8"),
        };

        public static Color Container(int variant) => Containers[Mathf.Abs(variant) % Containers.Length];

        public static Color TeamColor(int team) => team == 2 ? Crimson : Cyan;

        public static Color FighterColor(Fighter fighter) =>
            fighter.Team != 0 ? TeamColor(fighter.Team) : Fighters[fighter.Id % Fighters.Length];

        public static Color Of(WeaponKind kind) => kind switch
        {
            WeaponKind.Shotgun => Amber,
            WeaponKind.Sniper => Good,
            WeaponKind.Rockets => Hex("#ff7a3d"),
            WeaponKind.Railgun => Neon,
            _ => Text,
        };

        public static Color WithAlpha(this Color color, float alpha) => new Color(color.r, color.g, color.b, alpha);

        public static Color Hex(string hex) => ColorUtility.TryParseHtmlString(hex, out Color color) ? color : Color.magenta;

        public static string Css(Color color) => "#" + ColorUtility.ToHtmlStringRGB(color);
    }

    /// <summary>The physics layers the game uses (named by the project builder).</summary>
    public static class Layers
    {
        public const int World = 8;
        public const int Fighter = 9;
        public const int Effect = 10;

        public static int WorldMask => 1 << World;
        public static int ShotMask => (1 << World) | (1 << Fighter);
    }
}
