using FlyingRush.Core;
using UnityEngine;

namespace FlyingRush.Game
{
    /// <summary>
    /// The player's side of a fighter: reads the input, drives the body, turns the first-person
    /// camera with recoil and screen shake, zooms while aiming and shows the weapon in hand.
    /// </summary>
    public sealed class PlayerController : MonoBehaviour
    {
        Camera view;
        Transform viewmodelRoot;
        Transform viewmodel;
        Transform viewMuzzle;
        WeaponKind shown = (WeaponKind)(-1);
        GameArt art;
        Material accent;
        float recoil;
        float kick;
        float bob;
        float trauma;
        float eyeHeight;
        float noiseSeed;

        public FighterBody Body { get; private set; }
        public float BaseFov { get; set; } = 85f;
        public bool ShakeEnabled { get; set; } = true;
        /// <summary>True while aiming down the sights (the sniper's scope shows).</summary>
        public bool Aiming { get; private set; }
        /// <summary>The fighter that last killed the player, to look at while waiting to respawn.</summary>
        public FighterBody Killer { get; set; }
        /// <summary>Aims down the sights without the button (screenshots).</summary>
        public bool ForceAim { get; set; }
        /// <summary>The weapon in hand shows (not behind the pause menu or the results).</summary>
        public bool ShowViewmodel { get; set; } = true;

        public void Init(FighterBody body, Camera camera, GameArt gameArt)
        {
            Body = body;
            view = camera;
            art = gameArt;
            accent = new Material(art.Visor);
            accent.SetColor("_EmissionColor", Palette.Cyan * 0.8f);
            viewmodelRoot = new GameObject("Viewmodel").transform;
            viewmodelRoot.SetParent(view.transform, false);
            viewmodelRoot.localPosition = new Vector3(0.2f, -0.2f, 0.28f);
            viewmodelRoot.localScale = Vector3.one * 0.62f;
            eyeHeight = Jumps.StandHeight - 0.18f;
            noiseSeed = Random.value * 100f;
        }

        /// <summary>Adds screen shake (0..1, it decays on its own).</summary>
        public void Shake(float amount)
        {
            if (ShakeEnabled)
            {
                trauma = Mathf.Clamp01(trauma + amount);
            }
        }

        public void Kick(WeaponStats stats)
        {
            recoil += stats.Recoil;
            kick = 1f;
            Shake(stats.Recoil * 0.05f);
        }

        /// <summary>One frame of play: look, move, shoot.</summary>
        public void Tick(GameInput input, Combat combat, AudioDirector audio, bool canAct, float deltaTime)
        {
            if (Body == null)
            {
                return;
            }
            Fighter fighter = Body.Fighter;
            bool alive = fighter.Alive;
            Gun gun = Body.Loadout.Current;
            Aiming = alive && (ForceAim || (canAct && input.Aim)) && !gun.Reloading;
            float targetFov = Aiming ? Mathf.Min(BaseFov, gun.Stats.AimFov) : BaseFov;
            view.fieldOfView = Mathf.Lerp(view.fieldOfView, targetFov, 1f - Mathf.Exp(-18f * deltaTime));
            input.LookScale = view.fieldOfView / BaseFov;

            if (alive && canAct)
            {
                Body.Look(Body.Yaw + input.Look.x, Body.Pitch + input.Look.y);
                Vector3 forward = Quaternion.Euler(0f, Body.Yaw, 0f) * Vector3.forward;
                Vector3 right = Quaternion.Euler(0f, Body.Yaw, 0f) * Vector3.right;
                Vector3 wish = forward * input.Move.y + right * input.Move.x;
                bool jumpedFromGround = Body.Grounded;
                int jumpsBefore = Body.Jumps.Left;
                Body.Drive(wish, input.Jump, input.Crouch, deltaTime);
                if (Body.Jumps.Left < jumpsBefore || (input.Jump && jumpedFromGround && !Body.Grounded))
                {
                    audio.Play(jumpedFromGround ? art.Jump : art.AirJump, 0.55f, 1f + (Jumps.MaxJumps - Body.Jumps.Left) * 0.04f);
                }
                if (Body.LandedThisFrame > 7f)
                {
                    audio.Play(art.Land, Mathf.Clamp01(Body.LandedThisFrame / 20f));
                    Shake(Mathf.Clamp01(Body.LandedThisFrame / 60f));
                }

                if (input.Switch && Body.Loadout.Switch())
                {
                    audio.Play(art.Switch, 0.6f);
                }
                if (input.Reload && Body.Loadout.Current.StartReload())
                {
                    audio.Play(art.Reload, 0.7f);
                }
                gun = Body.Loadout.Current;
                bool trigger = gun.Stats.Automatic ? input.Fire : input.FirePressed;
                if (trigger)
                {
                    FireResult result = combat.Fire(Body, AimWithRecoil(), Aiming);
                    if (result == FireResult.Fired)
                    {
                        Kick(gun.Stats);
                    }
                    else if (result == FireResult.Reloading && input.FirePressed && gun.ReloadProgress < 0.05f)
                    {
                        audio.Play(art.Reload, 0.7f);
                    }
                    else if (result == FireResult.Empty && input.FirePressed)
                    {
                        audio.Play(art.Empty, 0.6f);
                    }
                }
            }
            else if (alive)
            {
                Body.Drive(Vector3.zero, false, false, deltaTime);
            }

            // The kick recovers quickly; part of the climb stays, so a long burst walks up.
            float recovered = recoil * Mathf.Min(1f, 9f * deltaTime);
            recoil -= recovered;
            if (alive && canAct)
            {
                Body.Look(Body.Yaw, Body.Pitch + recovered * 0.35f);
            }
            kick = Mathf.MoveTowards(kick, 0f, deltaTime * 8f);
            trauma = Mathf.MoveTowards(trauma, 0f, deltaTime * 1.4f);
            PlaceCamera(deltaTime);
            UpdateViewmodel(deltaTime);
        }

        /// <summary>The aim direction including the kick that has not recovered yet.</summary>
        Vector3 AimWithRecoil() => Quaternion.Euler(-(Body.Pitch + recoil * 0.65f), Body.Yaw, 0f) * Vector3.forward;

        void PlaceCamera(float deltaTime)
        {
            Fighter fighter = Body.Fighter;
            float shake = trauma * trauma;
            float time = Time.unscaledTime * 22f;
            float yawShake = shake * 3f * (Mathf.PerlinNoise(noiseSeed, time) * 2f - 1f);
            float pitchShake = shake * 3f * (Mathf.PerlinNoise(noiseSeed + 7f, time) * 2f - 1f);
            float roll = shake * 4f * (Mathf.PerlinNoise(noiseSeed + 13f, time) * 2f - 1f);
            if (!fighter.Alive)
            {
                // A death camera above the body that turns toward whoever got you.
                Vector3 above = Body.transform.position + Vector3.up * 3.2f - Body.AimDirection * 2.5f;
                view.transform.position = Vector3.Lerp(view.transform.position, above, 1f - Mathf.Exp(-3f * deltaTime));
                Vector3 look = Killer != null && Killer != Body ? Killer.Chest : Body.transform.position;
                Quaternion toward = Quaternion.LookRotation((look - view.transform.position).normalized);
                view.transform.rotation = Quaternion.Slerp(view.transform.rotation, toward, 1f - Mathf.Exp(-4f * deltaTime));
                return;
            }
            eyeHeight = Mathf.Lerp(eyeHeight, Body.Height - 0.18f, 1f - Mathf.Exp(-14f * deltaTime));
            Vector3 flat = new Vector3(Body.Velocity.x, 0f, Body.Velocity.z);
            if (Body.Grounded)
            {
                bob += flat.magnitude * deltaTime * 1.6f;
            }
            float bobOffset = Body.Grounded ? Mathf.Sin(bob * 2f) * 0.03f * Mathf.Clamp01(flat.magnitude / Jumps.RunSpeed) : 0f;
            view.transform.position = Body.transform.position + Vector3.up * (eyeHeight + bobOffset);
            view.transform.rotation = Quaternion.Euler(-(Body.Pitch + recoil * 0.65f) + pitchShake, Body.Yaw + yawShake, roll);
        }

        /// <summary>Snaps the camera to the eye at once (after a respawn).</summary>
        public void SnapCamera()
        {
            if (Body == null)
            {
                return;
            }
            eyeHeight = Body.Height - 0.18f;
            recoil = 0f;
            trauma = 0f;
            view.transform.position = Body.transform.position + Vector3.up * eyeHeight;
            view.transform.rotation = Quaternion.Euler(-Body.Pitch, Body.Yaw, 0f);
            view.fieldOfView = BaseFov;
        }

        void UpdateViewmodel(float deltaTime)
        {
            Gun gun = Body.Loadout.Current;
            if (gun.Kind != shown)
            {
                shown = gun.Kind;
                if (viewmodel != null)
                {
                    Destroy(viewmodel.gameObject);
                }
                viewMuzzle = Shapes.Weapon(gun.Kind, viewmodelRoot, art.Gun, accent);
                viewmodel = viewMuzzle.parent;
                foreach (Renderer part in viewmodel.GetComponentsInChildren<Renderer>())
                {
                    part.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    part.receiveShadows = false;
                }
                Body.MuzzleOverride = viewMuzzle;
                accent.SetColor("_EmissionColor", Palette.Of(gun.Kind) * 0.7f);
            }
            bool scoped = Aiming && gun.Kind == WeaponKind.Sniper;
            viewmodelRoot.gameObject.SetActive(Body.Fighter.Alive && !scoped && ShowViewmodel);
            float reload = gun.Reloading ? Mathf.Sin(gun.ReloadProgress * Mathf.PI) : 0f;
            Vector3 rest = Aiming ? new Vector3(0f, -0.1f, 0.24f) : new Vector3(0.17f, -0.17f, 0.24f);
            Vector3 flat = new Vector3(Body.Velocity.x, 0f, Body.Velocity.z);
            float sway = Body.Grounded ? Mathf.Sin(bob) * 0.012f * Mathf.Clamp01(flat.magnitude / Jumps.RunSpeed) : 0f;
            Vector3 target = rest + new Vector3(sway, -reload * 0.25f - Mathf.Abs(sway) * 0.5f, -kick * 0.08f);
            viewmodelRoot.localPosition = Vector3.Lerp(viewmodelRoot.localPosition, target, 1f - Mathf.Exp(-20f * deltaTime));
            viewmodelRoot.localRotation = Quaternion.Euler(-kick * 6f - reload * 30f, 0f, reload * 20f);
        }

        void OnDestroy()
        {
            if (accent != null)
            {
                Destroy(accent);
            }
            // The viewmodel hangs off the camera, which outlives this fighter.
            if (viewmodelRoot != null)
            {
                Destroy(viewmodelRoot.gameObject);
            }
        }
    }
}
