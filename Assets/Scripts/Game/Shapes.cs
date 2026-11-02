using System.Collections.Generic;
using FlyingRush.Core;
using UnityEngine;

namespace FlyingRush.Game
{
    /// <summary>
    /// Meshes and simple models built in code: boxes with world-scaled UVs for the map, and the
    /// weapons made of a few boxes each.
    /// </summary>
    public static class Shapes
    {
        static readonly Dictionary<Vector3, Mesh> Boxes = new Dictionary<Vector3, Mesh>();
        static GameArt meshes;

        /// <summary>Uses the primitive meshes the project builder stored in the art asset.</summary>
        public static void Use(GameArt art) => meshes = art;

        static Mesh MeshOf(PrimitiveType type)
        {
            if (meshes == null)
            {
                return null;
            }
            return type switch
            {
                PrimitiveType.Sphere => meshes.Sphere,
                PrimitiveType.Capsule => meshes.Capsule,
                PrimitiveType.Cylinder => meshes.Cylinder,
                PrimitiveType.Quad => meshes.Quad,
                _ => meshes.Cube,
            };
        }

        /// <summary>
        /// A box of the given size centred on its origin whose UVs run one unit per
        /// <paramref name="tile"/> metres on every face, so textures keep their scale on any block.
        /// </summary>
        public static Mesh Box(Vector3 size, float tile = 4f)
        {
            Vector3 key = size * 1000f + new Vector3(tile, 0f, 0f);
            if (Boxes.TryGetValue(key, out Mesh cached))
            {
                return cached;
            }
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            Vector3 h = size * 0.5f;
            void Face(Vector3 normal, Vector3 right, Vector3 up, float width, float height)
            {
                int start = vertices.Count;
                Vector3 center = Vector3.Scale(normal, h);
                Vector3 r = right * (width / 2f), u = up * (height / 2f);
                vertices.Add(center - r - u);
                vertices.Add(center + r - u);
                vertices.Add(center + r + u);
                vertices.Add(center - r + u);
                for (int i = 0; i < 4; i++)
                {
                    normals.Add(normal);
                }
                uvs.Add(new Vector2(0f, 0f));
                uvs.Add(new Vector2(width / tile, 0f));
                uvs.Add(new Vector2(width / tile, height / tile));
                uvs.Add(new Vector2(0f, height / tile));
                triangles.AddRange(new[] { start, start + 2, start + 1, start, start + 3, start + 2 });
            }
            Face(Vector3.forward, Vector3.left, Vector3.up, size.x, size.y);
            Face(Vector3.back, Vector3.right, Vector3.up, size.x, size.y);
            Face(Vector3.right, Vector3.forward, Vector3.up, size.z, size.y);
            Face(Vector3.left, Vector3.back, Vector3.up, size.z, size.y);
            Face(Vector3.up, Vector3.right, Vector3.forward, size.x, size.z);
            Face(Vector3.down, Vector3.right, Vector3.back, size.x, size.z);
            var mesh = new Mesh { name = $"Box {size.x}x{size.y}x{size.z}" };
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            Boxes[key] = mesh;
            return mesh;
        }

        /// <summary>A visual-only box (no collider).</summary>
        public static GameObject Solid(string name, Transform parent, Vector3 localPosition, Vector3 size, Material material, float tile = 4f)
        {
            var box = new GameObject(name);
            box.transform.SetParent(parent, false);
            box.transform.localPosition = localPosition;
            box.AddComponent<MeshFilter>().sharedMesh = Box(size, tile);
            var renderer = box.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            return box;
        }

        /// <summary>
        /// A Unity primitive's mesh without a collider. Creating real primitives and removing their
        /// colliders would need the collider classes, which a stripped WebGL player leaves out.
        /// </summary>
        public static GameObject Primitive(PrimitiveType type, string name, Transform parent, Vector3 localPosition, Vector3 scale, Material material)
        {
            GameObject shape;
            Mesh mesh = MeshOf(type);
            if (mesh != null)
            {
                shape = new GameObject(name);
                shape.AddComponent<MeshFilter>().sharedMesh = mesh;
                shape.AddComponent<MeshRenderer>();
            }
            else
            {
                shape = GameObject.CreatePrimitive(type);
                Object.Destroy(shape.GetComponent<Collider>());
            }
            shape.name = name;
            shape.transform.SetParent(parent, false);
            shape.transform.localPosition = localPosition;
            shape.transform.localScale = scale;
            shape.GetComponent<Renderer>().sharedMaterial = material;
            return shape;
        }

        /// <summary>
        /// A weapon model pointing along +Z with its grip at the origin; the returned transform marks
        /// the muzzle.
        /// </summary>
        public static Transform Weapon(WeaponKind kind, Transform parent, Material body, Material accent)
        {
            var root = new GameObject(kind.ToString()).transform;
            root.SetParent(parent, false);
            void Part(Vector3 position, Vector3 size, Material material) => Solid("Part", root, position, size, material, 1f);
            float length;
            switch (kind)
            {
                case WeaponKind.Shotgun:
                    Part(new Vector3(0f, 0.04f, 0.2f), new Vector3(0.09f, 0.1f, 0.62f), body);
                    Part(new Vector3(0f, -0.02f, 0.28f), new Vector3(0.08f, 0.06f, 0.3f), accent);
                    Part(new Vector3(0f, -0.06f, -0.08f), new Vector3(0.06f, 0.14f, 0.12f), body);
                    length = 0.52f;
                    break;
                case WeaponKind.Sniper:
                    Part(new Vector3(0f, 0.03f, 0.3f), new Vector3(0.06f, 0.07f, 0.95f), body);
                    Part(new Vector3(0f, 0.11f, 0.12f), new Vector3(0.05f, 0.05f, 0.3f), accent);
                    Part(new Vector3(0f, -0.05f, -0.08f), new Vector3(0.06f, 0.13f, 0.18f), body);
                    length = 0.78f;
                    break;
                case WeaponKind.Rockets:
                    Part(new Vector3(0f, 0.06f, 0.18f), new Vector3(0.16f, 0.16f, 0.8f), body);
                    Part(new Vector3(0f, 0.06f, 0.6f), new Vector3(0.18f, 0.18f, 0.06f), accent);
                    Part(new Vector3(0f, -0.06f, 0f), new Vector3(0.06f, 0.14f, 0.1f), body);
                    length = 0.62f;
                    break;
                case WeaponKind.Railgun:
                    Part(new Vector3(0f, 0.04f, 0.25f), new Vector3(0.1f, 0.08f, 0.8f), body);
                    Part(new Vector3(0.055f, 0.04f, 0.3f), new Vector3(0.02f, 0.05f, 0.6f), accent);
                    Part(new Vector3(-0.055f, 0.04f, 0.3f), new Vector3(0.02f, 0.05f, 0.6f), accent);
                    Part(new Vector3(0f, -0.05f, -0.06f), new Vector3(0.06f, 0.13f, 0.14f), body);
                    length = 0.66f;
                    break;
                default:
                    Part(new Vector3(0f, 0.03f, 0.18f), new Vector3(0.07f, 0.09f, 0.55f), body);
                    Part(new Vector3(0f, -0.06f, 0.14f), new Vector3(0.05f, 0.12f, 0.07f), accent);
                    Part(new Vector3(0f, -0.05f, -0.06f), new Vector3(0.06f, 0.12f, 0.1f), body);
                    length = 0.46f;
                    break;
            }
            var muzzle = new GameObject("Muzzle").transform;
            muzzle.SetParent(root, false);
            muzzle.localPosition = new Vector3(0f, 0.04f, length);
            return muzzle;
        }
    }
}
