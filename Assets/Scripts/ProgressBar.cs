using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ProgressBar : MonoBehaviour
{
    public Slider progressBar;
    public float targetDuration; // Remove the default value
    private float elapsedTime = 0f;

    private void Start()
    {
        // Set the targetDuration based on the current scene
        SetTargetDuration();
        ResetProgress(); // Optionally reset the progress when the game starts
    }

    private void Update()
    {
        // Only increase the progress bar if the game is currently playing
        if (GameManager.Instance.IsGamePlaying())
        {
            // Increment the elapsed time by the time passed since the last frame
            elapsedTime += Time.deltaTime;

            // Calculate the progress as a percentage (0 to 1)
            float progress = Mathf.Clamp01(elapsedTime / targetDuration);

            // Update the progress bar value
            progressBar.value = progress;

            // Check if the progress bar is full
            if (progressBar.value >= 1f)
            {
                GameManager.Instance.GameOver();
                Debug.Log("Level Completed!");
            }
        }
    }

    // Method to reset progress bar
    public void ResetProgress()
    {
        elapsedTime = 0f;
        progressBar.value = 0f;
    }

    // Method to set the target duration based on the current scene
    private void SetTargetDuration()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Day Sunny")
        {
            targetDuration = 120f; // 2 minutes
        }
        else if (currentSceneName == "Day Winter")
        {
            targetDuration = 240f; // 4 minutes
        }
        else
        {
            targetDuration = 120f; // Default duration if scene is not recognized
        }
    }
}
