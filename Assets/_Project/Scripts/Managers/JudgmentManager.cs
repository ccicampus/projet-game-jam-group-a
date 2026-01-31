using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the judgment phase and game flow
/// Coordinates between minigames, visitor, and UI
/// </summary>
public class JudgmentManager : MonoBehaviour
{
    public static JudgmentManager Instance { get; private set; }

    [Header("Timer Settings")]
    [SerializeField] private float judgmentTimeLimit = 10f;

    [Header("Events")]
    public UnityEvent OnJudgmentPhaseStart;
    public UnityEvent<float> OnJudgmentTimerUpdate; // Sends remaining time
    public UnityEvent<bool> OnJudgmentComplete; // True if correct, false if wrong

    [Header("Debug")]
    [SerializeField] private bool debugMode = false;

    private float remainingTime;
    private bool isJudgmentActive = false;
    private Visitor currentVisitor;

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

    /// <summary>
    /// Start the judgment phase for current visitor
    /// </summary>
    public void StartJudgmentPhase()
    {
        currentVisitor = VisitorSpawner.Instance?.GetCurrentVisitor();

        if (currentVisitor == null)
        {
            Debug.LogError("No current visitor to judge!");
            return;
        }

        isJudgmentActive = true;
        remainingTime = judgmentTimeLimit;

        if (debugMode)
            Debug.Log($"Judgment phase started for {currentVisitor.VisitorName}. Time limit: {judgmentTimeLimit}s");

        OnJudgmentPhaseStart?.Invoke();
    }

    private void Update()
    {
        if (isJudgmentActive)
        {
            remainingTime -= Time.deltaTime;
            OnJudgmentTimerUpdate?.Invoke(remainingTime);

            if (remainingTime <= 0)
            {
                // Time's up - force a random judgment or treat as wrong
                HandleTimeout();
            }
        }
    }

    /// <summary>
    /// Player makes a judgment decision
    /// </summary>
    public void MakeJudgment(VisitorType playerGuess)
    {
        if (!isJudgmentActive)
        {
            Debug.LogWarning("Judgment phase is not active!");
            return;
        }

        if (currentVisitor == null)
        {
            Debug.LogError("No visitor to judge!");
            return;
        }

        isJudgmentActive = false;

        // Judge the visitor
        int scoreAwarded = currentVisitor.Judge(playerGuess);
        bool wasCorrect = currentVisitor.WasCorrectlyIdentified;

        if (debugMode)
            Debug.Log($"Player guessed: {playerGuess}. Correct: {wasCorrect}. Score: {scoreAwarded}");

        // Reveal the visitor's true identity
        currentVisitor.Reveal();

        OnJudgmentComplete?.Invoke(wasCorrect);

        // Start dismissal sequence after delay
        Invoke(nameof(DismissCurrentVisitor), 2f);
    }

    private void HandleTimeout()
    {
        if (debugMode)
            Debug.Log("Judgment timeout! Treating as incorrect.");

        isJudgmentActive = false;

        // Treat timeout as wrong judgment (no points awarded)
        currentVisitor.Reveal();
        OnJudgmentComplete?.Invoke(false);

        Invoke(nameof(DismissCurrentVisitor), 2f);
    }

    private void DismissCurrentVisitor()
    {
        if (currentVisitor != null)
        {
            currentVisitor.Dismiss();
            currentVisitor = null;
        }

        // Trigger next visitor spawn after short delay
        Invoke(nameof(SpawnNextVisitor), 2f);
    }

    private void SpawnNextVisitor()
    {
        if (VisitorSpawner.Instance != null)
        {
            VisitorSpawner.Instance.SpawnNextVisitor();
        }
    }

    public bool IsJudgmentActive => isJudgmentActive;
    public float RemainingTime => remainingTime;

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }

        OnJudgmentPhaseStart?.RemoveAllListeners();
        OnJudgmentTimerUpdate?.RemoveAllListeners();
        OnJudgmentComplete?.RemoveAllListeners();
    }
}
