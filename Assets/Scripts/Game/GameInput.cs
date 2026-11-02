using UnityEngine;
using UnityEngine.InputSystem;

namespace FlyingRush.Game
{
    public enum InputSource
    {
        KeyboardMouse,
        Gamepad,
    }

    /// <summary>
    /// Keyboard and mouse or a gamepad through the Input System, read once per frame with
    /// <see cref="Read"/>. Menus read the step and confirm helpers.
    /// </summary>
    public sealed class GameInput
    {
        const float StickDeadZone = 0.18f;
        const float StickStep = 0.6f;
        /// <summary>Degrees a second a fully pushed right stick turns the view at sensitivity 1.</summary>
        const float StickTurnPerSensitivity = 26f;

        int stickVertical;
        int stickHorizontal;
        int lastFrame = -1;

        static Keyboard Keys => Keyboard.current;
        static Mouse Pointer => Mouse.current;

        /// <summary>Where to run, x right and y forward, at most length 1.</summary>
        public Vector2 Move { get; private set; }

        /// <summary>How far to turn the view this frame in degrees (x yaw, y pitch up).</summary>
        public Vector2 Look { get; private set; }

        public bool Jump { get; private set; }
        public bool Crouch { get; private set; }
        public bool Fire { get; private set; }
        public bool FirePressed { get; private set; }
        public bool Aim { get; private set; }
        public bool Reload { get; private set; }
        public bool Switch { get; private set; }
        public bool Scoreboard { get; private set; }

        public InputSource Source { get; private set; } = InputSource.KeyboardMouse;

        /// <summary>Sensitivity 1..20 from the settings.</summary>
        public int Sensitivity { get; set; } = 8;
        public bool InvertY { get; set; }

        /// <summary>Slows the view down while aiming down the sights (the zoom ratio).</summary>
        public float LookScale { get; set; } = 1f;

        /// <summary>When false the mouse does not turn the view (menus, or the cursor is free).</summary>
        public bool MouseLook { get; set; }

        public void Read()
        {
            if (lastFrame == Time.frameCount)
            {
                return;
            }
            lastFrame = Time.frameCount;
            Vector2 move = Vector2.zero;
            Vector2 look = Vector2.zero;
            bool jump = false, crouch = false, fire = false, firePressed = false, aim = false;
            bool reload = false, swap = false, board = false;

            Keyboard keys = Keys;
            if (keys != null)
            {
                Vector2 typed = new Vector2(
                    (keys.dKey.isPressed ? 1f : 0f) - (keys.aKey.isPressed ? 1f : 0f),
                    (keys.wKey.isPressed ? 1f : 0f) - (keys.sKey.isPressed ? 1f : 0f));
                bool any = typed != Vector2.zero || keys.spaceKey.wasPressedThisFrame;
                move += typed;
                jump |= keys.spaceKey.wasPressedThisFrame;
                crouch |= keys.leftCtrlKey.isPressed || keys.cKey.isPressed;
                reload |= keys.rKey.wasPressedThisFrame;
                swap |= keys.qKey.wasPressedThisFrame || keys.digit1Key.wasPressedThisFrame || keys.digit2Key.wasPressedThisFrame;
                board |= keys.tabKey.isPressed;
                if (any)
                {
                    Source = InputSource.KeyboardMouse;
                }
            }

            Mouse pointer = Pointer;
            if (pointer != null)
            {
                if (MouseLook)
                {
                    Vector2 delta = pointer.delta.ReadValue();
                    float degrees = 0.012f * Sensitivity * LookScale;
                    look += new Vector2(delta.x, delta.y * (InvertY ? -1f : 1f)) * degrees;
                    swap |= Mathf.Abs(pointer.scroll.ReadValue().y) > 0.1f;
                }
                fire |= pointer.leftButton.isPressed;
                firePressed |= pointer.leftButton.wasPressedThisFrame;
                aim |= pointer.rightButton.isPressed;
                if (pointer.leftButton.wasPressedThisFrame || pointer.delta.ReadValue().sqrMagnitude > 4f)
                {
                    Source = InputSource.KeyboardMouse;
                }
            }

            foreach (Gamepad pad in Gamepad.all)
            {
                Vector2 stick = pad.leftStick.ReadValue();
                if (stick.sqrMagnitude < StickDeadZone * StickDeadZone)
                {
                    stick = Vector2.zero;
                }
                Vector2 right = pad.rightStick.ReadValue();
                if (right.sqrMagnitude < StickDeadZone * StickDeadZone)
                {
                    right = Vector2.zero;
                }
                // A curve gives fine aim near the centre and fast turns at the edge.
                right = right.normalized * Mathf.Pow(right.magnitude, 1.8f);
                float rate = StickTurnPerSensitivity * Sensitivity * 0.5f * LookScale * Time.unscaledDeltaTime;
                look += new Vector2(right.x, right.y * (InvertY ? -1f : 1f)) * rate;
                move += stick;
                bool pressed = pad.buttonSouth.wasPressedThisFrame || pad.rightTrigger.wasPressedThisFrame;
                jump |= pad.buttonSouth.wasPressedThisFrame || pad.leftShoulder.wasPressedThisFrame;
                crouch |= pad.buttonEast.isPressed || pad.rightStickButton.isPressed;
                fire |= pad.rightTrigger.isPressed;
                firePressed |= pad.rightTrigger.wasPressedThisFrame;
                aim |= pad.leftTrigger.isPressed;
                reload |= pad.buttonWest.wasPressedThisFrame;
                swap |= pad.buttonNorth.wasPressedThisFrame || pad.rightShoulder.wasPressedThisFrame;
                board |= pad.selectButton.isPressed;
                if (stick != Vector2.zero || right != Vector2.zero || pressed)
                {
                    Source = InputSource.Gamepad;
                }
            }

            Move = Vector2.ClampMagnitude(move, 1f);
            Look = look;
            Jump = jump;
            Crouch = crouch;
            Fire = fire;
            FirePressed = firePressed;
            Aim = aim;
            Reload = reload;
            Switch = swap;
            Scoreboard = board;
        }

        public bool Pause => (Keys != null && (Keys.escapeKey.wasPressedThisFrame || Keys.pKey.wasPressedThisFrame)) ||
            AnyPad(pad => pad.startButton.wasPressedThisFrame);

        public bool Confirm =>
            (Keys != null && (Keys.enterKey.wasPressedThisFrame || Keys.numpadEnterKey.wasPressedThisFrame || Keys.spaceKey.wasPressedThisFrame)) ||
            AnyPad(pad => pad.buttonSouth.wasPressedThisFrame);

        public bool Back =>
            (Keys != null && (Keys.escapeKey.wasPressedThisFrame || Keys.backspaceKey.wasPressedThisFrame)) ||
            AnyPad(pad => pad.buttonEast.wasPressedThisFrame);

        /// <summary>-1 for a step up a menu, +1 for a step down, 0 otherwise.</summary>
        public int VerticalStep()
        {
            int step = 0;
            if (Keys != null)
            {
                if (Keys.upArrowKey.wasPressedThisFrame || Keys.wKey.wasPressedThisFrame) step = -1;
                if (Keys.downArrowKey.wasPressedThisFrame || Keys.sKey.wasPressedThisFrame) step = 1;
            }
            if (step != 0)
            {
                Source = InputSource.KeyboardMouse;
                return step;
            }
            return PadStep(true, ref stickVertical);
        }

        /// <summary>-1 for a step left through a setting, +1 for a step right, 0 otherwise.</summary>
        public int HorizontalStep()
        {
            int step = 0;
            if (Keys != null)
            {
                if (Keys.leftArrowKey.wasPressedThisFrame || Keys.aKey.wasPressedThisFrame) step = -1;
                if (Keys.rightArrowKey.wasPressedThisFrame || Keys.dKey.wasPressedThisFrame) step = 1;
            }
            if (step != 0)
            {
                Source = InputSource.KeyboardMouse;
                return step;
            }
            return PadStep(false, ref stickHorizontal);
        }

        int PadStep(bool vertical, ref int held)
        {
            int step = 0;
            int stickSign = 0;
            foreach (Gamepad pad in Gamepad.all)
            {
                if (vertical)
                {
                    if (pad.dpad.up.wasPressedThisFrame) step = -1;
                    if (pad.dpad.down.wasPressedThisFrame) step = 1;
                }
                else
                {
                    if (pad.dpad.left.wasPressedThisFrame) step = -1;
                    if (pad.dpad.right.wasPressedThisFrame) step = 1;
                }
                Vector2 stick = pad.leftStick.ReadValue();
                float axis = vertical ? -stick.y : stick.x;
                if (Mathf.Abs(axis) > StickStep)
                {
                    stickSign = axis > 0f ? 1 : -1;
                }
            }
            if (step == 0 && stickSign != 0 && stickSign != held)
            {
                step = stickSign;
            }
            held = stickSign;
            if (step != 0)
            {
                Source = InputSource.Gamepad;
            }
            return step;
        }

        static bool AnyPad(System.Func<Gamepad, bool> test)
        {
            foreach (Gamepad pad in Gamepad.all)
            {
                if (test(pad))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
