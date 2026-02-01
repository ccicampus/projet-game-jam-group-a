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

            StartCoroutine(PlaySoundForClosingDoorWithDelay(1.0f));
        }
        Invoke("HideMenu", 0.5f);
    }

    IEnumerator PlaySoundForClosingDoorWithDelay(float delay)
    {
        StartCoroutine(FadeOutMusic(1.5f));

        yield return new WaitForSeconds(delay);

        AudioSource doorSource = doorAnimator.GetComponent<AudioSource>();
        if (doorSource != null)
            doorSource.Play();

        yield return new WaitForSeconds(1.0f);

        SceneTransitionManager.Instance.LoadScene("Main");
    }

    IEnumerator FadeOutMusic(float duration)
    {
        float startVolume = backgroundMusic.volume;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            backgroundMusic.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }
        backgroundMusic.volume = 0;
    }

    void HideMenu()
    {
        titlePanel.SetActive(false);
    }

    public void LoadGame()
    {
        // Load save data                                                                         
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