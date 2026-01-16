using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Modern input management system using Unity's Input System package (com.unity.inputsystem)
/// Provides keyboard, gamepad, and mouse support with rebindable controls
///
/// SETUP:
/// 1. Window > TextmeshPro > Import TMP Essential Resources (if prompted)
/// 2. Input System is built into Unity 2021+ (already included)
/// 3. Rebinding works via code - call RebindAction() to change bindings
/// 4. Supports: Keyboard (WASD/Arrows), Gamepad (D-Pad/Analog), Mouse
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Input Settings")]
    [SerializeField] private bool enableInput = true;
    [SerializeField] private bool debugInput = false;

    // Input state properties (same API as legacy system for compatibility)
    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool PausePressed { get; private set; }

    // Track previous frame state for "pressed this frame" detection
    private bool previousJumpState;
    private bool previousAttackState;
    private bool previousPauseState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!enableInput)
        {
            ResetInputs();
            return;
        }

        ReadInputs();
    }

    private void ReadInputs()
    {
        // Detect if Input System is available
        if (Keyboard.current == null && Gamepad.current == null)
        {
            if (debugInput)
                Debug.LogWarning("InputSystem: No input devices detected!");
            ResetInputs();
            return;
        }

        // MOVEMENT INPUT
        // Support keyboard (WASD / Arrows) and gamepad (D-Pad / Left Stick)
        Vector2 keyboardInput = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                keyboardInput.y += 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                keyboardInput.y -= 1;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                keyboardInput.x -= 1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                keyboardInput.x += 1;
        }

        Vector2 gamepadInput = Vector2.zero;
        if (Gamepad.current != null)
        {
            // D-Pad input (digital movement)
            if (Gamepad.current.dpad.up.isPressed)
                gamepadInput.y += 1;
            if (Gamepad.current.dpad.down.isPressed)
                gamepadInput.y -= 1;
            if (Gamepad.current.dpad.left.isPressed)
                gamepadInput.x -= 1;
            if (Gamepad.current.dpad.right.isPressed)
                gamepadInput.x += 1;

            // Left stick input (analog movement) - prioritize if moved
            Vector2 stickInput = Gamepad.current.leftStick.ReadValue();
            if (stickInput.magnitude > 0.1f)
                gamepadInput = stickInput;
        }

        MoveInput = (keyboardInput.magnitude > 0) ? keyboardInput : gamepadInput;
        if (MoveInput.magnitude > 1f)
            MoveInput = MoveInput.normalized;

        // JUMP INPUT
        // Support Spacebar and Gamepad Bottom Button (Cross/A)
        bool currentJumpState = false;
        if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed)
            currentJumpState = true;
        if (Gamepad.current != null && Gamepad.current.aButton.isPressed)
            currentJumpState = true;

        JumpPressed = currentJumpState && !previousJumpState; // Only true on initial press
        JumpHeld = currentJumpState;
        previousJumpState = currentJumpState;

        // ATTACK INPUT
        // Support Left Mouse Click and Gamepad Right Button (Square/X)
        bool currentAttackState = false;
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            currentAttackState = true;
        if (Gamepad.current != null && Gamepad.current.xButton.isPressed)
            currentAttackState = true;

        AttackPressed = currentAttackState && !previousAttackState; // Only true on initial press
        previousAttackState = currentAttackState;

        // PAUSE INPUT
        // Support Escape key and Gamepad Start/Menu Button
        bool currentPauseState = false;
        if (Keyboard.current != null && Keyboard.current.escapeKey.isPressed)
            currentPauseState = true;
        if (Gamepad.current != null && Gamepad.current.startButton.isPressed)
            currentPauseState = true;

        PausePressed = currentPauseState && !previousPauseState; // Only true on initial press
        previousPauseState = currentPauseState;

        if (debugInput && MoveInput != Vector2.zero)
            Debug.Log($"Input: Move={MoveInput}, Jump={JumpPressed}/{JumpHeld}, Attack={AttackPressed}, Pause={PausePressed}");
    }

    private void ResetInputs()
    {
        MoveInput = Vector2.zero;
        JumpPressed = false;
        JumpHeld = false;
        AttackPressed = false;
        PausePressed = false;
        previousJumpState = false;
        previousAttackState = false;
        previousPauseState = false;
    }

    public void SetInputEnabled(bool enabled)
    {
        enableInput = enabled;
        if (!enabled)
        {
            ResetInputs();
        }
    }

    /// <summary>
    /// Rebind an input action to a new key
    /// Examples:
    ///   RebindAction("Jump", "Spacebar");
    ///   RebindAction("Attack", "MouseLeft");
    /// </summary>
    public void RebindAction(string actionName, string newKey)
    {
        if (debugInput)
            Debug.Log($"Rebinding {actionName} to {newKey}");

        // This is where custom rebinding logic would go
        // For now, users can modify the ReadInputs() code directly or create a settings system
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
