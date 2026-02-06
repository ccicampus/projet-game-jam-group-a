using System.Collections;
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
    private bool canShowButton = false;

    private void Start()
    {
        if (clickButton == null)
        {
            Debug.LogError("Click button not assigned!");
            return;
        }

        canShowButton = false;
        lastButtonState = false;
        clickButton.gameObject.SetActive(false);

        clickButton.onClick.AddListener(OnHandleClicked);

        if (debugMode)
            Debug.Log("Door handle initialisé - en attente de la sonnette");
    }

    /// <summary>
    /// Player clicked the door handle
    /// </summary>
    private void OnHandleClicked()
    {
        if (debugMode)
            Debug.Log("Door handle clicked - opening door!");

        // Trigger door open animation
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("OpenDoor");
            GameManager.Instance.openingDoor = true;
            VisitorSpawner.Instance.SpawnNextVisitor();
        }

        if (doorOpenSound)
        {
            StartCoroutine(PlaySoundWithDelay(1.5f, doorOpenSound));
        }
        if (VisitorSpawner.Instance.GetCurrentVisitor())
        {
            Visitor visitor = VisitorSpawner.Instance.GetCurrentVisitor();
            if (visitor.GetVisitorData().GreetingSound)
            {
                StartCoroutine(PlaySoundWithDelay(3f, visitor.GetVisitorData().GreetingSound));
            }
        }

        // Hide button after clicked
        clickButton.gameObject.SetActive(false);
        canShowButton = false;
    }

    private IEnumerator PlaySoundWithDelay(float delay, AudioClip sfx)
    {
        yield return new WaitForSeconds(delay);

        AudioManager.Instance.PlaySFX(sfx);
        // AudioSource source = GetComponent<AudioSource>();
        // if (source != null && doorOpenSound != null)
        // {
        //     source.PlayOneShot(doorOpenSound);
        // }
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

    public void EnableInteraction()
    {
        canShowButton = true;

        clickButton.gameObject.SetActive(true);
        lastButtonState = true;

        if (debugMode)
            Debug.Log("Interaction activée après la sonnette !");
    }

    void Update()
    {
        // Si on n'a pas encore sonné, on ne touche à rien
        if (!canShowButton)
            return;

        if (doorAnimator == null)
            return;

        AnimatorStateInfo stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);

        bool isClosed = stateInfo.IsName("IdleClosed") && !doorAnimator.IsInTransition(0);

        if (isClosed && !clickButton.gameObject.activeSelf)
        {
            clickButton.gameObject.SetActive(true);
        }

        else if (!isClosed && clickButton.gameObject.activeSelf)
        {
            clickButton.gameObject.SetActive(false);
        }
    }
}
