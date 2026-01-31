using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleMenuController : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject optionsPanel;
    public GameObject optionsButton;
    public GameObject collectionPanel;
    public GameObject collectionButton;
    public AudioSource backgroundMusic;

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

        GameObject bckToTitleBtn = optionsPanel.transform.Find("BackToTitleButton").gameObject;

        if (bckToTitleBtn != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(bckToTitleBtn);
        }
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        titlePanel.SetActive(true);

        if (optionsButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(optionsButton);
        }
    }

    public void OpenCollection()
    {
        titlePanel.SetActive(false);
        collectionPanel.SetActive(true);

        GameObject bckToTitleBtn = optionsPanel.transform.Find("BackToTitleButton").gameObject;

        if (bckToTitleBtn != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(bckToTitleBtn);
        }
    }

    public void CloseCollection()
    {
        collectionPanel.SetActive(false);
        titlePanel.SetActive(true);

        if (collectionButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(collectionButton);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Le bouton 'quit' a été pressé !");
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        backgroundMusic.volume = volume;
    }
}