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
    public Image background;
    private float horizontalInput;
    private bool right = true;
    private bool tutorial = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        background.sprite = tutorialImage;
        hpText.text = "";
        timerText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (introductionTimeout > 0)
        {
            introductionTimeout -= Time.deltaTime;
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
            background.sprite = imageState0;
        }
        else
        {
            background.sprite = imageState1;
        }
    }

    void processTutorial()
    {
        if (InputManager.Instance.JumpPressed)
        {
            tutorial = false;
            hpText.text = totalHits.ToString();
        }
    }

    void processHit()
    {
        horizontalInput = InputManager.Instance.MoveInput.x;

        if (right && horizontalInput == 1)
        {
            right = !right;
            totalHits -= 1;
            hpText.text = totalHits.ToString();
        }
        else if (!right && horizontalInput == -1)
        {
            right = !right;
            totalHits -= 1;
            hpText.text = totalHits.ToString();
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
        }
        else
        {
            if (totalHits <= 0)
            {
                hpText.text = "win";
            }
            else
            {
                processHit();
            }
        }
    }
}
