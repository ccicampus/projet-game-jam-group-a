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

    [Header("Audio")]
    [SerializeField] private AudioClip doorOpenSound;

    [Header("Debug")]
    [SerializeField] private bool debugMode = false;

    private bool isDoorOpen = false;

    private void Start()
    {
        if (clickButton == null)
        {
            Debug.LogError("Click button not assigned!");
            return;
        }

        // Hide button initially (wait for door to close)
        clickButton.gameObject.SetActive(false);

        // Hook button click
        clickButton.onClick.AddListener(OnHandleClicked);

        if (debugMode)
            Debug.Log("Door handle initialized - button hidden until door closes");
    }

    /// <summary>
    /// Player clicked the door handle
    /// </summary>
    private void OnHandleClicked()
    {
        if (isDoorOpen)
            return;

        isDoorOpen = true;

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
        isDoorOpen = false;
        clickButton.gameObject.SetActive(true);

        if (debugMode)
            Debug.Log("Door reset - button visible again");
    }

    private void OnDestroy()
    {
        if (clickButton != null)
            clickButton.onClick.RemoveListener(OnHandleClicked);
    }
}
