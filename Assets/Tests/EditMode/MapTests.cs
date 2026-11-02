using System.Collections.Generic;
using System.Linq;
using FlyingRush.Core;
using NUnit.Framework;

namespace FlyingRush.Core.Tests
{
    public class MapTests
    {
        static MapLayout map;
        static WaypointGraph graph;

        [OneTimeSetUp]
        public void BuildOnce()
        {
            map = MapLayout.Cage();
            graph = WaypointGraph.Build(map);
        }

        static Vec3 HalfTurn(Vec3 v) => new Vec3(-v.X, v.Y, -v.Z);

        [Test]
        public void TheCageIsClosedByFourWallsTallerThanAnyClimb()
        {
            Block[] walls = map.Blocks.Where(b => b.Kind == BlockKind.CageWall).ToArray();

            Assert.That(walls.Length, Is.EqualTo(4));
            Assert.That(walls.All(w => w.Top > Jumps.MaxClimb * 2f), Is.True);
        }

        [Test]
        public void TheMapHasTheOriginalLandmarks()
        {
            Assert.That(map.Blocks.Count(b => b.Kind == BlockKind.Base), Is.GreaterThanOrEqualTo(2), "two team bases");
            Assert.That(map.Blocks.Count(b => b.Kind == BlockKind.Tower), Is.EqualTo(2), "two sniper towers");
            Assert.That(map.Blocks.Count(b => b.Kind == BlockKind.Container), Is.GreaterThanOrEqualTo(10));
            Assert.That(map.CrouchZones.Count, Is.EqualTo(2), "two crouching tunnels");
            Assert.That(map.Pads.Select(p => p.kind).Distinct().Count(), Is.EqualTo(4), "every power weapon has a pad");
            Assert.That(map.Pads.All(p => p.kind != WeaponKind.Shotgun ? p.position.Y > 5f : map.MustCrouch(p.position)), Is.True,
                "power weapons lie up high, shotguns inside the tunnels");
        }

        [Test]
        public void EveryBlockHasAHalfTurnTwinSoNeitherTeamHasTheBetterSide()
        {
            foreach (Block block in map.Blocks)
            {
                bool twin = map.Blocks.Any(other => other.Kind == block.Kind &&
                    Vec3.Distance(other.Center, HalfTurn(block.Center)) < 0.01f &&
                    Vec3.Distance(other.Size, block.Size) < 0.01f);
                Assert.That(twin, Is.True, $"{block.Kind} at {block.Center}");
            }
        }

        [Test]
        public void EachTeamHasTheSameNumberOfBaseSpawnsAndFreeForAllUsesThemAll()
        {
            Assert.That(map.Spawns.Count(s => s.Team == 1), Is.EqualTo(map.Spawns.Count(s => s.Team == 2)));
            Assert.That(map.Spawns.Count(s => s.Team == 1), Is.GreaterThanOrEqualTo(5), "room for a full team");
            Assert.That(map.Spawns.Count, Is.GreaterThanOrEqualTo(10), "room for every free for all fighter");
        }

        [Test]
        public void SpawnsAndPadsAreClearOfSolidBlocksWithGroundUnderThem()
        {
            IEnumerable<Vec3> points = map.Spawns.Select(s => s.Position).Concat(map.Pads.Select(p => p.position));
            foreach (Vec3 point in points)
            {
                Assert.That(map.InsideSolid(point + new Vec3(0f, 0.9f, 0f), MapLayout.BodyRadius), Is.False, $"{point} is inside a block");
                Assert.That(map.Clear(point + new Vec3(0f, 0.05f, 0f), point - new Vec3(0f, 0.2f, 0f)), Is.False, $"{point} stands on nothing");
            }
        }

        [Test]
        public void BlockSegmentsHitWhatTheyCrossAndMissWhatTheyDoNot()
        {
            var block = new Block(Vec3.Zero, new Vec3(2f, 2f, 2f), BlockKind.Cover);

            Assert.That(block.Intersects(new Vec3(-5f, 0f, 0f), new Vec3(5f, 0f, 0f)), Is.True);
            Assert.That(block.Intersects(new Vec3(-5f, 2f, 0f), new Vec3(5f, 2f, 0f)), Is.False);
            Assert.That(block.Intersects(new Vec3(-5f, 0f, 1.3f), new Vec3(5f, 0f, 1.3f)), Is.False);
            Assert.That(block.Intersects(new Vec3(-5f, 0f, 1.3f), new Vec3(5f, 0f, 1.3f), 0.5f), Is.True, "a margin widens it sideways");
            Assert.That(block.Intersects(new Vec3(-5f, 0f, 0f), new Vec3(-2f, 0f, 0f)), Is.False, "stops short");
        }

        [Test]
        public void TheTunnelsAreTooLowToStandIn()
        {
            foreach (Block zone in map.CrouchZones)
            {
                Vec3 inside = new Vec3(zone.Center.X, 0f, zone.Center.Z);
                Assert.That(map.MustCrouch(inside), Is.True);
                Assert.That(map.Clear(inside + new Vec3(0f, 0.1f, 0f), inside + new Vec3(0f, Jumps.StandHeight, 0f)), Is.False);
                Assert.That(map.Clear(inside + new Vec3(0f, 0.1f, 0f), inside + new Vec3(0f, Jumps.CrouchHeight, 0f)), Is.True);
            }
            Assert.That(map.MustCrouch(new Vec3(0f, 0f, -20f + 10f)), Is.False);
        }

        [Test]
        public void TheWaypointGraphCoversTheFloorTheBlocksAndThePlatforms()
        {
            Assert.That(graph.Nodes.Count, Is.GreaterThan(150));
            Assert.That(graph.Nodes.Count(n => n.Y > 5f), Is.GreaterThan(10), "waypoints up high");
            Assert.That(graph.LinkCount, Is.GreaterThan(graph.Nodes.Count));
            foreach (Vec3 node in graph.Nodes)
            {
                Assert.That(map.InsideSolid(node + new Vec3(0f, 0.5f, 0f)), Is.False, $"{node}");
            }
        }

        [Test]
        public void EveryWaypointCanBeReachedFromEverySpawn()
        {
            HashSet<int> reachable = graph.Reachable(graph.Nearest(map.Spawns[0].Position));

            Assert.That(reachable.Count, Is.EqualTo(graph.Nodes.Count),
                "unreachable: " + string.Join(", ", Enumerable.Range(0, graph.Nodes.Count).Where(i => !reachable.Contains(i)).Select(i => graph.Nodes[i])));
        }

        [Test]
        public void EveryWeaponPadHasAPathFromEverySpawn()
        {
            foreach (SpawnPoint spawn in map.Spawns)
            {
                foreach ((WeaponKind kind, Vec3 position) in map.Pads)
                {
                    List<int> path = graph.Path(graph.Nearest(spawn.Position), graph.Nearest(position));
                    Assert.That(path, Is.Not.Empty, $"{spawn.Position} to the {kind} at {position}");
                    Assert.That(Vec3.Distance(graph.Nodes[path.Last()], position), Is.LessThan(1.5f));
                }
            }
        }

        [Test]
        public void PathsOnlyUseLinksAndNoClimbIsHigherThanTheJumpsAllow()
        {
            List<int> path = graph.Path(graph.Nearest(map.Spawns[0].Position), graph.Nearest(new Vec3(0f, 13f, 9.5f)));

            Assert.That(path.Count, Is.GreaterThan(2));
            for (int i = 1; i < path.Count; i++)
            {
                Assert.That(graph.LinksOf(path[i - 1]), Does.Contain(path[i]));
                Assert.That(graph.Nodes[path[i]].Y - graph.Nodes[path[i - 1]].Y, Is.LessThanOrEqualTo(WaypointGraph.MaxClimb));
            }
        }

        [Test]
        public void APathToItselfIsJustThatWaypoint()
        {
            Assert.That(graph.Path(5, 5), Is.EqualTo(new[] { 5 }));
            Assert.That(graph.Path(-1, 5), Is.Empty);
        }

        [Test]
        public void NobodyClimbsStraightThroughACoverWall()
        {
            Block cover = map.Blocks.First(b => b.Kind == BlockKind.Cover && b.Top < 2f);
            Vec3 before = cover.Center - new Vec3(cover.Size.X > 1f ? 0f : 3f, cover.Center.Y, cover.Size.X > 1f ? 3f : 0f);
            Vec3 after = cover.Center + new Vec3(cover.Size.X > 1f ? 0f : 3f, -cover.Center.Y, cover.Size.X > 1f ? 3f : 0f);

            Assert.That(WaypointGraph.CanTravel(map, before, after), Is.False);
        }

        [Test]
        public void FallingBelowTheFloorOrLeavingTheCageIsOutOfBounds()
        {
            Assert.That(MapLayout.OutOfBounds(new Vec3(0f, -13f, 0f)), Is.True);
            Assert.That(MapLayout.OutOfBounds(new Vec3(50f, 3f, 0f)), Is.True);
            Assert.That(MapLayout.OutOfBounds(new Vec3(10f, 12f, -30f)), Is.False);
        }
    }

    public class SpawnPickerTests
    {
        static readonly SpawnPoint[] Spawns =
        {
            new SpawnPoint(new Vec3(0f, 0f, 0f), 0, 0f),
            new SpawnPoint(new Vec3(40f, 0f, 0f), 0, 0f),
            new SpawnPoint(new Vec3(80f, 0f, 0f), 1, 0f),
            new SpawnPoint(new Vec3(-80f, 0f, 0f), 2, 0f),
        };

        [Test]
        public void ItPicksTheSpawnFarthestFromTheEnemies()
        {
            var picker = new SpawnPicker();

            int pick = picker.Pick(Spawns, 0, new[] { new Vec3(-75f, 0f, 0f), new Vec3(2f, 0f, 0f), new Vec3(38f, 0f, 0f) });

            Assert.That(pick, Is.EqualTo(2));
        }

        [Test]
        public void ATeamOnlySpawnsAtItsOwnBase()
        {
            var picker = new SpawnPicker();

            Assert.That(picker.Pick(Spawns, 1, new Vec3[0]), Is.EqualTo(2));
            Assert.That(picker.Pick(Spawns, 2, new Vec3[0]), Is.EqualTo(3));
        }

        [Test]
        public void SafeSpawnsAreUsedInTurnLikeTheOriginalRoundRobin()
        {
            var picker = new SpawnPicker();
            var picks = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                picks.Add(picker.Pick(Spawns, 0, new Vec3[0]));
            }

            Assert.That(picks, Is.EqualTo(new[] { 0, 1, 2, 3, 0, 1, 2, 3 }));
        }
    }
}
