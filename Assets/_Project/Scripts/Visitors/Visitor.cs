using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents a masked visitor at the door
/// Manages state and judgment logic only - rendering handled by UI
/// </summary>
public class Visitor : MonoBehaviour
{
    // Singleton
    public static Visitor Instance { get; private set; }


    [Header("Configuration")]
    [SerializeField] private VisitorData visitorData;

    [Header("Debug")]
    [SerializeField] private bool debugMode = false;

    // Events for game flow integration
    public UnityEvent OnVisitorAppear;
    public UnityEvent<VisitorType> OnVisitorRevealed;
    public UnityEvent OnVisitorDismissed;

    // State tracking
    private bool hasBeenJudged = false;
    private bool wasCorrectlyIdentified = false;

    public SpriteRenderer sprite;

    private void Awake()
    {
        // Singleton pattern with proper cleanup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (debugMode)
                Debug.Log("GameManager initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (visitorData == null)
        {
            Debug.LogError("VisitorData is null! Assign in Inspector.");
            return;
        }
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = visitorData.MaskSprite;
    }

    /// <summary>
    /// Called when visitor appears at door
    /// </summary>
    public void Appear()
    {
        if (debugMode)
            Debug.Log($"Visitor '{visitorData.VisitorName}' appearing");

        // Play greeting sound
        if (visitorData.GreetingSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(visitorData.GreetingSound);
        }

        OnVisitorAppear?.Invoke();
    }

    /// <summary>
    /// Judge the visitor
    /// </summary>
    public int Judge(VisitorType playerGuess)
    {
        if (hasBeenJudged)
        {
            Debug.LogWarning("Visitor already judged!");
            return 0;
        }

        hasBeenJudged = true;
        wasCorrectlyIdentified = playerGuess == visitorData.ActualType;

        int scoreAwarded = 0;

        if (wasCorrectlyIdentified)
        {
            scoreAwarded = visitorData.BaseScoreValue;
            if (debugMode)
                Debug.Log($"✓ Correct! {visitorData.VisitorName} is {visitorData.ActualType}. +{scoreAwarded}");
        }
        else
        {
            scoreAwarded = -Mathf.RoundToInt(visitorData.BaseScoreValue * 0.5f);
            if (debugMode)
                Debug.Log($"✗ Wrong! {visitorData.VisitorName} is actually {visitorData.ActualType}. {scoreAwarded}");
        }

        // Award/deduct score
        if (GameManager.Instance != null)
        {
            if (scoreAwarded > 0)
                GameManager.Instance.AddScore(scoreAwarded);
            else if (scoreAwarded < 0)
                GameManager.Instance.ApplyMalus(Mathf.Abs(scoreAwarded));
        }

        return scoreAwarded;
    }

    /// <summary>
    /// Reveal the visitor's true identity
    /// </summary>
    public void Reveal()
    {
        if (debugMode)
            Debug.Log($"Revealing {visitorData.VisitorName} as {visitorData.ActualType}");

        if (sprite)
        {
            sprite.sprite = visitorData.UnmaskSprite;
        }

        // Play reveal sound
        if (visitorData.RevealSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(visitorData.RevealSound);
        }

        OnVisitorRevealed?.Invoke(visitorData.ActualType);

        // Award to collection if correct
        if (wasCorrectlyIdentified && CollectionManager.Instance != null)
        {
            CollectionManager.Instance.CollectCard(visitorData.MaskSprite, visitorData.BaseScoreValue);
        }
    }

    /// <summary>
    /// Dismiss the visitor
    /// </summary>
    public void Dismiss()
    {
        if (debugMode)
            Debug.Log($"Dismissing {visitorData.VisitorName}");

        OnVisitorDismissed?.Invoke();
        Destroy(gameObject, 0.5f);
    }

    /// <summary>
    /// Play boink reaction (optional) - hit during fighting mini-game
    /// </summary>
    public void PlayBoink()
    {
        if (visitorData?.BoinkSprite == null)
            return;

        if (debugMode)
            Debug.Log($"{visitorData.VisitorName} took a hit - BOINK!");

        if (visitorData.BoinkSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(visitorData.BoinkSound);
        }

        // Boink visual effect handled by mini-game or UI
    }

    /// <summary>
    /// Get a random clue for mini-games
    /// </summary>
    public string GetRandomClue()
    {
        if (visitorData.TextClues == null || visitorData.TextClues.Length == 0)
            return "No clues available.";

        return visitorData.TextClues[Random.Range(0, visitorData.TextClues.Length)];
    }

    /// <summary>
    /// Get visitor data
    /// </summary>
    public VisitorData GetVisitorData()
    {
        return visitorData;
    }

    private void OnDestroy()
    {
        OnVisitorAppear?.RemoveAllListeners();
        OnVisitorRevealed?.RemoveAllListeners();
        OnVisitorDismissed?.RemoveAllListeners();

        // Clean up singleton reference
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Properties
    public string VisitorName => visitorData?.VisitorName ?? "Unknown";
    public VisitorType ActualType => visitorData?.ActualType ?? VisitorType.Kid;
    public bool HasBeenJudged => hasBeenJudged;
    public bool WasCorrectlyIdentified => wasCorrectlyIdentified;
}
