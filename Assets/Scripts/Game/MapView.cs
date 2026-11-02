using System.Collections.Generic;
using FlyingRush.Core;
using UnityEngine;

namespace FlyingRush.Game
{
    /// <summary>
    /// Builds the cage from the core's layout: a mesh and a box collider for every block, neon
    /// trims along the edges, glowing grid panels on the walls and the weapon pads.
    /// </summary>
    public sealed class MapView : MonoBehaviour
    {
        readonly List<PadView> pads = new List<PadView>();

        public IReadOnlyList<PadView> Pads => pads;

        public void Build(MapLayout layout, GameArt art)
        {
            foreach (Block block in layout.Blocks)
            {
                AddBlock(block, art);
            }
            AddWallDetail(art);
            AddFloorLines(art);
            foreach ((WeaponKind kind, Vec3 position) in layout.Pads)
            {
                var pad = new GameObject($"{kind} Pad").AddComponent<PadView>();
                pad.transform.SetParent(transform, false);
                pad.Build(new WeaponPad(kind, position), art);
                pads.Add(pad);
            }
        }

        void AddBlock(Block block, GameArt art)
        {
            Vector3 size = block.Size.ToUnity();
            Material material = MaterialFor(block, art, out float tile);
            GameObject box = Shapes.Solid(block.Kind.ToString(), transform, block.Center.ToUnity(), size, material, tile);
            box.layer = Layers.World;
            box.isStatic = true;
            box.AddComponent<BoxCollider>().size = size;

            // Neon trims on the top edges of what you stand on, so platforms read at a distance.
            bool trimmed = block.Kind == BlockKind.Platform || block.Kind == BlockKind.Base || block.Kind == BlockKind.Ledge;
            if (trimmed)
            {
                Material trim = block.Kind == BlockKind.Base ? art.Teal : art.Neon;
                float top = size.y / 2f;
                Vector3 h = size / 2f;
                Shapes.Solid("Trim", box.transform, new Vector3(0f, top, h.z), new Vector3(size.x + 0.06f, 0.08f, 0.08f), trim);
                Shapes.Solid("Trim", box.transform, new Vector3(0f, top, -h.z), new Vector3(size.x + 0.06f, 0.08f, 0.08f), trim);
                Shapes.Solid("Trim", box.transform, new Vector3(h.x, top, 0f), new Vector3(0.08f, 0.08f, size.z + 0.06f), trim);
                Shapes.Solid("Trim", box.transform, new Vector3(-h.x, top, 0f), new Vector3(0.08f, 0.08f, size.z + 0.06f), trim);
            }
            if (block.Kind == BlockKind.Container)
            {
                // Dark end frames, like doors on a shipping container.
                bool alongX = size.x >= size.z;
                float half = (alongX ? size.x : size.z) / 2f - 0.08f;
                Vector3 frame = alongX ? new Vector3(0.18f, size.y + 0.02f, size.z + 0.02f) : new Vector3(size.x + 0.02f, size.y + 0.02f, 0.18f);
                Vector3 offset = alongX ? new Vector3(half, 0f, 0f) : new Vector3(0f, 0f, half);
                Shapes.Solid("Frame", box.transform, offset, frame, art.Metal, 1f);
                Shapes.Solid("Frame", box.transform, -offset, frame, art.Metal, 1f);
            }
        }

        static Material MaterialFor(Block block, GameArt art, out float tile)
        {
            tile = 4f;
            switch (block.Kind)
            {
                case BlockKind.Ground:
                    tile = 8f;
                    return art.Floor;
                case BlockKind.CageWall:
                    tile = 6f;
                    return art.Wall;
                case BlockKind.Container:
                    tile = 2.6f;
                    return art.Containers[Mathf.Abs(block.Variant) % art.Containers.Length];
                case BlockKind.Tower:
                case BlockKind.Base:
                case BlockKind.Cover:
                    tile = 3f;
                    return art.Concrete;
                default:
                    tile = 2f;
                    return art.Metal;
            }
        }

        /// <summary>Glowing grid panels and a neon line along the top of each cage wall.</summary>
        void AddWallDetail(GameArt art)
        {
            float h = MapLayout.HalfSize;
            float top = MapLayout.WallHeight;
            for (int side = 0; side < 4; side++)
            {
                Quaternion turn = Quaternion.Euler(0f, side * 90f, 0f);
                var group = new GameObject("Wall Detail").transform;
                group.SetParent(transform, false);
                group.localRotation = turn;
                Shapes.Solid("Top Trim", group, new Vector3(0f, top - 0.3f, h - 0.05f), new Vector3(h * 2f, 0.25f, 0.12f), art.Neon);
                Shapes.Solid("Low Trim", group, new Vector3(0f, 0.15f, h - 0.05f), new Vector3(h * 2f, 0.1f, 0.1f), art.Neon);
                for (int i = -2; i <= 2; i++)
                {
                    float y = 13f + (i % 2 == 0 ? 0f : 4f);
                    Shapes.Solid("Grid", group, new Vector3(i * 15f + 3f, y, h - 0.04f), new Vector3(6f, 3f, 0.08f), art.Grid, 1.5f);
                }
            }
        }

        /// <summary>Lane markings on the floor.</summary>
        void AddFloorLines(GameArt art)
        {
            float h = MapLayout.HalfSize - 6f;
            Shapes.Solid("Centre Ring", transform, new Vector3(0f, 0.01f, 0f), new Vector3(14f, 0.02f, 0.2f), art.Teal);
            Shapes.Solid("Centre Ring", transform, new Vector3(0f, 0.01f, 0f), new Vector3(0.2f, 0.02f, 14f), art.Teal);
            Shapes.Solid("Lane", transform, new Vector3(-h, 0.01f, 0f), new Vector3(0.14f, 0.02f, h * 2f), art.Teal);
            Shapes.Solid("Lane", transform, new Vector3(h, 0.01f, 0f), new Vector3(0.14f, 0.02f, h * 2f), art.Teal);
        }

        public void Tick(float deltaTime)
        {
            foreach (PadView pad in pads)
            {
                pad.Tick(deltaTime);
            }
        }

        public void ResetPads()
        {
            foreach (PadView pad in pads)
            {
                pad.Pad.Reset();
            }
        }
    }

    /// <summary>A weapon pad: a glowing plate with the weapon spinning above it until it is taken.</summary>
    public sealed class PadView : MonoBehaviour
    {
        public const float PickupRadius = 1.4f;

        Transform model;
        Renderer ring;
        float spin;

        public WeaponPad Pad { get; private set; }

        public void Build(WeaponPad pad, GameArt art)
        {
            Pad = pad;
            transform.position = pad.Position.ToUnity();
            Shapes.Primitive(PrimitiveType.Cylinder, "Plate", transform, new Vector3(0f, 0.03f, 0f), new Vector3(1.6f, 0.03f, 1.6f), art.Metal);
            ring = Shapes.Primitive(PrimitiveType.Cylinder, "Glow", transform, new Vector3(0f, 0.05f, 0f), new Vector3(1.2f, 0.02f, 1.2f), art.Pad)
                .GetComponent<Renderer>();
            var holder = new GameObject("Model").transform;
            holder.SetParent(transform, false);
            holder.localPosition = new Vector3(0f, 0.9f, 0f);
            var accent = new Material(art.Neon) { color = Palette.Of(pad.Kind) };
            accent.SetColor("_EmissionColor", Palette.Of(pad.Kind) * 1.6f);
            Transform muzzle = Shapes.Weapon(pad.Kind, holder, art.Gun, accent);
            muzzle.parent.localPosition = new Vector3(0f, 0f, -0.3f);
            holder.localScale = Vector3.one * 1.5f;
            model = holder;
        }

        public void Tick(float deltaTime)
        {
            Pad.Tick(deltaTime);
            spin += deltaTime;
            bool available = Pad.Available;
            model.gameObject.SetActive(available);
            model.localRotation = Quaternion.Euler(0f, spin * 90f, 0f);
            model.localPosition = new Vector3(0f, 0.9f + Mathf.Sin(spin * 2.2f) * 0.08f, 0f);
            ring.enabled = available || Mathf.Repeat(spin, 1f) < 0.5f && Pad.SecondsUntilBack < 3f;
        }
    }

    public static class VecExtensions
    {
        public static Vector3 ToUnity(this Vec3 v) => new Vector3(v.X, v.Y, v.Z);
        public static Vec3 ToCore(this Vector3 v) => new Vec3(v.x, v.y, v.z);
    }
}
