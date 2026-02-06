using UnityEngine;
using System.Collections;

public class MainSceneInitializer : MonoBehaviour
{
    public AudioSource levelMusic;
    public AudioClip doorBellSound;
    public float musicTargetVolume = 0.7f;
    public DoorHandle doorHandleScript;

    void Start()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        // if (levelMusic != null)
        // {
        //     levelMusic.volume = 0;
        //     levelMusic.Play();

        //     float duration = 2.0f;
        //     float currentTime = 0;

        //     while (currentTime < duration)
        //     {
        //         currentTime += Time.deltaTime;
        //         levelMusic.volume = Mathf.Lerp(0, musicTargetVolume, currentTime / duration);
        //         yield return null;
        //     }
        //     levelMusic.volume = musicTargetVolume;
        // }
        
        yield return new WaitForSeconds(1.5f);

        // if (doorBellSound != null)
        //     AudioManager.Instance.PlaySFX(doorBellSound);

        StartCoroutine(AudioManager.Instance.FadeInMusic("Gameplay", 2.0f));

        yield return new WaitForSeconds(2.0f);

        if (doorHandleScript != null)
        {
            doorHandleScript.EnableInteraction();
        }
    }

    IEnumerator NewVisitor()
    {
        yield return new WaitForSeconds(1.5f);

        if (doorBellSound != null)
            AudioManager.Instance.PlaySFX(doorBellSound);

        yield return new WaitForSeconds(2.0f);

        if (doorHandleScript != null)
        {
            doorHandleScript.EnableInteraction();
        }
    }

    public void WaitForVisitor()
    {
        StartCoroutine(NewVisitor());
    }

    void TriggerButton()
    {
        if (doorHandleScript != null)
            doorHandleScript.EnableInteraction();
    }

}
