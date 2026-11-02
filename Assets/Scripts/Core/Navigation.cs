using System;
using System.Collections.Generic;

namespace FlyingRush.Core
{
    /// <summary>
    /// The bots' map: waypoints on the floor and on top of every block you can stand on, linked
    /// when a fighter can get from one to the other by walking, dropping or multi-jumping. A climb
    /// goes straight up over the lower waypoint and then across, a drop across and then down, so a
    /// link is only made when that path is clear of solid blocks.
    /// </summary>
    public sealed class WaypointGraph
    {
        public const float GroundSpacing = 6f;
        public const float TopSpacing = 4.5f;
        public const float MaxLevelLink = 8.5f;
        public const float MaxClimbLink = 8f;
        /// <summary>The highest climb the bots attempt, a margin under <see cref="Jumps.MaxClimb"/>.</summary>
        public static readonly float MaxClimb = Jumps.MaxClimb * 0.9f;
        const float Headroom = 2f;
        const float KneeHeight = 0.5f;

        readonly List<Vec3> nodes = new List<Vec3>();
        readonly List<List<int>> links = new List<List<int>>();

        public IReadOnlyList<Vec3> Nodes => nodes;
        public IReadOnlyList<int> LinksOf(int node) => links[node];

        public int LinkCount
        {
            get
            {
                int count = 0;
                foreach (List<int> list in links)
                {
                    count += list.Count;
                }
                return count / 2;
            }
        }

        public static WaypointGraph Build(MapLayout map)
        {
            var graph = new WaypointGraph();
            float reach = MapLayout.HalfSize - 2f;
            for (float x = -reach; x <= reach + 0.01f; x += GroundSpacing)
            {
                for (float z = -reach; z <= reach + 0.01f; z += GroundSpacing)
                {
                    graph.TryAdd(map, new Vec3(x, 0f, z));
                }
            }
            foreach (Block block in map.Blocks)
            {
                if (block.Kind == BlockKind.Ground || block.Kind == BlockKind.CageWall || block.Kind == BlockKind.TunnelRoof)
                {
                    continue;
                }
                foreach (Vec3 top in TopPoints(block))
                {
                    graph.TryAdd(map, top);
                }
            }
            // The tunnels are below the ground grid's spacing: add their ends and middle.
            foreach (Block zone in map.CrouchZones)
            {
                float half = zone.Size.X / 2f + 1.2f;
                graph.TryAdd(map, new Vec3(zone.Center.X - half, 0f, zone.Center.Z));
                graph.TryAdd(map, new Vec3(zone.Center.X, 0f, zone.Center.Z));
                graph.TryAdd(map, new Vec3(zone.Center.X + half, 0f, zone.Center.Z));
            }
            // Every spawn and weapon pad gets a waypoint of its own.
            foreach (SpawnPoint spawn in map.Spawns)
            {
                graph.TryAdd(map, spawn.Position);
            }
            foreach ((WeaponKind _, Vec3 position) in map.Pads)
            {
                graph.TryAdd(map, position);
            }
            graph.Link(map);
            return graph;
        }

        static IEnumerable<Vec3> TopPoints(Block block)
        {
            float inset = 1f;
            int countX = Math.Max(1, (int)MathF.Ceiling((block.Size.X - inset * 2f) / TopSpacing));
            int countZ = Math.Max(1, (int)MathF.Ceiling((block.Size.Z - inset * 2f) / TopSpacing));
            for (int i = 0; i < countX; i++)
            {
                for (int j = 0; j < countZ; j++)
                {
                    float x = countX == 1 ? block.Center.X : block.Min.X + inset + (block.Size.X - inset * 2f) * i / (countX - 1);
                    float z = countZ == 1 ? block.Center.Z : block.Min.Z + inset + (block.Size.Z - inset * 2f) * j / (countZ - 1);
                    yield return new Vec3(x, block.Top, z);
                }
            }
        }

        void TryAdd(MapLayout map, Vec3 point)
        {
            foreach (Vec3 existing in nodes)
            {
                if (Vec3.Distance(existing, point) < 1f)
                {
                    return;
                }
            }
            if (!Standable(map, point))
            {
                return;
            }
            nodes.Add(point);
            links.Add(new List<int>());
        }

        /// <summary>Room for a body (crouched in a tunnel) and solid ground under the feet.</summary>
        static bool Standable(MapLayout map, Vec3 feet)
        {
            float room = map.MustCrouch(feet) ? Jumps.CrouchHeight : Headroom;
            Vec3 low = feet + new Vec3(0f, 0.05f, 0f);
            Vec3 high = feet + new Vec3(0f, room, 0f);
            foreach (Block block in map.Blocks)
            {
                if (block.Intersects(low, high, MapLayout.BodyRadius))
                {
                    return false;
                }
            }
            return true;
        }

        void Link(MapLayout map)
        {
            for (int a = 0; a < nodes.Count; a++)
            {
                for (int b = a + 1; b < nodes.Count; b++)
                {
                    if (CanTravel(map, nodes[a], nodes[b]))
                    {
                        links[a].Add(b);
                        links[b].Add(a);
                    }
                }
            }
        }

        /// <summary>Whether a fighter can travel between two points in either direction.</summary>
        public static bool CanTravel(MapLayout map, Vec3 a, Vec3 b)
        {
            Vec3 low = a.Y <= b.Y ? a : b;
            Vec3 high = a.Y <= b.Y ? b : a;
            float rise = high.Y - low.Y;
            float across = Vec3.FlatDistance(a, b);
            float margin = MapLayout.BodyRadius * 0.8f;
            if (rise < 0.4f)
            {
                if (across > MaxLevelLink)
                {
                    return false;
                }
                float knee = Math.Max(a.Y, b.Y) + KneeHeight;
                return map.Clear(new Vec3(a.X, knee, a.Z), new Vec3(b.X, knee, b.Z), margin) && NoGapBetween(map, a, b);
            }
            if (rise > MaxClimb || across > MaxClimbLink)
            {
                return false;
            }
            float over = high.Y + 1f;
            Vec3 corner = new Vec3(low.X, over, low.Z);
            return map.Clear(low + new Vec3(0f, KneeHeight, 0f), corner, margin) &&
                   map.Clear(corner, new Vec3(high.X, over, high.Z), margin);
        }

        /// <summary>
        /// A level link off the floor must have something to stand on all the way across, so the
        /// bots do not walk off a platform's edge expecting more platform.
        /// </summary>
        static bool NoGapBetween(MapLayout map, Vec3 a, Vec3 b)
        {
            if (a.Y < 0.2f && b.Y < 0.2f)
            {
                return true;
            }
            const int steps = 6;
            for (int i = 1; i < steps; i++)
            {
                Vec3 point = Vec3.Lerp(a, b, i / (float)steps);
                if (!map.Clear(point + new Vec3(0f, 0.1f, 0f), point - new Vec3(0f, 0.3f, 0f)))
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        /// <summary>The waypoint closest to a point (by straight distance, height counted triple).</summary>
        public int Nearest(Vec3 point)
        {
            int best = -1;
            float bestCost = float.MaxValue;
            for (int i = 0; i < nodes.Count; i++)
            {
                Vec3 d = nodes[i] - point;
                float cost = d.X * d.X + d.Z * d.Z + d.Y * d.Y * 9f;
                if (cost < bestCost)
                {
                    bestCost = cost;
                    best = i;
                }
            }
            return best;
        }

        /// <summary>
        /// The cheapest chain of waypoints from one to another (A*), climbs costing extra; empty when
        /// there is none.
        /// </summary>
        public List<int> Path(int from, int to)
        {
            var path = new List<int>();
            if (from < 0 || to < 0)
            {
                return path;
            }
            int count = nodes.Count;
            var cost = new float[count];
            var came = new int[count];
            var closed = new bool[count];
            for (int i = 0; i < count; i++)
            {
                cost[i] = float.MaxValue;
                came[i] = -1;
            }
            cost[from] = 0f;
            var open = new List<int> { from };
            while (open.Count > 0)
            {
                int bestIndex = 0;
                float bestScore = float.MaxValue;
                for (int i = 0; i < open.Count; i++)
                {
                    float score = cost[open[i]] + Vec3.Distance(nodes[open[i]], nodes[to]);
                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestIndex = i;
                    }
                }
                int current = open[bestIndex];
                open.RemoveAt(bestIndex);
                if (current == to)
                {
                    break;
                }
                closed[current] = true;
                foreach (int next in links[current])
                {
                    if (closed[next])
                    {
                        continue;
                    }
                    float step = StepCost(nodes[current], nodes[next]);
                    float total = cost[current] + step;
                    if (total < cost[next])
                    {
                        if (cost[next] == float.MaxValue)
                        {
                            open.Add(next);
                        }
                        cost[next] = total;
                        came[next] = current;
                    }
                }
            }
            if (from != to && came[to] < 0)
            {
                return path;
            }
            for (int node = to; node != -1; node = came[node])
            {
                path.Add(node);
                if (node == from)
                {
                    break;
                }
            }
            path.Reverse();
            return path;
        }

        static float StepCost(Vec3 a, Vec3 b)
        {
            float rise = b.Y - a.Y;
            return Vec3.Distance(a, b) + (rise > 0.4f ? rise * 1.5f : 0f);
        }

        /// <summary>The waypoints reachable from one, for the tests and the bots' roaming.</summary>
        public HashSet<int> Reachable(int from)
        {
            var seen = new HashSet<int> { from };
            var queue = new Queue<int>();
            queue.Enqueue(from);
            while (queue.Count > 0)
            {
                int node = queue.Dequeue();
                foreach (int next in links[node])
                {
                    if (seen.Add(next))
                    {
                        queue.Enqueue(next);
                    }
                }
            }
            return seen;
        }
    }

    /// <summary>
    /// Picks where a fighter comes back: the allowed spawn farthest from its nearest living enemy,
    /// turning through the spawns in order on a tie (the 2021 game used round robin).
    /// </summary>
    public sealed class SpawnPicker
    {
        int next;

        public int Pick(IReadOnlyList<SpawnPoint> spawns, int team, IReadOnlyList<Vec3> enemies)
        {
            int best = -1;
            float bestDistance = -1f;
            for (int k = 0; k < spawns.Count; k++)
            {
                int i = (next + k) % spawns.Count;
                SpawnPoint spawn = spawns[i];
                if (team != 0 && spawn.Team != team)
                {
                    continue;
                }
                float nearest = float.MaxValue;
                foreach (Vec3 enemy in enemies)
                {
                    nearest = Math.Min(nearest, Vec3.Distance(enemy, spawn.Position));
                }
                // Anything past 25 m counts as safe, so the order decides among the safe ones.
                float score = Math.Min(nearest, 25f);
                if (score > bestDistance + 0.01f)
                {
                    bestDistance = score;
                    best = i;
                }
            }
            if (best >= 0)
            {
                next = (best + 1) % spawns.Count;
            }
            return best;
        }
    }
}
