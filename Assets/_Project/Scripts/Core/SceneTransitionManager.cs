using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Handles scene loading and transitions with optional fade effects
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private bool useLoadingScreen = false;

    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (!isTransitioning)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }
    }

    public void LoadScene(int sceneIndex)
    {
        if (!isTransitioning)
        {
            StartCoroutine(LoadSceneCoroutine(sceneIndex));
        }
    }

    public void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        isTransitioning = true;

        // Fade out or show loading screen here
        yield return new WaitForSeconds(transitionDuration);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        if (asyncLoad == null)
        {
            Debug.LogError($"Failed to load scene: {sceneName}. Scene does not exist.");
            isTransitioning = false;
            yield break;
        }

        while (!asyncLoad.isDone)
        {
            // if (asyncLoad.failed)
            // {
            //     Debug.LogError($"Failed to load scene: {sceneName}");
            //     isTransitioning = false;
            //     yield break;
            // }

            // Update loading bar if needed
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            yield return null;
        }

        // Fade in
        yield return new WaitForSeconds(transitionDuration);

        isTransitioning = false;
    }

    private IEnumerator LoadSceneCoroutine(int sceneIndex)
    {
        isTransitioning = true;

        yield return new WaitForSeconds(transitionDuration);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        if (asyncLoad == null)
        {
            Debug.LogError($"Failed to load scene at index: {sceneIndex}. Scene does not exist.");
            isTransitioning = false;
            yield break;
        }

        while (!asyncLoad.isDone)
        {
            // if (asyncLoad.failed)
            // {
            //     Debug.LogError($"Failed to load scene at index: {sceneIndex}");
            //     isTransitioning = false;
            //     yield break;
            // }

            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            yield return null;
        }

        yield return new WaitForSeconds(transitionDuration);

        isTransitioning = false;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            StopAllCoroutines();
        }
    }
}
