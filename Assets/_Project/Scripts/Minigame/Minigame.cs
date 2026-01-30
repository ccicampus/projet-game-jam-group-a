using UnityEngine;
using UnityEngine.UI;

public class Minigame : MonoBehaviour
{
    public int totalHits;
    public float timeout;
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
        timeout -= Time.deltaTime;
        if (timeout < 0 && totalHits > 0)
        {
            Debug.Log("dead");
            textObject.text = "dead";
        } else
        {
            if (totalHits <= 0)
            {
                Debug.Log("win");
                textObject.text = "win";
            } else
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
    }
}
