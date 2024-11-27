using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
     [SerializeField] private Text scoreTextNow; // Text for displaying the current score
    [SerializeField] private Text highScoreText; // Text for displaying the high score
    private float currentScoreNow;
    private bool isGameActiveNow;

    void Start()
    {
        // Generate a unique key for the current scene
    string highScoreKey = "HighScore_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

    // Load the saved high score for this scene
    float savedHighScore = PlayerPrefs.GetFloat(highScoreKey, 0f);

    currentScoreNow = 0f; // Initialize the current score
    isGameActiveNow = true;

    UpdateScoreTextNow(); // Update the score UI
    UpdateHighScoreText(savedHighScore); // Update the high score UI
    }

    void Update()
    {
        if (isGameActiveNow)
        {
            currentScoreNow += Time.deltaTime; // Increment score over time
            UpdateScoreTextNow(); // Update the score UI
        }
    }

    public void EndGameCoinsNow()
    {
        isGameActiveNow = false;

    // Generate a unique key for the current scene
    string highScoreKey = "HighScore_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

    // Load the current high score for the scene
    float highScore = PlayerPrefs.GetFloat(highScoreKey, 0f);

    if (currentScoreNow > highScore)
    {
        highScore = currentScoreNow; // Update the high score
        PlayerPrefs.SetFloat(highScoreKey, highScore); // Save the new high score
    }

    PlayerPrefs.SetFloat("CurrentScore", currentScoreNow);
    PlayerPrefs.Save();

    UpdateHighScoreText(highScore); // Update the high score UI
    }

    public void RestartGameCoinsNow()
    {
        isGameActiveNow = true;
        currentScoreNow = 0f; // Reset the current score
        UpdateScoreTextNow(); // Update the score UI
    }

    public void ResetScore()
    {
        currentScoreNow = 0f; // Reset the current score
        UpdateScoreTextNow(); // Update the score UI
    }

    private void UpdateScoreTextNow()
    {
        scoreTextNow.text = Mathf.Round(currentScoreNow).ToString();
    }

    private void UpdateHighScoreText(float highScore)
    {
        highScoreText.text = "High Score: " + Mathf.Round(highScore).ToString();
    }
}
