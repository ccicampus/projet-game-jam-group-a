using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Minigame : MonoBehaviour
{
    public int totalHits;
    public float timeout;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI timerText;
    public Sprite imageState0;
    public Sprite imageState1;
    public Sprite tutorialImage;
    private float introductionTimeout = 5f;
    public GameObject versusBackground;
    public GameObject versusGrandma;
    public GameObject versusMonster;
    public GameObject background;
    public Animator doorAnimator;
    private float horizontalInput;
    private bool right = true;
    private bool tutorial = true;
    private SpriteRenderer sprite;
    private bool end = false;

    [Header("Audio")]
    [SerializeField] private AudioClip bonkSFX;
    [SerializeField] private AudioClip dieSFX;
    [SerializeField] private AudioClip closeDoorSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Image versusMonsterImage = versusMonster.GetComponent<Image>();
        versusMonsterImage.sprite = VisitorSpawner.Instance.GetCurrentVisitor().GetVisitorData().UnmaskSprite;
        VisitorSpawner.Instance.GetCurrentVisitor().Reveal();

        Image backgroundImage = background.GetComponent<Image>();
        backgroundImage.sprite = tutorialImage;
        hpText.text = "";
        timerText.text = "";
        sprite = GetComponent<SpriteRenderer>();
        doorAnimator.Play("FullyOpen");
        AudioManager.Instance.PlayFightMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (introductionTimeout > 0)
        {
            introductionTimeout -= Time.deltaTime;
        }
        else if (end)
        {
            return;
        }
        else
        {
            disableIntroduction();
            if (tutorial)
            {
                processTutorial();
            }
            else
            {
                processMinigame();
            }
        }
    }

    void disableIntroduction()
    {
        versusBackground.SetActive(false);
        versusGrandma.SetActive(false);
        versusMonster.SetActive(false);
    }

    void updateSprite()
    {
        if (right)
        {
            sprite.sprite = imageState0;
        }
        else
        {
            sprite.sprite = imageState1;
        }
    }

    void processTutorial()
    {
        if (InputManager.Instance.JumpPressed)
        {
            tutorial = false;
            hpText.text = totalHits.ToString();
            background.SetActive(false);
        }
    }

    void processHit()
    {
        horizontalInput = InputManager.Instance.MoveInput.x;
        Visitor visitor = VisitorSpawner.Instance.GetCurrentVisitor();

        if (!right && horizontalInput == 1)
        {
            right = !right;
            visitor.sprite.sprite = visitor.GetVisitorData().UnmaskSprite;
        }
        else if (right && horizontalInput == -1)
        {
            right = !right;
            totalHits -= 1;
            hpText.text = totalHits.ToString();
            visitor.sprite.sprite = visitor.GetVisitorData().BoinkSprite;
            AudioManager.Instance.PlaySFX(bonkSFX);
        }
    }

    void processMinigame()
    {
        timeout -= Time.deltaTime;
        timerText.text = $"{timeout:F2} s";
        updateSprite();

        if (timeout < 0 && totalHits > 0)
        {
            hpText.text = "dead";
            end = true;
            SceneTransitionManager.Instance.LoadScene(0);
        }
        else
        {
            if (totalHits <= 0)
            {
                AudioManager.Instance.PlaySFX(dieSFX);
                hpText.text = "win";
                end = true;
                GameManager.Instance.fight = false;
                VisitorSpawner.Instance.GetCurrentVisitor().Judge(VisitorType.Monster);
                doorAnimator.SetTrigger("CloseDoor");
                // StartCoroutine(PlayCloseDoor());
                AudioManager.Instance.PlaySFX(closeDoorSFX);
                Destroy(VisitorSpawner.Instance.GetCurrentVisitor().gameObject);
                SceneTransitionManager.Instance.LoadScene(1);
                AudioManager.Instance.PlayMusicByName("Gameplay");
                gameObject.SetActive(false);
            }
            else
            {
                processHit();
            }
        }
    }

    // IEnumerator PlayCloseDoor()
    // {
    //     yield return new WaitForSeconds(1.0f);
    //     AudioManager.Instance.PlaySFX(closeDoorSFX);
    // }
}
