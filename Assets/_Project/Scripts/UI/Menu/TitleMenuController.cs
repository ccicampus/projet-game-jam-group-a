using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuController : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject optionsPanel;

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

    public void OpenOptions()
    {
        titlePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        titlePanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Le bouton 'quit' a été pressé !");
        Application.Quit();
    }
}