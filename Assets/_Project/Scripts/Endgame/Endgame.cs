using GLTFast.Schema;
using UnityEngine;

public class Endgame : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    private float timeout = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeout -= Time.deltaTime;
        if (timeout < 0)
        {
            canvas.SetActive(true);
        }   
    }

    public void GetToTitleMenu()
    {
        GameManager.Instance.HardResetManager();
        SceneTransitionManager.Instance.LoadScene(0);
    }

}
