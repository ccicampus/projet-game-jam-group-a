using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadGame()
    {
        // Load save data                                                                         
        SaveSystem.Instance.LoadGame();
        SceneManager.LoadScene("Main");
    }

    public void ShowOptions()
    {
        Debug.Log("Options clicked");
    }

    public void Quit()
    {
        Application.Quit();
    }
}