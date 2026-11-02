using System;
using System.Collections;
using System.IO;
using FlyingRush.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace FlyingRush.Game
{
    /// <summary>
    /// Opt-in capture of README screenshots: <c>FlyingRush.exe -screenshot &lt;folder&gt; -scenario title,fight,...</c>.
    /// Without the -screenshot argument nothing is created, so normal runs and WebGL builds
    /// (which have no command line) are unaffected. The same hook runs inside the editor
    /// (ProjectBuilder.CaptureScreenshots), which renders the camera and the HUD off screen and
    /// blends them, for machines that cannot run the player.
    /// </summary>
    public sealed class ScreenshotScenarios : MonoBehaviour
    {
        const string DefaultScenarios = "title,fight,flying,sniper,railgun,tunnel,team,scoreboard,results,pause,howto";
        const int Width = 1280;
        const int Height = 720;

        string folder;
        string[] scenarios;
        RenderTexture sceneTarget;
        RenderTexture uiTarget;
        GameDirector director;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void StartWhenRequested()
        {
            string target = Argument("-screenshot");
            if (string.IsNullOrEmpty(target))
            {
                return;
            }
            var host = new GameObject(nameof(ScreenshotScenarios)).AddComponent<ScreenshotScenarios>();
            host.folder = target;
            host.scenarios = (Argument("-scenario") ?? DefaultScenarios).Split(',');
        }

        IEnumerator Start()
        {
            Directory.CreateDirectory(folder);
            director = FindAnyObjectByType<GameDirector>();
            director.UseMemoryStore();
            if (Application.isEditor)
            {
                RenderOffscreen();
            }
            yield return new WaitForSecondsRealtime(0.5f);
            foreach (string raw in scenarios)
            {
                string scenario = raw.Trim();
                director.Seed(StableSeed(scenario));
                director.ScriptedInput = false;
                director.BotsFrozen = false;
                yield return Stage(scenario);
                Texture2D shot;
                if (Application.isEditor)
                {
                    yield return null;
                    shot = CaptureOffscreen();
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                    shot = ScreenCapture.CaptureScreenshotAsTexture();
                }
                File.WriteAllBytes(Path.Combine(folder, scenario + ".png"), shot.EncodeToPNG());
                Destroy(shot);
                Debug.Log($"Saved screenshot {scenario}.png");
                Time.timeScale = 1f;
            }
#if UNITY_EDITOR
            UnityEditor.EditorApplication.Exit(0);
#else
            Application.Quit();
#endif
        }

        IEnumerator Stage(string scenario)
        {
            switch (scenario)
            {
                case "title":
                    director.ToTitle();
                    yield return new WaitForSecondsRealtime(6f);
                    break;
                case "howto":
                    director.ToTitle();
                    director.Activate("howto");
                    yield return new WaitForSecondsRealtime(1f);
                    break;
                case "fight":
                    yield return Match(MatchMode.FreeForAll);
                    director.BotsFrozen = true;
                    FighterBody target = Bot(0);
                    Stand(target, new Vector3(-15f, 0f, 4.5f), 250f);
                    Stand(Bot(1), new Vector3(-3f, 6f, -2f), 260f);
                    Stand(Bot(2), new Vector3(-17f, 7.5f, 9f), 200f);
                    PlacePlayer(new Vector3(-31f, 0f, 3f), 90f);
                    AimAt(target.Chest + Vector3.up * 0.25f);
                    yield return Shoot(5, 0.1f);
                    break;
                case "flying":
                    yield return Match(MatchMode.FreeForAll);
                    director.BotsFrozen = true;
                    PlacePlayer(new Vector3(-12f, 10f, -24f), 35f);
                    for (int i = 0; i < 4; i++)
                    {
                        director.Player.Body.Drive(Vector3.zero, true, false, 0.02f);
                    }
                    director.Player.Body.Look(35f, -24f);
                    yield return new WaitForSecondsRealtime(0.12f);
                    director.Player.Body.Look(35f, -24f);
                    break;
                case "sniper":
                    yield return Match(MatchMode.FreeForAll);
                    director.BotsFrozen = true;
                    director.Player.Body.Loadout.PickUp(WeaponKind.Sniper);
                    PlacePlayer(new Vector3(-31.5f, 9.5f, 4f), 90f);
                    Stand(Bot(0), new Vector3(2f, 6f, 1f), 270f);
                    AimAt(Bot(0).Eye);
                    director.ScriptedAim = true;
                    yield return new WaitForSecondsRealtime(0.6f);
                    AimAt(Bot(0).Eye);
                    yield return new WaitForSecondsRealtime(0.1f);
                    break;
                case "railgun":
                    yield return Match(MatchMode.FreeForAll);
                    director.BotsFrozen = true;
                    director.Player.Body.Loadout.PickUp(WeaponKind.Railgun);
                    PlacePlayer(new Vector3(-33f, 0f, 3.5f), 90f);
                    Stand(Bot(0), new Vector3(-24f, 0f, 3f), 270f);
                    Stand(Bot(1), new Vector3(-15f, 0f, 2.5f), 270f);
                    yield return new WaitForSecondsRealtime(0.3f);
                    director.Hud.ClearMessages();
                    AimAt(Vector3.Lerp(Bot(0).Chest, Bot(1).Chest, 0.5f));
                    yield return Shoot(1, 0.06f);
                    break;
                case "tunnel":
                    yield return Match(MatchMode.FreeForAll);
                    director.BotsFrozen = true;
                    director.ScriptedInput = true;
                    PlacePlayer(new Vector3(31f, 0f, -12f), 270f);
                    director.Player.Body.Drive(Vector3.zero, false, true, 0.02f);
                    Stand(Bot(0), new Vector3(15f, 0f, -11f), 90f);
                    AimAt(Bot(0).Chest);
                    yield return new WaitForSecondsRealtime(0.5f);
                    break;
                case "team":
                    yield return Match(MatchMode.TeamDeathmatch);
                    yield return new WaitForSecondsRealtime(2.5f);
                    director.Hud.ClearMessages();
                    PlacePlayer(new Vector3(0f, 3f, -38f), 0f);
                    director.Player.Body.Look(0f, 4f);
                    yield return new WaitForSecondsRealtime(0.4f);
                    break;
                case "scoreboard":
                    yield return Match(MatchMode.FreeForAll);
                    Score(new[] { 6, 9, 4, 11, 2, 7, 5, 3 });
                    // Let the pieces of the staged deaths settle before the shot.
                    yield return new WaitForSecondsRealtime(1.6f);
                    director.Hud.ClearMessages();
                    director.ShowScoreboard = true;
                    yield return new WaitForSecondsRealtime(0.3f);
                    break;
                case "results":
                    yield return Match(MatchMode.FreeForAll);
                    Score(new[] { 12, 9, 4, 14, 2, 7, 5, 3 });
                    director.Match.Damage(0, 4, 1000f, WeaponKind.Railgun, true);
                    director.Match.Damage(0, 5, 1000f, WeaponKind.Rifle, false);
                    director.Match.Damage(0, 6, 1000f, WeaponKind.Rifle, true);
                    float waited = 0f;
                    while (director.Screen != GameScreen.Results && waited < 10f)
                    {
                        waited += Time.unscaledDeltaTime;
                        yield return null;
                    }
                    yield return new WaitForSecondsRealtime(0.6f);
                    break;
                case "pause":
                    yield return Match(MatchMode.FreeForAll);
                    yield return new WaitForSecondsRealtime(2f);
                    director.TogglePause();
                    director.Activate("settings");
                    yield return new WaitForSecondsRealtime(0.5f);
                    break;
                default:
                    Debug.LogWarning($"Unknown screenshot scenario '{scenario}'.");
                    break;
            }
        }

        IEnumerator Match(MatchMode mode)
        {
            director.Settings.Mode = mode;
            director.Settings.Bots = 7;
            director.StartMatch();
            director.ScriptedInput = true;
            yield return new WaitForSecondsRealtime(director.Match.Settings.CountdownSeconds + 0.3f);
            // Spawn protection off, so the staged hits land, and no countdown banner.
            director.Match.Tick(Vitals.SpawnProtectionSeconds);
            director.Hud.ClearMessages();
        }

        FighterBody Bot(int index) => director.Bots[index].Body;

        void Stand(FighterBody body, Vector3 feet, float yaw)
        {
            body.Place(feet, yaw);
            body.Look(yaw, 0f);
        }

        void PlacePlayer(Vector3 feet, float yaw)
        {
            director.Player.Body.Place(feet, yaw);
            director.Player.SnapCamera();
        }

        void AimAt(Vector3 point)
        {
            Vector3 to = point - director.Player.Body.Eye;
            float yaw = Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg;
            float pitch = Mathf.Atan2(to.y, new Vector2(to.x, to.z).magnitude) * Mathf.Rad2Deg;
            director.Player.Body.Look(yaw, pitch);
        }

        IEnumerator Shoot(int shots, float interval)
        {
            for (int i = 0; i < shots; i++)
            {
                FighterBody body = director.Player.Body;
                director.Combat.Fire(body, body.AimDirection, false);
                yield return new WaitForSecondsRealtime(interval);
            }
        }

        /// <summary>Gives the fighters kills by having each kill another, letting the victim respawn each time.</summary>
        void Score(int[] kills)
        {
            Match match = director.Match;
            int count = match.Fighters.Count;
            for (int i = 0; i < kills.Length && i < count; i++)
            {
                for (int k = 0; k < kills[i]; k++)
                {
                    int victim = (i + 1 + k % (count - 1)) % count;
                    match.Damage(i, victim, 1000f, WeaponKind.Rifle, k % 4 == 0);
                    match.Tick(match.Settings.RespawnSeconds + 0.05f);
                    match.Tick(Vitals.SpawnProtectionSeconds + 0.05f);
                }
            }
        }

        /// <summary>Points the camera and the HUD panel at render textures of a fixed size.</summary>
        void RenderOffscreen()
        {
            sceneTarget = new RenderTexture(Width, Height, 24, RenderTextureFormat.ARGB32) { antiAliasing = 4 };
            uiTarget = new RenderTexture(Width, Height, 24, RenderTextureFormat.ARGB32);
            Camera.main.targetTexture = sceneTarget;
            var document = director.Hud.GetComponent<UIDocument>();
            PanelSettings panel = Instantiate(document.panelSettings);
            panel.targetTexture = uiTarget;
            panel.clearColor = true;
            panel.colorClearValue = Color.clear;
            document.panelSettings = panel;
        }

        /// <summary>Blends the HUD over the scene, like the screen would show them.</summary>
        Texture2D CaptureOffscreen()
        {
            Color[] scene = Read(sceneTarget);
            Color[] ui = Read(uiTarget);
            for (int i = 0; i < scene.Length; i++)
            {
                Color over = ui[i];
                Color result = Color.Lerp(scene[i], new Color(over.r, over.g, over.b), over.a);
                result.a = 1f;
                scene[i] = result;
            }
            var shot = new Texture2D(Width, Height, TextureFormat.RGBA32, false);
            shot.SetPixels(scene);
            shot.Apply();
            return shot;
        }

        static Color[] Read(RenderTexture target)
        {
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = target;
            var texture = new Texture2D(target.width, target.height, TextureFormat.RGBA32, false);
            texture.ReadPixels(new Rect(0, 0, target.width, target.height), 0, 0);
            texture.Apply();
            RenderTexture.active = previous;
            Color[] pixels = texture.GetPixels();
            Destroy(texture);
            return pixels;
        }

        static int StableSeed(string text)
        {
            int seed = 17;
            foreach (char c in text)
            {
                seed = seed * 31 + c;
            }
            return seed & 0xffff;
        }

        static string Argument(string name)
        {
            string[] args = Environment.GetCommandLineArgs();
            int index = Array.IndexOf(args, name);
            return index >= 0 && index + 1 < args.Length ? args[index + 1] : null;
        }
    }
}
