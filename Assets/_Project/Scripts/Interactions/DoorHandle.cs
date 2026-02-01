using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Door handle interaction - player clicks to open door
/// Shows button when door is closed
/// </summary>
public class DoorHandle : MonoBehaviour
{
    [Header("Door Animation")]
    [SerializeField] private Animator doorAnimator;

    [Header("UI Button")]
    [SerializeField] private Button clickButton;

    [Header("Visual Effects")]
    [SerializeField] private Sprite rippleCircle;
    [SerializeField] private float rippleDuration = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip doorOpenSound;

    [Header("Debug")]
    [SerializeField] private bool debugMode = false;

    private bool lastButtonState = false;


    private void Start()
    {
        if (clickButton == null)
        {
            Debug.LogError("Click button not assigned!");
            return;
        }

        // Show button initially (door is closed)
        clickButton.gameObject.SetActive(true);

        // Hook button click
        clickButton.onClick.AddListener(OnHandleClicked);

        if (debugMode)
            Debug.Log("Door handle ready - button visible");
    }

    /// <summary>
    /// Player clicked the door handle
    /// </summary>
    private void OnHandleClicked()
    {
        if (debugMode)
            Debug.Log("Door handle clicked - opening door!");

        // Play door open sound (if available)
        if (doorOpenSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(doorOpenSound);
        }

        // Trigger door open animation
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("OpenDoor");
        }

        // Hide button after clicked
        clickButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Reset door - call this when door closes again (for next visitor)
    /// </summary>
    public void ResetDoor()
    {
        clickButton.gameObject.SetActive(true);

        if (debugMode)
            Debug.Log("Door reset - button visible again");
    }

    private void OnDestroy()
    {
        if (clickButton != null)
            clickButton.onClick.RemoveListener(OnHandleClicked);
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);

        bool shouldShowButton = stateInfo.IsName("IdleClosed") && !doorAnimator.IsInTransition(0);

        if (shouldShowButton && !lastButtonState)
        {
            // Button just became visible - play ripple effect
            clickButton.gameObject.SetActive(true);
            if (rippleCircle != null)
            {
                RippleEffect.PlayRipple(clickButton.transform.position, rippleCircle, rippleDuration);
            }
            lastButtonState = true;
        }
        else if (!shouldShowButton && lastButtonState)
        {
            // Button just became hidden
            clickButton.gameObject.SetActive(false);
            lastButtonState = false;
        }
    }
}
