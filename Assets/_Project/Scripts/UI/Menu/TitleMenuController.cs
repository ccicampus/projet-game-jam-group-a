using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class TitleMenuController : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public Animator doorAnimator;
    public GameObject titlePanel;
    public GameObject optionsPanel;
    public GameObject optionsButton;
    public GameObject collectionPanel;
    public GameObject collectionButton;

    public void PlayGame()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("CloseDoor");

            StartCoroutine(PlayMenuSoundsSequence(1.0f));
        }

        IEnumerator PlayMenuSoundsSequence(float delay)
        {
            yield return new WaitForSeconds(delay);

            AudioSource[] sources = doorAnimator.GetComponents<AudioSource>();

            if (sources.Length >= 2)
            {
                yield return new WaitForSeconds(0.2f);
                sources[0].Play();

                yield return new WaitForSeconds(3.0f);
                sources[1].Play();
            }
        }

        Invoke("HideMenu", 0.1f);
    }
    void HideMenu() { titlePanel.SetActive(false); }

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