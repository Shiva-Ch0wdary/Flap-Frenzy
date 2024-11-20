using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreTextNow;
    private float currentScorenNow;
    private bool isGameActiveNow;

    void Start()
    {
        PlayerPrefs.DeleteKey("CurrentScore");
        currentScorenNow = PlayerPrefs.GetFloat("CurrentScore", 0f);
        isGameActiveNow = true;
        UpdateScoreTextNow();
    }

    void Update()
    {
        if (isGameActiveNow)
        {
            currentScorenNow += Time.deltaTime;
            UpdateScoreTextNow();
        }
    }

    public void EndGameCoinsNow()
    {
        isGameActiveNow = false;
        PlayerPrefs.SetFloat("CurrentScore", currentScorenNow);
        PlayerPrefs.Save();
    }

    public void RestartGameCoinsNow()
    {
        isGameActiveNow = true;
        currentScorenNow = 0f;
        UpdateScoreTextNow();
    }

    private void UpdateScoreTextNow()
    {
        scoreTextNow.text = Mathf.Round(currentScorenNow).ToString();
    }
    public void ResetScore()
    {
        currentScorenNow = 0f; // Reset the score to 0
        UpdateScoreTextNow(); // Update the UI with the reset score
    }
}
