using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingScene : MonoBehaviour
{
    // References to the existing UI elements in the Canvas
    [SerializeField] private Slider loadingSlider; // The loading slider
    [SerializeField] private Text loadingText; // The loading text
    [SerializeField] private GameObject loadingScreen; // The background or container of the loading UI

    private string sceneToLoad;
    private float loadDuration = 3f; // Set the loading duration to 10 seconds
    private float elapsedTime = 0f;  // Time elapsed since the loading started

    private void Start()
    {
        // Make sure the loading screen is active at the start
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // Retrieve the scene name from PlayerPrefs or set the default scene to load
        sceneToLoad = PlayerPrefs.GetString("SceneToLoad", "Scenes/Start Menu");

        // Start the asynchronous loading of the scene
        StartCoroutine(LoadLevelAsync(sceneToLoad));
    }

    private IEnumerator LoadLevelAsync(string levelToLoad)
    {
        // Start loading the scene asynchronously
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        loadOperation.allowSceneActivation = false; // Do not activate the scene immediately

        while (elapsedTime < loadDuration)
        {
            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate progress based on the elapsed time (this will take 10 seconds)
            float progressValue = Mathf.Clamp01(elapsedTime / loadDuration);

            // Update the slider and text based on the progress
            if (loadingSlider != null)
            {
                loadingSlider.value = progressValue;
            }

            if (loadingText != null)
            {
                loadingText.text = $"Loading... {progressValue * 100:F0}%";
            }

            yield return null; // Wait for the next frame
        }

        // Once 10 seconds have passed, set the slider to 100% and update text
        if (loadingSlider != null)
        {
            loadingSlider.value = 1f;
        }

        if (loadingText != null)
        {
            loadingText.text = "Loading Complete!";
        }

        // Wait for a brief moment before activating the scene
        yield return new WaitForSeconds(0.5f);

        loadOperation.allowSceneActivation = true; // Activate the loaded scene
    }

    // This function can be called from other scripts or UI buttons to set the scene
    public static void SetSceneToLoad(string sceneName)
    {
        PlayerPrefs.SetString("SceneToLoad", sceneName);
    }
}

