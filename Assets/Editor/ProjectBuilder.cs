using System;
using System.IO;
using FlyingRush.Game;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

namespace FlyingRush.EditorTools
{
    /// <summary>
    /// Generates the textures, materials, sky, font atlases, art asset, icon, physics layers, scene
    /// and player settings from code, and builds the players, so every binary asset can be
    /// recreated from text. Batch mode:
    /// <code>
    /// Unity -batchmode -quit -projectPath . -executeMethod FlyingRush.EditorTools.ProjectBuilder.SetUpProject
    /// Unity -batchmode -quit -projectPath . -executeMethod FlyingRush.EditorTools.ProjectBuilder.BuildWindows -buildOutput &lt;folder&gt;
    /// Unity -batchmode -quit -projectPath . -executeMethod FlyingRush.EditorTools.ProjectBuilder.BuildWebGL -buildOutput &lt;folder&gt;
    /// </code>
    /// </summary>
    public static class ProjectBuilder
    {
        const string ScenePath = "Assets/Scenes/Main.unity";
        const string ArtFolder = "Assets/Art";
        const string ArtPath = "Assets/Art/GameArt.asset";
        const string IconPath = "Assets/Art/AppIcon.png";
        const string AudioFolder = "Assets/Audio";
        const string DisplayFontSource = "Assets/Fonts/Exo2-Black.ttf";
        const string DisplayFontPath = "Assets/UI/ExoBlack.asset";
        const string TextFontSource = "Assets/Fonts/Exo2-SemiBold.ttf";
        const string TextFontPath = "Assets/UI/ExoSemiBold.asset";
        const string StylePath = "Assets/UI/Hud.uss";
        const string ThemePath = "Assets/UI/HudTheme.tss";
        const string PanelSettingsPath = "Assets/UI/HudPanelSettings.asset";
        const string Ascii = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

        [UnityEditor.MenuItem("Flying Rush/Regenerate Art, Scene and Player Settings")]
        public static void SetUpProject()
        {
            NameLayers();
            CreateFont(DisplayFontSource, DisplayFontPath, "ExoBlack");
            CreateFont(TextFontSource, TextFontPath, "ExoSemiBold");
            // The style sheet refers to the fonts by path; re-import it so it picks up the new assets.
            AssetDatabase.ImportAsset(StylePath, ImportAssetOptions.ForceUpdate);
            GameArt art = CreateArt();
            PanelSettings panel = CreatePanelSettings();
            CreateScene(art, panel);
            ConfigurePlayer(CreateIcon());
            AssetDatabase.SaveAssets();
            Debug.Log("Flying Rush project set up.");
        }

        /// <summary>
        /// Enters play mode and lets ScreenshotScenarios capture the README screenshots, then exits.
        /// Run in batch mode without -quit: <c>-executeMethod ...CaptureScreenshots -screenshot &lt;folder&gt;</c>.
        /// </summary>
        public static void CaptureScreenshots()
        {
            EditorSceneManager.OpenScene(ScenePath);
            EditorApplication.isPlaying = true;
        }

        public static void BuildWindows() => Build(BuildTarget.StandaloneWindows64, "FlyingRush.exe");

        public static void BuildWebGL() => Build(BuildTarget.WebGL, string.Empty);

        // ---- Layers ----

        static void NameLayers()
        {
            var manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layers = manager.FindProperty("layers");
            layers.GetArrayElementAtIndex(Layers.World).stringValue = "World";
            layers.GetArrayElementAtIndex(Layers.Fighter).stringValue = "Fighter";
            layers.GetArrayElementAtIndex(Layers.Effect).stringValue = "Effect";
            manager.ApplyModifiedPropertiesWithoutUndo();
        }

        // ---- Font ----

        /// <summary>
        /// A static font atlas of the printable ASCII range from an Exo 2 TrueType font (used
        /// unmodified under the SIL Open Font License; the atlas only rasterises it).
        /// </summary>
        static FontAsset CreateFont(string sourcePath, string assetPath, string name)
        {
            if (AssetDatabase.LoadAssetAtPath<FontAsset>(assetPath) != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }
            var source = AssetDatabase.LoadAssetAtPath<Font>(sourcePath);
            FontAsset font = FontAsset.CreateFontAsset(source, 64, 6, GlyphRenderMode.SDFAA, 1024, 1024,
                AtlasPopulationMode.Dynamic, false);
            font.name = name;
            if (!font.TryAddCharacters(Ascii, out string missing))
            {
                Debug.LogWarning($"{name} is missing: {missing}");
            }
            font.atlasPopulationMode = AtlasPopulationMode.Static;
            AssetDatabase.CreateAsset(font, assetPath);
            foreach (Texture2D atlas in font.atlasTextures)
            {
                atlas.name = name + " Atlas";
                AssetDatabase.AddObjectToAsset(atlas, font);
            }
            font.material.name = name + " Material";
            AssetDatabase.AddObjectToAsset(font.material, font);
            EditorUtility.SetDirty(font);
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(assetPath);
            return font;
        }

        // ---- Art ----

        static GameArt CreateArt()
        {
            Directory.CreateDirectory(ArtFolder);
            var art = AssetDatabase.LoadAssetAtPath<GameArt>(ArtPath);
            if (art == null)
            {
                art = ScriptableObject.CreateInstance<GameArt>();
                AssetDatabase.CreateAsset(art, ArtPath);
            }

            Shader standard = Shader.Find("Standard");
            Shader additive = Shader.Find("Legacy Shaders/Particles/Additive");

            Texture2D floor = SaveTexture("FloorPlates", 256, FloorPixel, TextureWrapMode.Repeat);
            Texture2D wall = SaveTexture("WallPanels", 256, WallPixel, TextureWrapMode.Repeat);
            Texture2D corrugated = SaveTexture("Corrugated", 256, CorrugatedPixel, TextureWrapMode.Repeat);
            Texture2D concrete = SaveTexture("Concrete", 256, ConcretePixel, TextureWrapMode.Repeat);
            Texture2D metal = SaveTexture("TreadPlate", 128, TreadPixel, TextureWrapMode.Repeat);
            Texture2D grid = SaveTexture("NeonGrid", 128, GridPixel, TextureWrapMode.Repeat);
            Texture2D dot = SaveTexture("SoftDot", 64, SoftDotPixel, TextureWrapMode.Clamp);
            SaveTexture("Vignette", 128, VignettePixel, TextureWrapMode.Clamp, "Assets/UI");

            art.Floor = Lit("Floor", standard, Palette.Floor * 1.25f, floor, 0.3f, null, 0.2f);
            art.Wall = Lit("Wall", standard, Palette.Wall * 1.3f, wall, 0.2f);
            art.Concrete = Lit("Concrete", standard, Palette.Concrete, concrete, 0.1f);
            art.Metal = Lit("Metal", standard, Palette.Metal * 1.2f, metal, 0.55f, null, 0.6f);
            art.Containers = new Material[7];
            for (int i = 0; i < art.Containers.Length; i++)
            {
                art.Containers[i] = Lit($"Container{i}", standard, Palette.Container(i), corrugated, 0.35f, null, 0.3f);
            }
            art.Neon = Lit("Neon", standard, Palette.Neon, null, 0.6f, Palette.Neon * 2.2f);
            art.Teal = Lit("Teal", standard, Palette.Teal, null, 0.6f, Palette.Teal * 1.8f);
            art.Grid = Lit("Grid", standard, new Color(0.05f, 0.03f, 0.08f), grid, 0.3f, Palette.Neon * 1.8f);
            art.Grid.SetTexture("_EmissionMap", grid);
            art.Pad = Lit("Pad", standard, Palette.Amber, null, 0.6f, Palette.Amber * 1.6f);
            art.Fighter = Lit("Fighter", standard, Color.white, null, 0.45f, null, 0.15f);
            art.Visor = Lit("Visor", standard, new Color(0.1f, 0.1f, 0.12f), null, 0.9f, Palette.Cyan * 1.6f);
            art.Gun = Lit("Gun", standard, new Color(0.13f, 0.12f, 0.15f), null, 0.6f, null, 0.7f);
            art.Tracer = Additive("Tracer", additive, null);
            art.Glow = Additive("Glow", additive, dot);
            art.Spark = Additive("Spark", additive, dot);
            art.Sky = CreateSky();

            art.Rifle = Clip("rifle");
            art.Shotgun = Clip("shotgun");
            art.Sniper = Clip("sniper");
            art.RocketFire = Clip("rocket");
            art.Explosion = Clip("explosion");
            art.Railgun = Clip("railgun");
            art.Reload = Clip("reload");
            art.Empty = Clip("empty");
            art.Switch = Clip("switch");
            art.Pickup = Clip("pickup");
            art.Jump = Clip("jump");
            art.AirJump = Clip("air_jump");
            art.Land = Clip("land");
            art.Hit = Clip("hit");
            art.Headshot = Clip("headshot");
            art.Kill = Clip("kill");
            art.Hurt = Clip("hurt");
            art.Death = Clip("death");
            art.Countdown = Clip("countdown");
            art.Go = Clip("go");
            art.Streak = Clip("streak");
            art.Win = Clip("win");
            art.Lose = Clip("lose");
            art.Move = Clip("move");
            art.Select = Clip("select");
            art.Pause = Clip("pause");
            art.TitleMusic = Clip("title_theme", music: true);
            art.BattleMusic = Clip("battle_theme", music: true);
            EditorUtility.SetDirty(art);
            return art;
        }

        static Material MaterialAt(string name, Shader shader)
        {
            string path = $"{ArtFolder}/{name}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }
            material.shader = shader;
            return material;
        }

        /// <summary>A Standard material; an emission colour turns on its glow.</summary>
        static Material Lit(string name, Shader shader, Color color, Texture2D texture, float smoothness,
            Color? emission = null, float metallic = 0f)
        {
            Material material = MaterialAt(name, shader);
            color.a = 1f;
            material.color = color;
            material.mainTexture = texture;
            material.SetFloat("_Glossiness", smoothness);
            material.SetFloat("_Metallic", metallic);
            if (emission.HasValue)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", emission.Value);
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
            }
            else
            {
                material.DisableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", Color.black);
                material.SetTexture("_EmissionMap", null);
            }
            material.enableInstancing = true;
            EditorUtility.SetDirty(material);
            return material;
        }

        static Material Additive(string name, Shader shader, Texture2D texture)
        {
            Material material = MaterialAt(name, shader);
            material.mainTexture = texture;
            material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0.5f));
            EditorUtility.SetDirty(material);
            return material;
        }

        /// <summary>
        /// A dusky violet sky on Unity's six-sided skybox shader: the sides fade from a magenta glow
        /// at the horizon to deep violet with a few stars, the top is the deep violet.
        /// </summary>
        static Material CreateSky()
        {
            Texture2D side = SaveTexture("SkySide", 256, SkySidePixel, TextureWrapMode.Clamp);
            Texture2D top = SaveTexture("SkyTop", 64, (u, v) => SkySidePixel(u, 1f), TextureWrapMode.Clamp);
            Texture2D bottom = SaveTexture("SkyBottom", 64, (u, v) => Palette.Hex("#1a0f22"), TextureWrapMode.Clamp);
            Material sky = MaterialAt("Sky", Shader.Find("Skybox/6 Sided"));
            sky.SetTexture("_FrontTex", side);
            sky.SetTexture("_BackTex", side);
            sky.SetTexture("_LeftTex", side);
            sky.SetTexture("_RightTex", side);
            sky.SetTexture("_UpTex", top);
            sky.SetTexture("_DownTex", bottom);
            sky.SetColor("_Tint", new Color(0.5f, 0.5f, 0.5f, 0.5f));
            sky.SetFloat("_Exposure", 1f);
            EditorUtility.SetDirty(sky);
            return sky;
        }

        static Color SkySidePixel(float u, float v)
        {
            Color horizon = Palette.Hex("#8a3a6e");
            Color middle = Palette.Hex("#3a1d52");
            Color high = Palette.Hex("#150a24");
            float h = Mathf.Clamp01((v - 0.5f) * 2f);
            Color color = v < 0.5f ? Color.Lerp(Palette.Hex("#1a0f22"), horizon, Mathf.Pow(v * 2f, 6f))
                : h < 0.35f ? Color.Lerp(horizon, middle, h / 0.35f) : Color.Lerp(middle, high, (h - 0.35f) / 0.65f);
            // A few faint stars high up.
            float star = Mathf.PerlinNoise(u * 140f, v * 140f);
            if (v > 0.7f && star > 0.86f)
            {
                color = Color.Lerp(color, Color.white, (star - 0.86f) * 4f * (v - 0.7f) / 0.3f);
            }
            color.a = 1f;
            return color;
        }

        /// <summary>A soft frame, transparent in the middle, for the low-health vignette.</summary>
        static Color VignettePixel(float u, float v)
        {
            float dx = Mathf.Abs(u * 2f - 1f), dy = Mathf.Abs(v * 2f - 1f);
            float edge = Mathf.Max(dx, dy * 0.95f);
            float radial = Mathf.Sqrt(dx * dx + dy * dy) / 1.414f;
            float alpha = Mathf.Clamp01((Mathf.Max(edge, radial * 1.05f) - 0.55f) / 0.45f);
            alpha = alpha * alpha;
            return new Color(1f, 1f, 1f, alpha);
        }

        static Texture2D SaveTexture(string name, int size, Func<float, float, Color> pixel, TextureWrapMode wrap,
            string folder = ArtFolder)
        {
            string path = $"{folder}/{name}.png";
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color[size * size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    pixels[y * size + x] = pixel((x + 0.5f) / size, (y + 0.5f) / size);
                }
            }
            texture.SetPixels(pixels);
            texture.Apply();
            File.WriteAllBytes(path, texture.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(texture);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            importer.alphaIsTransparency = name == "SoftDot" || name == "Vignette";
            if (name.StartsWith("Sky"))
            {
                importer.mipmapEnabled = false;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
            }
            importer.wrapMode = wrap;
            importer.anisoLevel = 4;
            importer.SaveAndReimport();
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }

        static float Grain(float u, float v, float scale) => Mathf.PerlinNoise(u * scale + 3.1f, v * scale + 7.7f);

        /// <summary>Square steel floor plates with seams and wear.</summary>
        static Color FloorPixel(float u, float v)
        {
            float cells = 2f;
            float cu = u * cells, cv = v * cells;
            float fu = cu - Mathf.Floor(cu), fv = cv - Mathf.Floor(cv);
            float edge = Mathf.Min(Mathf.Min(fu, 1f - fu), Mathf.Min(fv, 1f - fv));
            float seam = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((edge - 0.01f) / 0.025f));
            int tile = (int)Mathf.Floor(cu) * 3 + (int)Mathf.Floor(cv) * 5;
            float shade = 0.88f + 0.08f * (tile % 3);
            float wear = 0.92f + 0.12f * Grain(u, v, 9f) - 0.06f * Grain(u, v, 40f);
            float bolt = (Mathf.Abs(fu - 0.06f) < 0.02f || Mathf.Abs(fu - 0.94f) < 0.02f) && (Mathf.Abs(fv - 0.06f) < 0.02f || Mathf.Abs(fv - 0.94f) < 0.02f) ? 1.25f : 1f;
            float value = Mathf.Lerp(0.45f, shade * wear * bolt, seam);
            return new Color(value, value * 0.97f, value * 1.02f, 1f);
        }

        /// <summary>Tall wall panels with horizontal ribs, stained toward the bottom.</summary>
        static Color WallPixel(float u, float v)
        {
            float panel = u * 3f - Mathf.Floor(u * 3f);
            float gap = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((Mathf.Min(panel, 1f - panel) - 0.01f) / 0.02f));
            float rib = Mathf.Abs(Mathf.Sin(v * Mathf.PI * 8f)) > 0.97f ? 0.82f : 1f;
            float rust = Grain(u, v, 6f) * 0.25f + Grain(u, v, 25f) * 0.1f;
            float value = Mathf.Lerp(0.5f, (0.85f + rust) * rib, gap);
            return new Color(value, value * 0.92f, value * 0.95f, 1f);
        }

        /// <summary>The corrugated side of a shipping container, with rust streaks.</summary>
        static Color CorrugatedPixel(float u, float v)
        {
            float wave = 0.5f + 0.5f * Mathf.Sin(u * Mathf.PI * 2f * 10f);
            float ridge = 0.78f + 0.3f * wave;
            float streak = Grain(u * 6f, v * 0.6f, 4f);
            float rust = Mathf.Clamp01((streak - 0.55f) * 3f) * 0.25f;
            float edge = v < 0.04f || v > 0.96f ? 0.7f : 1f;
            float value = ridge * edge * (0.95f + 0.1f * Grain(u, v, 30f));
            return new Color(value * (1f - rust * 0.2f), value * (1f - rust), value * (1f - rust * 1.2f), 1f);
        }

        /// <summary>Concrete with pores and formwork lines.</summary>
        static Color ConcretePixel(float u, float v)
        {
            float base_ = 0.82f + 0.14f * Grain(u, v, 5f) - 0.08f * Grain(u, v, 48f);
            float line = Mathf.Abs(v * 2f - Mathf.Round(v * 2f)) < 0.006f ? 0.8f : 1f;
            float pore = Grain(u, v, 90f) > 0.78f ? 0.85f : 1f;
            float value = base_ * line * pore;
            return new Color(value, value * 0.98f, value * 0.97f, 1f);
        }

        /// <summary>Diamond tread plate.</summary>
        static Color TreadPixel(float u, float v)
        {
            float a = u * 8f + v * 8f, b = u * 8f - v * 8f;
            float da = Mathf.Abs(a - Mathf.Round(a)), db = Mathf.Abs(b - Mathf.Round(b));
            float raised = (Mathf.Repeat(Mathf.Floor(u * 8f) + Mathf.Floor(v * 8f), 2f) < 1f ? da : db) < 0.08f ? 1.25f : 1f;
            float value = (0.75f + 0.1f * Grain(u, v, 12f)) * raised;
            return new Color(value, value, value * 1.04f, 1f);
        }

        /// <summary>A glowing grid of small squares, like the 2021 map's purple panels.</summary>
        static Color GridPixel(float u, float v)
        {
            float fu = u * 8f - Mathf.Floor(u * 8f), fv = v * 8f - Mathf.Floor(v * 8f);
            float line = Mathf.Min(Mathf.Min(fu, 1f - fu), Mathf.Min(fv, 1f - fv));
            float glow = Mathf.Clamp01(1f - line / 0.12f);
            float cell = Mathf.Clamp01(1f - Vector2.Distance(new Vector2(fu, fv), new Vector2(0.5f, 0.5f)) / 0.3f) * 0.35f;
            float value = Mathf.Max(glow, cell);
            return new Color(value, value, value, 1f);
        }

        static Color SoftDotPixel(float u, float v)
        {
            float dx = u * 2f - 1f, dy = v * 2f - 1f;
            float alpha = Mathf.Clamp01(1f - Mathf.Sqrt(dx * dx + dy * dy));
            alpha = alpha * alpha * (3f - 2f * alpha);
            return new Color(alpha, alpha, alpha, alpha);
        }

        static AudioClip Clip(string name, bool music = false)
        {
            string path = $"{AudioFolder}/{name}.ogg";
            var importer = AssetImporter.GetAtPath(path) as AudioImporter;
            if (importer == null)
            {
                Debug.LogWarning($"Missing {path}: run python tools/audio/build.py first.");
                return null;
            }
            AudioImporterSampleSettings sample = importer.defaultSampleSettings;
            sample.loadType = music ? AudioClipLoadType.Streaming : AudioClipLoadType.DecompressOnLoad;
            importer.defaultSampleSettings = sample;
            importer.forceToMono = true;
            importer.SaveAndReimport();
            return AssetDatabase.LoadAssetAtPath<AudioClip>(path);
        }

        // ---- UI ----

        static PanelSettings CreatePanelSettings()
        {
            var panel = AssetDatabase.LoadAssetAtPath<PanelSettings>(PanelSettingsPath);
            if (panel == null)
            {
                panel = ScriptableObject.CreateInstance<PanelSettings>();
                AssetDatabase.CreateAsset(panel, PanelSettingsPath);
            }
            panel.themeStyleSheet = AssetDatabase.LoadAssetAtPath<ThemeStyleSheet>(ThemePath);
            panel.scaleMode = PanelScaleMode.ScaleWithScreenSize;
            panel.referenceResolution = new Vector2Int(1280, 720);
            panel.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
            panel.match = 1f;
            panel.clearColor = false;
            EditorUtility.SetDirty(panel);
            return panel;
        }

        // ---- Scene ----

        static void CreateScene(GameArt art, PanelSettings panel)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            RenderSettings.skybox = art.Sky;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = Palette.Hex("#8a78b8");
            RenderSettings.ambientEquatorColor = Palette.Hex("#7a4a62");
            RenderSettings.ambientGroundColor = Palette.Hex("#2c2236");
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = Palette.Fog;
            RenderSettings.fogStartDistance = 45f;
            RenderSettings.fogEndDistance = 170f;
            RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;

            var sun = new GameObject("Sun");
            sun.transform.rotation = Quaternion.Euler(52f, -30f, 0f);
            var light = sun.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = Palette.Hex("#ffd2b8");
            light.intensity = 1.1f;
            light.shadows = LightShadows.Soft;
            light.shadowStrength = 0.75f;
            RenderSettings.sun = light;

            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            var camera = cameraObject.AddComponent<Camera>();
            camera.fieldOfView = 85f;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 260f;
            camera.clearFlags = CameraClearFlags.Skybox;
            cameraObject.AddComponent<AudioListener>();

            var ui = new GameObject("HUD");
            var document = ui.AddComponent<UIDocument>();
            document.panelSettings = panel;
            Hud hud = ui.AddComponent<Hud>();

            var game = new GameObject("Game");
            var director = game.AddComponent<GameDirector>();
            var serialized = new SerializedObject(director);
            serialized.FindProperty("art").objectReferenceValue = art;
            serialized.FindProperty("hud").objectReferenceValue = hud;
            serialized.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
        }

        static void ConfigurePlayer(Texture2D icon)
        {
            PlayerSettings.companyName = "Trysac";
            PlayerSettings.productName = "Flying Rush";
            PlayerSettings.SetIcons(NamedBuildTarget.Unknown, new[] { icon }, IconKind.Any);
            int[] sizes = PlayerSettings.GetIconSizes(NamedBuildTarget.Standalone, IconKind.Any);
            var standaloneIcons = new Texture2D[sizes.Length];
            Array.Fill(standaloneIcons, icon);
            PlayerSettings.SetIcons(NamedBuildTarget.Standalone, standaloneIcons, IconKind.Any);
            PlayerSettings.colorSpace = ColorSpace.Linear;
            PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
            PlayerSettings.defaultScreenWidth = 1280;
            PlayerSettings.defaultScreenHeight = 720;
            PlayerSettings.resizableWindow = true;
            PlayerSettings.runInBackground = false;
            PlayerSettings.defaultWebScreenWidth = 1280;
            PlayerSettings.defaultWebScreenHeight = 720;
            PlayerSettings.SplashScreen.showUnityLogo = false;
            // Static hosts such as GitHub Pages do not send Content-Encoding for .br/.gz files.
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
            PlayerSettings.WebGL.decompressionFallback = false;
            QualitySettings.shadowDistance = 60f;
        }

        // ---- Icon: a fighter mid-air over the cage, with its jump trail ----

        static Texture2D CreateIcon()
        {
            const int size = 1024;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color[size * size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    pixels[y * size + x] = IconPixel(x + 0.5f - size / 2f, y + 0.5f - size / 2f);
                }
            }
            texture.SetPixels(pixels);
            texture.Apply();
            File.WriteAllBytes(IconPath, texture.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(texture);

            AssetDatabase.ImportAsset(IconPath, ImportAssetOptions.ForceUpdate);
            var importer = (TextureImporter)AssetImporter.GetAtPath(IconPath);
            importer.alphaIsTransparency = true;
            importer.mipmapEnabled = false;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
            return AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);
        }

        static Color IconPixel(float x, float y)
        {
            const float aa = 1.5f;
            float tile = RoundedBox(x, y, 470f, 470f, 170f);
            Color color = Color.Lerp(Palette.Hex("#1a0f26"), Palette.Hex("#5a2a6a"), Mathf.Clamp01((y + 470f) / 940f));
            color.a = Coverage(-tile, aa);

            // Containers along the bottom.
            float left = RoundedBox(x + 230f, y + 330f, 200f, 90f, 10f);
            float right = RoundedBox(x - 260f, y + 360f, 170f, 70f, 10f);
            color = Over(color, Palette.Container(0) * 0.9f, Coverage(-left, aa));
            color = Over(color, Palette.Container(1) * 0.9f, Coverage(-right, aa));
            for (int i = -4; i <= 4; i++)
            {
                float rib = RoundedBox(x + 230f + i * 40f, y + 330f, 6f, 80f, 3f);
                color = Over(color, Palette.Container(0) * 0.6f, Coverage(-rib, aa) * Coverage(-left, aa));
            }

            // Seven jump rings rising from the left, the "flying" trail.
            for (int i = 0; i < 7; i++)
            {
                float t = i / 6f;
                float cx = -300f + t * 340f, cy = -250f + t * 420f;
                float ring = Mathf.Abs(Length((x - cx) / 1.8f, y - cy) - 26f) - 5f;
                color = Over(color, Palette.Cyan, Coverage(-ring, aa) * (0.35f + 0.65f * t));
            }

            // The fighter: a capsule body with a glowing visor, top right.
            float body = RoundedBox(x - 150f, y - 170f, 95f, 140f, 90f);
            color = Over(color, Palette.Amber, Coverage(-body, aa));
            float head = Length(x - 150f, y - 340f) - 78f;
            color = Over(color, Palette.Amber, Coverage(-head, aa));
            float visor = RoundedBox(x - 175f, y - 345f, 60f, 22f, 18f);
            color = Over(color, Palette.Cyan, Coverage(-visor, aa));
            float pack = RoundedBox(x - 50f, y - 190f, 30f, 70f, 14f);
            color = Over(color, Palette.Hex("#2a2030"), Coverage(-pack, aa));
            float flame = Length((x - 40f) / 0.6f, y - 90f) - 40f;
            color = Over(color, Palette.Neon, Coverage(-flame, aa) * 0.9f);
            return color;
        }

        static float Length(float x, float y) => Mathf.Sqrt(x * x + y * y);

        static float RoundedBox(float x, float y, float halfWidth, float halfHeight, float radius)
        {
            float qx = Mathf.Abs(x) - (halfWidth - radius);
            float qy = Mathf.Abs(y) - (halfHeight - radius);
            float outside = new Vector2(Mathf.Max(qx, 0f), Mathf.Max(qy, 0f)).magnitude;
            return outside + Mathf.Min(Mathf.Max(qx, qy), 0f) - radius;
        }

        static float Coverage(float inside, float width) => Mathf.Clamp01(inside / width + 0.5f);

        static Color Over(Color below, Color above, float amount)
        {
            Color mixed = Color.Lerp(below, above, amount);
            mixed.a = below.a;
            return mixed;
        }

        // ---- Builds ----

        static void Build(BuildTarget target, string fileName)
        {
            string folder = Argument("-buildOutput");
            if (string.IsNullOrEmpty(folder))
            {
                throw new ArgumentException("Pass -buildOutput <folder> outside the repository.");
            }
            var options = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                target = target,
                locationPathName = string.IsNullOrEmpty(fileName) ? folder : Path.Combine(folder, fileName),
                options = BuildOptions.None,
            };
            BuildReport report = BuildPipeline.BuildPlayer(options);
            BuildSummary summary = report.summary;
            Debug.Log($"Build {target}: {summary.result}, {summary.totalSize / (1024 * 1024f):0.0} MB, {summary.totalErrors} errors.");
            if (Application.isBatchMode)
            {
                EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
            }
        }

        static string Argument(string name)
        {
            string[] args = Environment.GetCommandLineArgs();
            int index = Array.IndexOf(args, name);
            return index >= 0 && index + 1 < args.Length ? args[index + 1] : null;
        }
    }
}
