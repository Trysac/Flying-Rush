using System.Collections.Generic;
using UnityEngine;

namespace FlyingRush.Game
{
    /// <summary>
    /// Pooled visual effects: bullet tracers, the railgun beam, impact sparks, explosions, muzzle
    /// flashes and the burst of pieces a fighter leaves when it goes down.
    /// </summary>
    public sealed class Effects : MonoBehaviour
    {
        sealed class Line
        {
            public LineRenderer Renderer;
            public float Age;
            public float Life;
            public float Width;
            public Color Color;
        }

        sealed class Flash
        {
            public Transform Transform;
            public Renderer Renderer;
            public float Age;
            public float Life;
            public float Size;
            public Color Color;
        }

        sealed class Piece
        {
            public Transform Transform;
            public Vector3 Velocity;
            public Vector3 Spin;
            public float Age;
        }

        const float PieceLife = 1.4f;

        GameArt art;
        ParticleSystem sparks;
        readonly List<Line> lines = new List<Line>();
        readonly List<Flash> flashes = new List<Flash>();
        readonly List<Piece> pieces = new List<Piece>();
        MaterialPropertyBlock block;

        /// <summary>Total live effects, for the tests.</summary>
        public int Active => CountLines() + CountFlashes() + pieces.Count;

        public void Init(GameArt gameArt)
        {
            art = gameArt;
            block = new MaterialPropertyBlock();
            sparks = CreateSparks();
        }

        ParticleSystem CreateSparks()
        {
            var host = new GameObject("Sparks");
            host.transform.SetParent(transform, false);
            host.layer = Layers.Effect;
            var system = host.AddComponent<ParticleSystem>();
            system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ParticleSystem.MainModule main = system.main;
            main.playOnAwake = false;
            main.loop = false;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.15f, 0.45f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(2f, 9f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.05f, 0.14f);
            main.gravityModifier = 1.2f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.maxParticles = 600;
            ParticleSystem.EmissionModule emission = system.emission;
            emission.enabled = false;
            ParticleSystem.ShapeModule shape = system.shape;
            shape.enabled = false;
            var renderer = host.GetComponent<ParticleSystemRenderer>();
            renderer.sharedMaterial = art.Spark;
            return system;
        }

        /// <summary>Sparks thrown out along a surface normal.</summary>
        public void Impact(Vector3 point, Vector3 normal, Color color, int count = 8)
        {
            if (sparks == null)
            {
                return;
            }
            var emit = new ParticleSystem.EmitParams { applyShapeToPosition = false };
            for (int i = 0; i < count; i++)
            {
                emit.position = point + normal * 0.05f;
                emit.velocity = (normal + Random.insideUnitSphere * 0.9f).normalized * Random.Range(2f, 8f);
                emit.startColor = Color.Lerp(color, Color.white, Random.value * 0.4f);
                sparks.Emit(emit, 1);
            }
        }

        public void Tracer(Vector3 from, Vector3 to, Color color, float width = 0.04f, float life = 0.07f)
        {
            Line line = TakeLine();
            line.Renderer.SetPosition(0, from);
            line.Renderer.SetPosition(1, to);
            line.Age = 0f;
            line.Life = life;
            line.Width = width;
            line.Color = color;
            line.Renderer.enabled = true;
            Apply(line, 0f);
        }

        /// <summary>The railgun's beam: wide, bright and slow to fade.</summary>
        public void Beam(Vector3 from, Vector3 to, Color color)
        {
            // Start a little ahead of the muzzle so the beam does not fill the shooter's view.
            Vector3 start = from + (to - from).normalized * 0.8f;
            Tracer(start, to, color, 0.12f, 0.5f);
            Tracer(start, to, Color.white, 0.04f, 0.35f);
        }

        public void MuzzleFlash(Vector3 position, Color color, float size = 0.35f) => Glow(position, color, size, 0.06f);

        public void Explosion(Vector3 position, float radius)
        {
            Glow(position, new Color(1f, 0.6f, 0.25f), radius * 1.6f, 0.35f);
            Glow(position, Color.white, radius * 0.7f, 0.15f);
            Impact(position, Vector3.up, new Color(1f, 0.55f, 0.2f), 40);
        }

        /// <summary>A soft additive sprite facing the camera that grows and fades.</summary>
        public void Glow(Vector3 position, Color color, float size, float life)
        {
            Flash flash = TakeFlash();
            flash.Transform.position = position;
            flash.Age = 0f;
            flash.Life = life;
            flash.Size = size;
            flash.Color = color;
            flash.Renderer.enabled = true;
            Apply(flash, 0f);
        }

        /// <summary>A fighter goes down: its pieces fly off and fall.</summary>
        public void Burst(Vector3 center, Material material, Vector3 push)
        {
            for (int i = 0; i < 9; i++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(cube.GetComponent<Collider>());
                cube.layer = Layers.Effect;
                cube.transform.SetParent(transform, false);
                cube.transform.position = center + Random.insideUnitSphere * 0.5f;
                cube.transform.localScale = Vector3.one * Random.Range(0.15f, 0.32f);
                cube.GetComponent<Renderer>().sharedMaterial = material;
                pieces.Add(new Piece
                {
                    Transform = cube.transform,
                    Velocity = push * 0.4f + Random.insideUnitSphere * 5f + Vector3.up * 4f,
                    Spin = Random.insideUnitSphere * 720f,
                });
            }
            Impact(center, Vector3.up, material.color, 18);
        }

        void Update()
        {
            float dt = Time.deltaTime;
            foreach (Line line in lines)
            {
                if (!line.Renderer.enabled)
                {
                    continue;
                }
                line.Age += dt;
                if (line.Age >= line.Life)
                {
                    line.Renderer.enabled = false;
                    continue;
                }
                Apply(line, line.Age / line.Life);
            }
            Camera view = Camera.main;
            foreach (Flash flash in flashes)
            {
                if (!flash.Renderer.enabled)
                {
                    continue;
                }
                flash.Age += dt;
                if (flash.Age >= flash.Life)
                {
                    flash.Renderer.enabled = false;
                    continue;
                }
                if (view != null)
                {
                    flash.Transform.rotation = view.transform.rotation;
                }
                Apply(flash, flash.Age / flash.Life);
            }
            for (int i = pieces.Count - 1; i >= 0; i--)
            {
                Piece piece = pieces[i];
                piece.Age += dt;
                piece.Velocity += Vector3.down * 18f * dt;
                Vector3 next = piece.Transform.position + piece.Velocity * dt;
                if (next.y < 0.1f && piece.Transform.position.y >= 0.1f)
                {
                    next.y = 0.1f;
                    piece.Velocity = new Vector3(piece.Velocity.x * 0.5f, -piece.Velocity.y * 0.3f, piece.Velocity.z * 0.5f);
                }
                piece.Transform.position = next;
                piece.Transform.Rotate(piece.Spin * dt);
                float shrink = Mathf.Clamp01((PieceLife - piece.Age) / 0.4f);
                piece.Transform.localScale = Vector3.one * 0.25f * shrink;
                if (piece.Age >= PieceLife)
                {
                    Destroy(piece.Transform.gameObject);
                    pieces.RemoveAt(i);
                }
            }
        }

        void Apply(Line line, float t)
        {
            float fade = 1f - t;
            line.Renderer.widthMultiplier = line.Width * (0.4f + 0.6f * fade);
            Color color = line.Color * fade;
            color.a = fade;
            line.Renderer.startColor = color;
            line.Renderer.endColor = color * 0.6f;
        }

        void Apply(Flash flash, float t)
        {
            flash.Transform.localScale = Vector3.one * flash.Size * (0.6f + 0.6f * t);
            block.SetColor("_TintColor", flash.Color * (1f - t) * 0.6f);
            flash.Renderer.SetPropertyBlock(block);
        }

        Line TakeLine()
        {
            foreach (Line line in lines)
            {
                if (!line.Renderer.enabled)
                {
                    return line;
                }
            }
            var host = new GameObject("Tracer");
            host.transform.SetParent(transform, false);
            host.layer = Layers.Effect;
            var renderer = host.AddComponent<LineRenderer>();
            renderer.sharedMaterial = art.Tracer;
            renderer.positionCount = 2;
            renderer.useWorldSpace = true;
            renderer.numCapVertices = 2;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            var created = new Line { Renderer = renderer };
            lines.Add(created);
            return created;
        }

        Flash TakeFlash()
        {
            foreach (Flash flash in flashes)
            {
                if (!flash.Renderer.enabled)
                {
                    return flash;
                }
            }
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Destroy(quad.GetComponent<Collider>());
            quad.name = "Glow";
            quad.layer = Layers.Effect;
            quad.transform.SetParent(transform, false);
            var renderer = quad.GetComponent<Renderer>();
            renderer.sharedMaterial = art.Glow;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            var created = new Flash { Transform = quad.transform, Renderer = renderer };
            flashes.Add(created);
            return created;
        }

        int CountLines()
        {
            int count = 0;
            foreach (Line line in lines)
            {
                count += line.Renderer.enabled ? 1 : 0;
            }
            return count;
        }

        int CountFlashes()
        {
            int count = 0;
            foreach (Flash flash in flashes)
            {
                count += flash.Renderer.enabled ? 1 : 0;
            }
            return count;
        }
    }
}
