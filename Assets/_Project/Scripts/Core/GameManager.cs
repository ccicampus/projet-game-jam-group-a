using UnityEngine;

/// <summary>
/// Central game manager handling game state and core systems
/// Implements singleton pattern and persists across scenes
/// </summary>
public class GameManager : MonoBehaviour
{
    // Constants
    private const int TARGET_FRAMERATE = 60;
    private const float PAUSED_TIMESCALE = 0f;
    private const float NORMAL_TIMESCALE = 1f;

    // Singleton
    public static GameManager Instance { get; private set; }

    // Serialized fields
    [Header("Game State")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int playerScore = 0;

    [Header("Debug")]
    [SerializeField] private bool debugMode = false;

    [Header("Door")]
    public Animator doorAnimator;
    public GameObject dialog;
    public Dialogs dialogScript;

    [Header("Guessing")]
    public float guessing_timer = 3f;
    private bool hasTriggeredDialogThisRound = false;
    private float dialogDelayTimer = 0f;
    private const float DIALOG_DELAY = 2f;
    // Properties for controlled access
    public bool IsPaused { get; private set; }
    public int CurrentLevel => currentLevel;
    public int PlayerScore => playerScore;

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
        InitializeGame();
    }

    void Update()
    {
        if (doorAnimator != null)
        {
            AnimatorStateInfo stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
            float progress = stateInfo.normalizedTime;

            // Debug.Log($"State: {stateInfo.fullPathHash}, Progress: {progress}");

            // When door reaches Fully Open, start the delay timer                                   
            if (stateInfo.IsName("FullyOpen") && !doorAnimator.IsInTransition(0))
            {
                Debug.Log("✓ IN FULLY OPEN STATE");
                if (!hasTriggeredDialogThisRound)
                {
                    dialogDelayTimer += Time.deltaTime;
                    Debug.Log($"Dialog delay: {dialogDelayTimer}/{DIALOG_DELAY}");

                    if (dialogDelayTimer >= DIALOG_DELAY)
                    {
                        Debug.Log(">>> ACTIVATING DIALOG <<<");
                        if (dialog != null)
                            dialog.SetActive(true);
                        else
                            Debug.LogError("Dialog GameObject is NULL!");
                        hasTriggeredDialogThisRound = true;
                    }
                }
            }

            if (stateInfo.IsName("DoorClosing"))
            {
                hasTriggeredDialogThisRound = false;
                dialogDelayTimer = 0f;
            }

            if (dialogScript != null && dialogScript.end)
            {
                if (dialog.activeSelf == true)
                {
                    dialog.SetActive(false);
                }
                guessing_timer -= Time.deltaTime;
                if (guessing_timer < 0)
                {
                    SceneTransitionManager.Instance.LoadScene(2);
                }
            }
        }
    }

    private void InitializeGame()
    {
        // Set target framerate
        Application.targetFrameRate = TARGET_FRAMERATE;

        // Initialize other systems here
        if (debugMode)
            Debug.Log("Game initialized");

        // TEST: Add test cards to collection (remove this later)
        if (CollectionManager.Instance != null)
        {
            CollectionManager.Instance.CollectCard(null, 100);
            CollectionManager.Instance.CollectCard(null, 250);
            CollectionManager.Instance.CollectCard(null, 150);

            if (debugMode)
                Debug.Log("Test cards added to collection");
        }
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = PAUSED_TIMESCALE;

        if (debugMode)
            Debug.Log("Game paused");
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = NORMAL_TIMESCALE;

        if (debugMode)
            Debug.Log("Game resumed");
    }

    public void AddScore(int points)
    {
        if (points < 0)
        {
            Debug.LogWarning("Attempted to add negative score");
            return;
        }

        playerScore += points;

        if (debugMode)
            Debug.Log($"Score added: {points}. Total: {playerScore}");
    }

    public void ApplyMalus(int penalty)
    {
        playerScore -= Mathf.Abs(penalty);
        if (playerScore < 0)
            playerScore = 0;

        if (debugMode)
            Debug.Log($"Malus applied: -{penalty}. Total: {playerScore}");
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0)
        {
            Debug.LogError("Invalid level index: " + levelIndex);
            return;
        }

        currentLevel = levelIndex;
        // Add scene loading logic here

        if (debugMode)
            Debug.Log($"Loading level {levelIndex}");
    }

    public void RestartLevel()
    {
        ResumeGame();
        // Add scene reload logic here

        if (debugMode)
            Debug.Log("Restarting level");
    }

    public void QuitGame()
    {
        if (debugMode)
            Debug.Log("Quitting game");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnDestroy()
    {
        // Clean up singleton reference
        if (Instance == this)
        {
            Instance = null;
            // Ensure timescale is reset
            Time.timeScale = NORMAL_TIMESCALE;
        }
    }
}
