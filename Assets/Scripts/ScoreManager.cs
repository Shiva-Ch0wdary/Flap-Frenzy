using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
     [SerializeField] private Text scoreTextNow; // Text for displaying the current score
    [SerializeField] private Text highScoreText; // Text for displaying the high score
    private float currentScoreNow;
    private bool isGameActiveNow;
    private float scoreAtRevive; // To store score before reviving

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

        // Save current score as the score at revive
        scoreAtRevive = currentScoreNow;

        // Update and save high score
        float highScore = PlayerPrefs.GetFloat(highScoreKey, 0f);
        if (currentScoreNow > highScore)
        {
            highScore = currentScoreNow;
            PlayerPrefs.SetFloat(highScoreKey, highScore);
        }

        PlayerPrefs.SetFloat("CurrentScore", currentScoreNow);
        PlayerPrefs.Save();

        UpdateHighScoreText(highScore);
    }
    public void ContinueGameFromScore(float savedScore)
    {
        currentScoreNow = savedScore; // Resume the score from the saved value
        isGameActiveNow = true; // Resume the game
        UpdateScoreTextNow(); // Update the score UI
    }
    public void RestartGameCoinsNow(bool isReviving = false)
    {

        isGameActiveNow = true;

        if (isReviving)
        {
            currentScoreNow = scoreAtRevive; // Continue from where it stopped
        }
        else
        {
            currentScoreNow = 0f; // Reset score if not reviving
        }

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
