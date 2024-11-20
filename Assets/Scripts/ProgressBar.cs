using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider progressBar;
    public float targetDuration = 120; // Set the time in seconds for the progress bar to fill
    private float elapsedTime = 0f;

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
}
