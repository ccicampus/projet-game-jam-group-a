using UnityEngine;
using System.Collections;

public class MainSceneInitializer : MonoBehaviour
{
    public AudioSource levelMusic;
    public AudioSource doorBellSound;
    public float musicTargetVolume = 0.7f;
    public DoorHandle doorHandleScript;

    void Start()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(1.5f);

        if (doorBellSound != null)
            doorBellSound.Play();

        yield return new WaitForSeconds(2.0f);

        if (doorHandleScript != null)
        {
            doorHandleScript.EnableInteraction();
        }

        if (levelMusic != null)
        {
            levelMusic.volume = 0;
            levelMusic.Play();

            float duration = 2.0f;
            float currentTime = 0;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                levelMusic.volume = Mathf.Lerp(0, musicTargetVolume, currentTime / duration);
                yield return null;
            }
            levelMusic.volume = musicTargetVolume;
        }
    }

    IEnumerator NewVisitor()
    {
        yield return new WaitForSeconds(1.5f);

        if (doorBellSound != null)
            doorBellSound.Play();

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
