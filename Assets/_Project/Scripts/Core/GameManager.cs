using UnityEngine;
using UnityEngine.UI;

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
    public bool openingDoor = false;

    [Header("Game Loop")]
    public VisitorSpawner spawner;
    public int numberOfVisitors = 5;
    public int judged = 0;
    private bool endgame = false;
    public MainSceneInitializer mainSceneInitializer;
    private bool haveReset = false;

    [Header("Guessing")]
    public float guessing_timer = 3f;
    public float max_guessing_timer = 3f;
    private bool hasTriggeredDialogThisRound = false;
    private float dialogDelayTimer = 0f;
    private const float DIALOG_DELAY = 2f;
    // Properties for controlled access
    public bool IsPaused { get; private set; }
    public int CurrentLevel => currentLevel;
    public int PlayerScore => playerScore;
    private Button treatsButton;
    private Button passButton;

    [Header("Fight")]
    public bool fight = false;


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
        guessing_timer = max_guessing_timer;
    }

    void Update()
    {
        // #if UNITY_EDITOR
        findReferences();
        if (judged >= numberOfVisitors && spawner.GetCurrentVisitor() == false)
        {
            endgame = true;
            if (dialog && dialog.activeSelf)
            {
                dialog.SetActive(false);
            }
            if (doorAnimator != null)
            {
                doorAnimator.ResetTrigger("OpenDoor");
            }
            // TRIGGER END SCENE
            HandleEndGame();
        }
        else if (doorAnimator != null && spawner != null && endgame == false)
        {
            AnimatorStateInfo stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);

            if (openingDoor && judged < numberOfVisitors)
            {
                openingDoor = false;
                if (dialogScript != null)
                {
                    Visitor visitor = spawner.GetCurrentVisitor();
                    Image portraitSprite = dialogScript.portrait.GetComponent<Image>();
                    portraitSprite.sprite = visitor.GetVisitorData().BustSprite;
                    Animator portraitAnimator = dialogScript.portrait.GetComponent<Animator>();
                    portraitAnimator.runtimeAnimatorController = visitor.GetVisitorData().BustAnimation;
                    dialogScript.jsonFile = visitor.GetVisitorData().DialoguesJson;
                    dialogScript.ResetDialogues();
                }
            }


            // When door reaches Fully Open, start the delay timer                                   
            if (stateInfo.IsName("FullyOpen") && !doorAnimator.IsInTransition(0))
            {
                haveReset = false;
                if (!hasTriggeredDialogThisRound)
                {
                    dialogDelayTimer += Time.deltaTime;

                    if (dialogDelayTimer >= DIALOG_DELAY)
                    {
                        if (dialog != null)
                            dialog.SetActive(true);
                        else
                            hasTriggeredDialogThisRound = true;
                    }
                }
            }

            if (stateInfo.IsName("IdleClosed") && !haveReset && !fight)
            {
                ResetManager();
            }

            ActivateButtons();
            if (dialogScript != null && dialogScript.end)
            {
                HandleEndOfEncounter();
            }
        }
        // #endif
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

    void HandleEndOfEncounter()
    {
        if (dialog.activeSelf == true)
        {
            dialog.SetActive(false);
        }

        Visitor visitor = VisitorSpawner.Instance.GetCurrentVisitor();
        guessing_timer -= Time.deltaTime;
        if ((guessing_timer < 0 && visitor.GetVisitorData().ActualType == VisitorType.Monster) || fight)
        {
            SceneTransitionManager.Instance.LoadScene(2);
        }
        else if (guessing_timer < 0 && visitor.GetVisitorData().ActualType == VisitorType.Kid)
        {
            if (visitor.HasBeenJudged == false)
            {
                visitor.Judge(VisitorType.Monster);
                doorAnimator.SetTrigger("CloseDoor");
            }
        }
    }

    void HandleEndGame()
    {
        if (doorAnimator != null)
        {
            AnimatorStateInfo stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("IdleClosed") && !doorAnimator.IsInTransition(0))
            {
                SceneTransitionManager.Instance.LoadScene(3);
            }
        }
    }

    void ActivateButtons()
    {
        if (treatsButton && passButton)
        {
            if (dialogScript.end && VisitorSpawner.Instance.GetCurrentVisitor().HasBeenJudged == false)
            {
                if (treatsButton.gameObject.activeSelf == false || passButton.gameObject.activeSelf == false)
                {
                    treatsButton.gameObject.SetActive(true);
                    passButton.gameObject.SetActive(true);
                }
            }
            else
            {
                if (treatsButton.gameObject.activeSelf == true || passButton.gameObject.activeSelf == true)
                {
                    treatsButton.gameObject.SetActive(false);
                    passButton.gameObject.SetActive(false);
                }
            }
        }
    }

    void ClickTreats()
    {
        if (dialogScript.end && guessing_timer > 0)
        {
            Visitor visitor = VisitorSpawner.Instance.GetCurrentVisitor();
            if (visitor.HasBeenJudged == false)
            {
                visitor.Judge(VisitorType.Kid);
                doorAnimator.SetTrigger("CloseDoor");
            }
        }
    }

    void ClickPass()
    {
        if (dialogScript.end && guessing_timer > 0)
        {
            Visitor visitor = VisitorSpawner.Instance.GetCurrentVisitor();
            if (visitor.HasBeenJudged == false)
            {
                visitor.Judge(VisitorType.Monster);
                if (visitor.GetVisitorData().ActualType == VisitorType.Monster)
                {
                    fight = true;
                }
                else
                {
                    doorAnimator.SetTrigger("CloseDoor");
                }
            }
        }
    }

    public void ResetManager()
    {
        haveReset = true;
        fight = false;
        guessing_timer = max_guessing_timer;
        hasTriggeredDialogThisRound = false;
        dialogDelayTimer = 0f;
        if (mainSceneInitializer)
        {
            mainSceneInitializer.WaitForVisitor();
        }
        if (VisitorSpawner.Instance.GetCurrentVisitor())
        {
            Destroy(VisitorSpawner.Instance.GetCurrentVisitor().gameObject);
        }
    }

    private void findReferences()
    {
        GameObject references = GameObject.Find("References");
        if (references)
        {
            References refs = references.GetComponent<References>();
            if (doorAnimator == null)
            {
                if (refs.doorAnimator)
                {
                    doorAnimator = refs.doorAnimator;
                }
            }

            if (dialog == null)
            {
                if (refs.dialog)
                {
                    dialog = refs.dialog;
                }
            }

            if (dialogScript == null)
            {
                if (refs.dialogScript)
                {
                    dialogScript = refs.dialogScript;
                }
            }

            if (treatsButton == null)
            {
                if (refs.treatsButton)
                {
                    treatsButton = refs.treatsButton;
                    treatsButton.onClick.AddListener(ClickTreats);
                }
            }

            if (passButton == null)
            {
                if (refs.passButton)
                {
                    passButton = refs.passButton;
                    passButton.onClick.AddListener(ClickPass);
                }
            }

            if (mainSceneInitializer == null)
            {
                if (refs.mainSceneInitializer)
                {
                    mainSceneInitializer = refs.mainSceneInitializer;
                }
            }
        }
        if (spawner == null)
        {
            spawner = VisitorSpawner.Instance;
        }
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
