using UnityEngine;
using UnityEngine.UI;

public class StartMenuHighScores : MonoBehaviour
{
    [SerializeField] private Text sunnyHighScoreText; // Text for Day Sunny High Score
    [SerializeField] private Text winterHighScoreText; // Text for Day Winter High Score
    [SerializeField] private Text cavesHighScoreText; // Text for Day Caves High Score

    void Start()
    {
        // Define the keys for the high scores of the scenes
        string sunnyKey = "HighScore_Day Sunny";
        string winterKey = "HighScore_Day Winter";
        string cavesKey = "HighScore_Day Caves";

        // Retrieve high scores from PlayerPrefs
        float sunnyHighScore = PlayerPrefs.GetFloat(sunnyKey, 0f);
        float winterHighScore = PlayerPrefs.GetFloat(winterKey, 0f);
        float cavesHighScore = PlayerPrefs.GetFloat(cavesKey, 0f);

        // Update the high score texts (only display the score value)
        sunnyHighScoreText.text = Mathf.Round(sunnyHighScore).ToString();
        winterHighScoreText.text = Mathf.Round(winterHighScore).ToString();
        cavesHighScoreText.text = Mathf.Round(cavesHighScore).ToString();
    }
    public void UpdateHighScores()
    {
        // Define the keys for the high scores of the scenes
        string sunnyKey = "HighScore_Day Sunny";
        string winterKey = "HighScore_Day Winter";
        string cavesKey = "HighScore_Day Caves";

        // Retrieve high scores from PlayerPrefs
        float sunnyHighScore = PlayerPrefs.GetFloat(sunnyKey, 0f);
        float winterHighScore = PlayerPrefs.GetFloat(winterKey, 0f);
        float cavesHighScore = PlayerPrefs.GetFloat(cavesKey, 0f);

        // Update the high score texts (only display the score value)
        sunnyHighScoreText.text = Mathf.Round(sunnyHighScore).ToString();
        winterHighScoreText.text = Mathf.Round(winterHighScore).ToString();
        cavesHighScoreText.text = Mathf.Round(cavesHighScore).ToString();
    }
}
