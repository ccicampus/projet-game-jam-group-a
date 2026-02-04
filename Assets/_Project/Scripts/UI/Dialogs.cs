using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Choices
{
    public string answer;
    public int next;
}

[System.Serializable]
public class Dialogue
{
    public string original;
    public Choices[] choices;
}

[System.Serializable]
public class Dialogues
{
    public Dialogue[] dialogues;
}

public class Dialogs : MonoBehaviour
{
    public GameObject dialogPanel;
    public GameObject portrait;
    public TextMeshProUGUI dialogueTextObject;
    public TextMeshProUGUI optionTextObject1;
    public TextMeshProUGUI optionTextObject2;
    public TextMeshProUGUI optionTextObject3;
    private TextMeshProUGUI[] optionTextList;
    public TextAsset jsonFile;
    private Dialogues dialoguesInJson;
    private int step = -1;
    private bool inTimeout = false;
    private float timeout = 0.1f;
    private float maxTimeout = 0.1f;
    public Sprite grandmaSprite;
    public RuntimeAnimatorController grandmaAnimator;
    private bool answering = false;
    private int answer_choice = 0;
    public bool end = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogPanel.SetActive(false);
        optionTextList = new TextMeshProUGUI[] { optionTextObject1, optionTextObject2, optionTextObject3 };
        dialoguesInJson = JsonUtility.FromJson<Dialogues>(jsonFile.text);
        getNextDialog();
    }

    // Update is called once per frame
    void Update()
    {
        if (inTimeout)
        {
            updateTimeout();
        }
        else
        {
            handleInput();
        }
    }

    void getNextDialog(int chosen = -1)
    {
        if (chosen == -1)
        {
            step += 1;
        }
        else
        {
            step = dialoguesInJson.dialogues[step].choices[chosen].next;
            if (step == -1)
            {
                end = true;
                // dialogPanel.SetActive(false);
                return;
            }
        }
        dialogueTextObject.text = dialoguesInJson.dialogues[step].original;
        int index = 0;
        foreach (Choices choice in dialoguesInJson.dialogues[step].choices)
        {
            optionTextList[index].text = $"{index + 1}. {choice.answer}";
            index += 1;
        }
        if (getNumberOfChoices() == 2)
        {
            optionTextList[2].text = "";
        }
        inTimeout = true;
    }

    void handleInput()
    {
        bool up = InputManager.Instance.JumpPressed;
        bool one = InputManager.Instance.Skill1Pressed;
        bool two = InputManager.Instance.Skill2Pressed;
        bool three = InputManager.Instance.Skill3Pressed;

        if (answering)
        {
            if (up)
            {
                answering = false;
                handleAnswering();
                getNextDialog(answer_choice);
            }
        }
        else
        {
            if (one)
            {
                answering = true;
                answer_choice = 0;
                handleAnswering();
            }
            else if (two)
            {
                answering = true;
                answer_choice = 1;
                handleAnswering();
            }
            else if (three && getNumberOfChoices() == 3)
            {
                answering = true;
                answer_choice = 2;
                handleAnswering();
            }
        }
    }

    void handleAnswering()
    {

        if (answering)
        {
            Image image = portrait.GetComponent<Image>();
            image.sprite = grandmaSprite;
            Animator animator = portrait.GetComponent<Animator>();
            animator.runtimeAnimatorController = grandmaAnimator;
            dialogueTextObject.text = dialoguesInJson.dialogues[step].choices[answer_choice].answer;
            optionTextObject1.text = "";
            optionTextObject2.text = "";
            optionTextObject3.text = "";
        }
        else
        {
            VisitorData visitorData = VisitorSpawner.Instance.GetCurrentVisitor().GetVisitorData();
            Image image = portrait.GetComponent<Image>();
            image.sprite = visitorData.BustSprite;
            Animator animator = portrait.GetComponent<Animator>();
            animator.runtimeAnimatorController = visitorData.BustAnimation;
        }
    }

    void updateTimeout()
    {
        if (timeout < 0)
        {
            inTimeout = false;
            timeout = maxTimeout;
        }
        else
        {
            timeout -= Time.deltaTime;
        }
    }

    int getNumberOfChoices()
    {
        return dialoguesInJson.dialogues[step].choices.GetLength(0);
    }

    public void ResetDialogues()
    {
        step = -1;
        inTimeout = false;
        answering = false;
        answer_choice = 0;
        end = false;
        dialogPanel.SetActive(false);
        optionTextList = new TextMeshProUGUI[] { optionTextObject1, optionTextObject2, optionTextObject3 };
        dialoguesInJson = JsonUtility.FromJson<Dialogues>(jsonFile.text);
        getNextDialog();
    }
}
