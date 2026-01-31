using TMPro;
using UnityEngine;

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
    public TextMeshProUGUI dialogueTextObject;
    public TextMeshProUGUI optionTextObject1;
    public TextMeshProUGUI optionTextObject2;
    public TextMeshProUGUI optionTextObject3;
    private TextMeshProUGUI[] optionTextList;
    public TextAsset jsonFile;
    private Dialogues dialoguesInJson;
    private int step = -1;
    private bool inTimeout = false;
    private float timeout = 0.5f;
    private float maxTimeout = 0.5f;
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
        if (dialoguesInJson.dialogues[step].choices.GetLength(0) == 2)
        {
            optionTextList[2].text = "";
        }
        inTimeout = true;
    }

    void handleInput()
    {
        float horizontalInput = InputManager.Instance.MoveInput.x;
        bool up = InputManager.Instance.JumpPressed;

        if (horizontalInput == -1)
        {
            getNextDialog(0);
        }
        else if (up)
        {
            getNextDialog(1);
        }
        else if (horizontalInput == 1)
        {
            getNextDialog(2);
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
}
