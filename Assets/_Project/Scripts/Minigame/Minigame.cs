using UnityEngine;
using UnityEngine.UI;

public class Minigame : MonoBehaviour
{
    public int totalHits;
    public Text textObject;
    private float horizontalInput;
    private bool right = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textObject.text = totalHits.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = InputManager.Instance.MoveInput.x;

        if (right && horizontalInput == 1)
        {
            right = !right;
            totalHits -= 1;
            Debug.Log(totalHits);
            textObject.text = totalHits.ToString();
        } else if (!right && horizontalInput == -1)
        {
            right = !right;
            totalHits -= 1;
            Debug.Log(totalHits);
            textObject.text = totalHits.ToString();
        }


        
    }
}
