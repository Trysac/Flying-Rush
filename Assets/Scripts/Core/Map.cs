using System;
using System.Collections.Generic;

namespace FlyingRush.Core
{
    public enum BlockKind
    {
        Ground,
        CageWall,
        Container,
        Cover,
        Base,
        Tower,
        Platform,
        Ledge,
        TunnelRoof,
    }

    /// <summary>An axis-aligned solid block of the map.</summary>
    public readonly struct Block
    {
        public readonly Vec3 Center;
        public readonly Vec3 Size;
        public readonly BlockKind Kind;
        /// <summary>A small number the game uses to vary colours.</summary>
        public readonly int Variant;

        public Block(Vec3 center, Vec3 size, BlockKind kind, int variant = 0)
        {
            Center = center;
            Size = size;
            Kind = kind;
            Variant = variant;
        }

        public Vec3 Min => Center - Size * 0.5f;
        public Vec3 Max => Center + Size * 0.5f;
        public float Top => Center.Y + Size.Y * 0.5f;

        /// <summary>True when the point is inside the block grown sideways by <paramref name="margin"/>.</summary>
        public bool Contains(Vec3 point, float margin = 0f)
        {
            Vec3 min = Min, max = Max;
            return point.X > min.X - margin && point.X < max.X + margin &&
                   point.Y > min.Y && point.Y < max.Y &&
                   point.Z > min.Z - margin && point.Z < max.Z + margin;
        }

        /// <summary>
        /// Whether the segment from a to b passes through the block grown sideways by
        /// <paramref name="margin"/> (a body's radius; heights are never grown).
        /// </summary>
        public bool Intersects(Vec3 a, Vec3 b, float margin = 0f)
        {
            Vec3 min = Min, max = Max;
            float t0 = 0f, t1 = 1f;
            return Slab(a.X, b.X - a.X, min.X - margin, max.X + margin, ref t0, ref t1) &&
                   Slab(a.Y, b.Y - a.Y, min.Y, max.Y, ref t0, ref t1) &&
                   Slab(a.Z, b.Z - a.Z, min.Z - margin, max.Z + margin, ref t0, ref t1);
        }

        static bool Slab(float start, float delta, float min, float max, ref float t0, ref float t1)
        {
            if (MathF.Abs(delta) < 1e-7f)
            {
                return start > min && start < max;
            }
            float near = (min - start) / delta;
            float far = (max - start) / delta;
            if (near > far)
            {
                (near, far) = (far, near);
            }
            t0 = MathF.Max(t0, near);
            t1 = MathF.Min(t1, far);
            return t0 < t1;
        }
    }

    public readonly struct SpawnPoint
    {
        public readonly Vec3 Position;
        /// <summary>0: anyone; 1 or 2: that team's base in team deathmatch.</summary>
        public readonly int Team;
        /// <summary>Which way to face, in degrees around Y (0 is +Z).</summary>
        public readonly float Yaw;

        public SpawnPoint(Vec3 position, int team, float yaw)
        {
            Position = position;
            Team = team;
            Yaw = yaw;
        }
    }

    /// <summary>
    /// The cage: a closed arena of shipping containers, cover walls, two team bases, two sniper
    /// towers, floating platforms, ledges on the cage walls and two low tunnels you crouch through,
    /// after the 2021 map. Every solid is a box, so the bots' waypoint graph and the tests can be
    /// worked out without the engine. The layout is the same after a half turn about the centre,
    /// so neither team's side is better.
    /// </summary>
    public sealed class MapLayout
    {
        public const float HalfSize = 42f;
        public const float WallHeight = 26f;
        /// <summary>Below this height a fighter has fallen out of the cage.</summary>
        public const float KillHeight = -12f;
        /// <summary>The fighters' radius, used to keep waypoints and links clear of walls.</summary>
        public const float BodyRadius = 0.45f;

        readonly List<Block> blocks = new List<Block>();
        readonly List<Block> crouchZones = new List<Block>();
        readonly List<SpawnPoint> spawns = new List<SpawnPoint>();
        readonly List<(WeaponKind kind, Vec3 position)> pads = new List<(WeaponKind, Vec3)>();

        public IReadOnlyList<Block> Blocks => blocks;
        /// <summary>Spaces too low to stand in: everyone crouches there.</summary>
        public IReadOnlyList<Block> CrouchZones => crouchZones;
        public IReadOnlyList<SpawnPoint> Spawns => spawns;
        public IReadOnlyList<(WeaponKind kind, Vec3 position)> Pads => pads;

        public static MapLayout Cage()
        {
            var map = new MapLayout();
            float h = HalfSize;
            map.Add(new Vec3(0f, -0.5f, 0f), new Vec3(h * 2f + 2f, 1f, h * 2f + 2f), BlockKind.Ground);
            map.Add(new Vec3(0f, WallHeight / 2f, h + 0.5f), new Vec3(h * 2f + 2f, WallHeight, 1f), BlockKind.CageWall);
            map.Add(new Vec3(0f, WallHeight / 2f, -h - 0.5f), new Vec3(h * 2f + 2f, WallHeight, 1f), BlockKind.CageWall);
            map.Add(new Vec3(h + 0.5f, WallHeight / 2f, 0f), new Vec3(1f, WallHeight, h * 2f), BlockKind.CageWall, 1);
            map.Add(new Vec3(-h - 0.5f, WallHeight / 2f, 0f), new Vec3(1f, WallHeight, h * 2f), BlockKind.CageWall, 1);

            // Everything else comes in pairs, the second a half turn of the first.
            // Team bases: a raised deck against the north and south walls with a low front wall.
            map.Pair(new Vec3(0f, 1.5f, -35.5f), new Vec3(20f, 3f, 11f), BlockKind.Base);
            map.Pair(new Vec3(-6f, 3.8f, -30.3f), new Vec3(5f, 1.6f, 0.6f), BlockKind.Cover);
            map.Pair(new Vec3(6f, 3.8f, -30.3f), new Vec3(5f, 1.6f, 0.6f), BlockKind.Cover);
            map.Pair(new Vec3(-12.5f, 0.75f, -33f), new Vec3(5f, 1.5f, 4f), BlockKind.Base, 1);
            map.Pair(new Vec3(12.5f, 0.75f, -33f), new Vec3(5f, 1.5f, 4f), BlockKind.Base, 1);

            // Sniper towers on the west and east: a pillar, a deck and a rim to hide behind.
            map.Pair(new Vec3(-35f, 4.5f, 0f), new Vec3(5f, 9f, 5f), BlockKind.Tower);
            map.Pair(new Vec3(-35f, 9.25f, 0f), new Vec3(10f, 0.5f, 10f), BlockKind.Platform, 1);
            map.Pair(new Vec3(-30.25f, 10.25f, 0f), new Vec3(0.5f, 1.5f, 5f), BlockKind.Cover, 1);
            map.Pair(new Vec3(-38f, 4.5f, 8f), new Vec3(4f, 0.4f, 3f), BlockKind.Ledge);

            // Shipping containers, some stacked two high.
            map.Pair(new Vec3(-14f, 1.3f, -14f), new Vec3(12f, 2.6f, 2.6f), BlockKind.Container, 0);
            map.Pair(new Vec3(-15f, 3.9f, -14f), new Vec3(12f, 2.6f, 2.6f), BlockKind.Container, 1);
            map.Pair(new Vec3(15f, 1.3f, -17f), new Vec3(2.6f, 2.6f, 12f), BlockKind.Container, 2);
            map.Pair(new Vec3(-23f, 1.3f, -4f), new Vec3(2.6f, 2.6f, 12f), BlockKind.Container, 3);
            map.Pair(new Vec3(0f, 1.3f, -19f), new Vec3(12f, 2.6f, 2.6f), BlockKind.Container, 4);
            map.Pair(new Vec3(-26f, 1.3f, -27f), new Vec3(2.6f, 2.6f, 6f), BlockKind.Container, 5);
            map.Pair(new Vec3(28f, 1.3f, -34f), new Vec3(6f, 2.6f, 2.6f), BlockKind.Container, 1);

            // Cover walls on the floor.
            map.Pair(new Vec3(-8f, 0.8f, -7f), new Vec3(4f, 1.6f, 0.5f), BlockKind.Cover);
            map.Pair(new Vec3(8f, 0.8f, -7f), new Vec3(0.5f, 1.6f, 4f), BlockKind.Cover);
            map.Pair(new Vec3(-30f, 0.8f, -16f), new Vec3(4f, 1.6f, 0.5f), BlockKind.Cover);
            map.Pair(new Vec3(30f, 0.8f, -24f), new Vec3(0.5f, 1.6f, 4f), BlockKind.Cover);
            map.Pair(new Vec3(-12f, 0.8f, -26f), new Vec3(0.5f, 1.6f, 4f), BlockKind.Cover);

            // Floating platforms over the centre: a low one and the railgun's high one.
            map.Add(new Vec3(0f, 5.75f, 0f), new Vec3(10f, 0.5f, 10f), BlockKind.Platform);
            map.Pair(new Vec3(0f, 12.75f, 9.5f), new Vec3(5f, 0.5f, 5f), BlockKind.Platform, 2);
            map.Pair(new Vec3(-16f, 7.25f, 9f), new Vec3(5f, 0.5f, 5f), BlockKind.Platform);

            // Ledges on the cage walls, to climb with the extra jumps.
            map.Pair(new Vec3(-18f, 6f, -41f), new Vec3(5f, 0.4f, 2f), BlockKind.Ledge);
            map.Pair(new Vec3(18f, 9f, -41f), new Vec3(5f, 0.4f, 2f), BlockKind.Ledge);
            map.Pair(new Vec3(41f, 7f, -22f), new Vec3(2f, 0.4f, 5f), BlockKind.Ledge);

            // The crouching tunnels: a low roof on two side walls.
            map.Tunnel(new Vec3(26f, 0f, -12f));
            map.Tunnel(new Vec3(-26f, 0f, 12f));

            // Spawns: free for all uses every one; team deathmatch only its own base's.
            map.PairSpawn(new Vec3(-5f, 3f, -36f), 1, 0f);
            map.PairSpawn(new Vec3(5f, 3f, -36f), 1, 0f);
            map.PairSpawn(new Vec3(-12.5f, 1.5f, -33f), 1, 0f);
            map.PairSpawn(new Vec3(12.5f, 1.5f, -33f), 1, 0f);
            map.PairSpawn(new Vec3(0f, 3f, -39f), 1, 0f);
            map.PairSpawn(new Vec3(-36f, 0f, -30f), 0, 45f);
            map.PairSpawn(new Vec3(36f, 0f, -12f), 0, -60f);
            map.PairSpawn(new Vec3(-20f, 0f, -22f), 0, 30f);
            map.PairSpawn(new Vec3(-34f, 0f, 14f), 0, 120f);

            // Power weapons, up high as in 2021 (the shotguns wait inside the tunnels).
            map.pads.Add((WeaponKind.Railgun, new Vec3(0f, 13f, 9.5f)));
            map.pads.Add((WeaponKind.Railgun, new Vec3(0f, 13f, -9.5f)));
            map.pads.Add((WeaponKind.Rockets, new Vec3(0f, 6f, 0f)));
            map.pads.Add((WeaponKind.Sniper, new Vec3(-36f, 9.5f, 0f)));
            map.pads.Add((WeaponKind.Sniper, new Vec3(36f, 9.5f, 0f)));
            map.pads.Add((WeaponKind.Shotgun, new Vec3(26f, 0f, -12f)));
            map.pads.Add((WeaponKind.Shotgun, new Vec3(-26f, 0f, 12f)));
            map.pads.Add((WeaponKind.Rockets, new Vec3(-16f, 7.5f, 9f)));
            map.pads.Add((WeaponKind.Rockets, new Vec3(16f, 7.5f, -9f)));
            return map;
        }

        void Add(Vec3 center, Vec3 size, BlockKind kind, int variant = 0) =>
            blocks.Add(new Block(center, size, kind, variant));

        static Vec3 HalfTurn(Vec3 v) => new Vec3(-v.X, v.Y, -v.Z);

        void Pair(Vec3 center, Vec3 size, BlockKind kind, int variant = 0)
        {
            Add(center, size, kind, variant);
            Add(HalfTurn(center), size, kind, variant);
        }

        void PairSpawn(Vec3 position, int team, float yaw)
        {
            spawns.Add(new SpawnPoint(position, team, yaw));
            spawns.Add(new SpawnPoint(HalfTurn(position), team == 0 ? 0 : 3 - team, yaw + 180f));
        }

        void Tunnel(Vec3 center)
        {
            const float length = 12f, inner = 2.6f, height = 1.3f;
            bool alongX = true;
            Vec3 size = alongX ? new Vec3(length, height, 0.6f) : new Vec3(0.6f, height, length);
            Add(center + new Vec3(0f, height / 2f, -(inner / 2f + 0.3f)), size, BlockKind.Container, 6);
            Add(center + new Vec3(0f, height / 2f, inner / 2f + 0.3f), size, BlockKind.Container, 6);
            Add(center + new Vec3(0f, height + 0.3f, 0f), new Vec3(length, 0.6f, inner + 1.2f), BlockKind.TunnelRoof);
            crouchZones.Add(new Block(center + new Vec3(0f, height / 2f, 0f), new Vec3(length + 1f, height, inner), BlockKind.TunnelRoof));
        }

        /// <summary>True when the point is inside a solid block grown by <paramref name="margin"/>.</summary>
        public bool InsideSolid(Vec3 point, float margin = 0f)
        {
            foreach (Block block in blocks)
            {
                if (block.Contains(point, margin))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>True when nothing solid lies on the segment.</summary>
        public bool Clear(Vec3 a, Vec3 b, float margin = 0f)
        {
            foreach (Block block in blocks)
            {
                if (block.Intersects(a, b, margin))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>True when the feet are in a space too low to stand in.</summary>
        public bool MustCrouch(Vec3 feet)
        {
            foreach (Block zone in crouchZones)
            {
                if (zone.Contains(feet + new Vec3(0f, 0.3f, 0f), 0.4f))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>Whether a position has fallen out of the cage.</summary>
        public static bool OutOfBounds(Vec3 position) =>
            position.Y < KillHeight || MathF.Abs(position.X) > HalfSize + 4f || MathF.Abs(position.Z) > HalfSize + 4f;
    }
}
