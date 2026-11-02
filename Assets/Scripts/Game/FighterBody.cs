using FlyingRush.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace FlyingRush.Game
{
    /// <summary>
    /// A fighter in the cage, the player's or a bot's: a character controller that runs, crouches
    /// and multi-jumps with the core's numbers, a body made of primitives in the fighter's colour,
    /// the weapon in its hands and its loadout. The player and the bot controllers drive it.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class FighterBody : MonoBehaviour
    {
        const float GroundAcceleration = 70f;
        const float AirAcceleration = 26f;
        const float MaxFallSpeed = 40f;
        const float Radius = 0.4f;
        const float EyeBelowTop = 0.18f;

        CharacterController controller;
        Transform model;
        Transform torso;
        Transform head;
        Transform hand;
        Transform weaponModel;
        Transform muzzle;
        WeaponKind shownWeapon = (WeaponKind)(-1);
        GameArt art;
        Material bodyMaterial;
        Material accentMaterial;
        bool firstPerson;
        float height = Jumps.StandHeight;
        float fallSpeedAtLanding;

        public Fighter Fighter { get; private set; }
        public Loadout Loadout { get; private set; } = new Loadout();
        public Jumps Jumps { get; } = new Jumps();
        public MapLayout Map { get; set; }
        public Vector3 Velocity { get; private set; }
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        public bool Grounded { get; private set; }
        public bool Crouched { get; private set; }
        public Color Color { get; private set; }
        public Material BodyMaterial => bodyMaterial;
        public bool IsPlayer => Fighter != null && Fighter.IsPlayer;
        /// <summary>Scales the damage this fighter deals (easier bots hit softer).</summary>
        public float DamageScale { get; set; } = 1f;

        /// <summary>Set for a frame when it lands hard enough to hear (the fall speed).</summary>
        public float LandedThisFrame { get; private set; }

        /// <summary>Where the camera sits and shots start.</summary>
        public Vector3 Eye => transform.position + Vector3.up * (height - EyeBelowTop);

        /// <summary>The middle of the body, where bots aim.</summary>
        public Vector3 Chest => transform.position + Vector3.up * (height * 0.62f);

        public float Height => height;

        public Vector3 AimDirection => Quaternion.Euler(-Pitch, Yaw, 0f) * Vector3.forward;

        /// <summary>Where the weapon's muzzle is in the world (the camera's viewmodel for the player).</summary>
        public Vector3 Muzzle => MuzzleOverride != null ? MuzzleOverride.position : muzzle != null ? muzzle.position : Eye;

        public Transform MuzzleOverride { get; set; }

        public void Init(Fighter fighter, GameArt gameArt, MapLayout map, bool isFirstPerson)
        {
            Fighter = fighter;
            art = gameArt;
            Map = map;
            firstPerson = isFirstPerson;
            gameObject.layer = Layers.Fighter;
            controller = GetComponent<CharacterController>();
            controller.radius = Radius;
            controller.height = height;
            controller.center = Vector3.up * (height / 2f);
            controller.slopeLimit = 50f;
            controller.stepOffset = 0.45f;
            controller.skinWidth = 0.06f;
            controller.minMoveDistance = 0f;
            Color = Palette.FighterColor(fighter);
            bodyMaterial = new Material(art.Fighter) { color = Color, name = fighter.Name };
            accentMaterial = new Material(art.Visor) { name = fighter.Name + " Visor" };
            accentMaterial.SetColor("_EmissionColor", Color.Lerp(Color, Color.white, 0.35f) * 1.8f);
            BuildModel();
        }

        void BuildModel()
        {
            model = new GameObject("Model").transform;
            model.SetParent(transform, false);
            torso = Shapes.Primitive(PrimitiveType.Capsule, "Torso", model, new Vector3(0f, 0.75f, 0f), new Vector3(0.72f, 0.62f, 0.5f), bodyMaterial).transform;
            Shapes.Primitive(PrimitiveType.Cube, "Belt", torso, new Vector3(0f, -0.2f, 0f), new Vector3(1.05f, 0.12f, 1.05f), art.Gun);
            Shapes.Primitive(PrimitiveType.Cube, "Pack", torso, new Vector3(0f, 0.25f, -0.55f), new Vector3(0.7f, 0.6f, 0.35f), art.Gun);
            Shapes.Primitive(PrimitiveType.Cube, "Thruster", torso, new Vector3(0f, -0.05f, -0.62f), new Vector3(0.4f, 0.2f, 0.2f), accentMaterial);
            head = Shapes.Primitive(PrimitiveType.Sphere, "Head", model, new Vector3(0f, 1.55f, 0f), new Vector3(0.46f, 0.46f, 0.46f), bodyMaterial).transform;
            Shapes.Primitive(PrimitiveType.Cube, "Visor", head, new Vector3(0f, 0.06f, 0.36f), new Vector3(0.8f, 0.28f, 0.3f), accentMaterial);
            hand = new GameObject("Hand").transform;
            hand.SetParent(model, false);
            hand.localPosition = new Vector3(0.24f, 1.2f, 0.3f);
            if (firstPerson)
            {
                foreach (Renderer part in model.GetComponentsInChildren<Renderer>())
                {
                    part.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                }
            }
        }

        public void Place(Vector3 feet, float yaw)
        {
            controller.enabled = false;
            transform.position = feet;
            controller.enabled = true;
            Velocity = Vector3.zero;
            Yaw = yaw;
            Pitch = 0f;
            Jumps.Land();
            SetCrouched(false, true);
            model.gameObject.SetActive(true);
        }

        public void Look(float yaw, float pitch)
        {
            Yaw = Mathf.Repeat(yaw, 360f);
            Pitch = Mathf.Clamp(pitch, -85f, 85f);
        }

        /// <summary>A fresh loadout: just the rifle.</summary>
        public void ResetLoadout() => Loadout = new Loadout();

        /// <summary>A push, from an explosion or a railgun hit.</summary>
        public void AddImpulse(Vector3 impulse) => Velocity += impulse;

        /// <summary>
        /// Moves the body one frame: <paramref name="wish"/> is the flat direction to run (length up to
        /// 1), a jump press uses one of the seven jumps, and crouching is held (or forced in a tunnel).
        /// </summary>
        public void Drive(Vector3 wish, bool jump, bool crouch, float deltaTime)
        {
            if (deltaTime <= 0f || !controller.enabled)
            {
                return;
            }
            LandedThisFrame = 0f;
            bool mustCrouch = Map != null && Map.MustCrouch(transform.position.ToCore());
            SetCrouched(crouch || mustCrouch, false);

            wish.y = 0f;
            wish = Vector3.ClampMagnitude(wish, 1f);
            float speed = Crouched ? Jumps.CrouchSpeed : Jumps.RunSpeed;
            Vector3 flat = new Vector3(Velocity.x, 0f, Velocity.z);
            Vector3 target = wish * speed;
            float acceleration = Grounded ? GroundAcceleration : AirAcceleration;
            flat = Vector3.MoveTowards(flat, target, acceleration * deltaTime);
            // In the air without input you keep your momentum, so a jump carries you across.
            if (!Grounded && wish.sqrMagnitude < 0.01f)
            {
                flat = new Vector3(Velocity.x, 0f, Velocity.z) * (1f - 0.4f * deltaTime);
            }
            float vertical = Velocity.y;
            if (jump && !Crouched && Jumps.TryJump(Grounded, out float upward))
            {
                vertical = upward;
                Grounded = false;
                // An air jump also turns you toward where you want to go.
                if (wish.sqrMagnitude > 0.01f && flat.magnitude < speed)
                {
                    flat = Vector3.Lerp(flat, wish * speed, 0.5f);
                }
            }
            vertical = Mathf.Max(vertical - Jumps.Gravity * deltaTime, -MaxFallSpeed);
            Velocity = new Vector3(flat.x, vertical, flat.z);

            CollisionFlags flags = controller.Move(Velocity * deltaTime);
            bool wasGrounded = Grounded;
            Grounded = (flags & CollisionFlags.Below) != 0;
            if (Grounded)
            {
                if (!wasGrounded)
                {
                    fallSpeedAtLanding = -Velocity.y;
                    LandedThisFrame = fallSpeedAtLanding;
                }
                Jumps.Land();
                Velocity = new Vector3(Velocity.x, -2f, Velocity.z);
            }
            if ((flags & CollisionFlags.Above) != 0 && Velocity.y > 0f)
            {
                Velocity = new Vector3(Velocity.x, 0f, Velocity.z);
            }
            AnimateModel(deltaTime);
        }

        void SetCrouched(bool wanted, bool force)
        {
            if (wanted == Crouched && !force)
            {
                return;
            }
            if (!wanted && !force && !RoomToStand())
            {
                return;
            }
            Crouched = wanted;
            height = wanted ? Jumps.CrouchHeight : Jumps.StandHeight;
            controller.height = height;
            controller.center = Vector3.up * (height / 2f);
            if (model != null)
            {
                model.localScale = new Vector3(1f, height / Jumps.StandHeight, 1f);
            }
        }

        bool RoomToStand()
        {
            Vector3 bottom = transform.position + Vector3.up * (Radius + 0.05f);
            Vector3 top = transform.position + Vector3.up * (Jumps.StandHeight - Radius);
            return !Physics.CheckCapsule(bottom, top, Radius * 0.95f, Layers.WorldMask, QueryTriggerInteraction.Ignore);
        }

        void AnimateModel(float deltaTime)
        {
            transform.rotation = Quaternion.Euler(0f, Yaw, 0f);
            if (head != null)
            {
                head.localRotation = Quaternion.Euler(-Pitch * 0.6f, 0f, 0f);
                hand.localRotation = Quaternion.Euler(-Pitch, 0f, 0f);
            }
            // A slight lean into the run.
            Vector3 local = transform.InverseTransformDirection(new Vector3(Velocity.x, 0f, Velocity.z));
            Quaternion lean = Quaternion.Euler(local.z * 1.2f, 0f, -local.x * 1.2f);
            torso.localRotation = Quaternion.Slerp(torso.localRotation, lean, 10f * deltaTime);
        }

        /// <summary>Shows the weapon in hand when it changes.</summary>
        public void ShowWeapon(WeaponKind kind)
        {
            if (kind == shownWeapon)
            {
                return;
            }
            shownWeapon = kind;
            if (weaponModel != null)
            {
                Destroy(weaponModel.gameObject);
            }
            muzzle = Shapes.Weapon(kind, hand, art.Gun, accentMaterial);
            weaponModel = muzzle.parent;
            if (firstPerson)
            {
                foreach (Renderer part in weaponModel.GetComponentsInChildren<Renderer>())
                {
                    part.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                }
            }
        }

        public void SetVisible(bool visible)
        {
            model.gameObject.SetActive(visible);
            controller.enabled = visible;
        }

        /// <summary>Whether a hit at this height is on the head.</summary>
        public bool IsHeadshot(Vector3 point) => DamageRules.IsHeadshot(point.y, transform.position.y, height);

        void OnDestroy()
        {
            if (bodyMaterial != null)
            {
                Destroy(bodyMaterial);
            }
            if (accentMaterial != null)
            {
                Destroy(accentMaterial);
            }
        }
    }
}
