using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private Spawner spawner;
   // [SerializeField] private Text scoreText;
    [SerializeField] private GameObject gameOver;
   // [SerializeField] private Text powerUpTimerText;
    // [SerializeField] private ScoreManager scoreManager;
    // [SerializeField] private ProgessBar progessBar;

    //change pause 
    [SerializeField] private GameObject pauseBoard; // Assign the PauseBoard GameObject in the Inspector
    [SerializeField] private GameObject gameOverDialog;
    [SerializeField] private Transform respawnPoint; // Assign the RespawnPoint GameObject in the Inspector
    [SerializeField] private Button pauseButton;     // Assign the PauseButton in the Inspector
    [SerializeField] private Button respawnButton;   // Assign the RespawnButton in the Inspector
    [SerializeField] private Button restartButton;   // Assign the RestartButton in the Inspector
    private Vector3 savedPosition; // Store the player's position

    //revive button
    [SerializeField] private Button reviveButton; // Add this to the Inspector
    private bool isRevived = false;
    //revive button ends
    //revive button-obstacle delay
    private bool isInvincible = false;
    [SerializeField] private float invincibilityDuration = 25f; // Duration of obstacle delay after revival
    public bool IsInvincible => isInvincible;
    //revive button-obstacle delay ends


    //progress bar
    [SerializeField] private ScoreManager scoreManager;
    //[SerializeField] private ProgressBar progressBar;

    [SerializeField] private Text currentScoreText; // UI Text for current score
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text highScore;    // UI Text for high score

    //character
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private int characterIndex;
    [SerializeField] private static Vector2 lastCheckPointPos=new Vector2(-3,0);
    public GameObject SelectedCharacter;
    //character

    public int score { get; private set; } = 0;

    private void Awake()
    {
        //character
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        Instantiate(playerPrefabs[characterIndex],lastCheckPointPos, Quaternion.identity);
        //character

        if (Instance != null )
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

    }
    public void IncreaseCoinCount()
    {
        player.coinCount++; // Access the coinCount in Player script
        PlayerPrefs.SetInt("CoinCount", player.coinCount); // Save updated coin count
        PlayerPrefs.Save();

        // Update the score in ScoreManager
    if (scoreManager != null)
    {
        scoreManager.RestartGameCoinsNow(); // Call to reflect score updates
    }

        player.UpdateCoinUI(); // Update UI with the new coin count
    }
    public int GetCoinCount()
    {
        return player.coinCount; // Get coin count directly from Player script
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

     private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      if (scoreManager != null)
            {
                scoreManager.gameObject.SetActive(true); // Enable the ScoreManager
            }
    }

    private void Start()
    {
        
          if (reviveButton != null)
        {
            reviveButton.onClick.AddListener(Revive);
            reviveButton.gameObject.SetActive(false); // Initially hidden
        }
        //change pause
        if (pauseBoard != null)
        {
            pauseBoard.SetActive(false); // Ensure the pause board is hidden initially
        }

        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(true); // Ensure the pause button is shown initially
        }

        if (respawnButton != null)
        {
            respawnButton.gameObject.SetActive(false); // Ensure the respawn button is hidden initially
        }

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false); // Ensure the restart button is hidden initially
        }//change pause

        Play();

        //change pause
        // Hook up the pause button event
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause);
        }

        // Hook up the respawn button event

        //commented respawn button condition
        //if (respawnButton != null)
        //{respawnButton.onClick.AddListener(Respawn);}

        // Hook up the restart button event
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }//change pause
    }
   
public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;

        //change pause
        // Save the player's current position before pausing
        if (player != null)
        {
            savedPosition = player.transform.position;
        }

        if (pauseBoard != null)
        {
            pauseBoard.SetActive(true);
        }

        if (respawnButton != null)
        {
            respawnButton.gameObject.SetActive(true);
        }

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(true);
        }

        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(false);
        }//change pause
    }

    public void Play()
    {
        //change pause
        if (gameOverDialog != null)
        {
            gameOverDialog.SetActive(false);
        }

        if (pauseBoard != null)
        {
            pauseBoard.SetActive(false);
        }

        if (respawnButton != null)
        {
            respawnButton.gameObject.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
        }

        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(true);
        }//change pause

        gameOver.SetActive(false);
        //progressbar
        scoreManager.RestartGameCoinsNow();
       // progressBar.ResetProgress();

        Time.timeScale = 1f;
        player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();
        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    public void Revive()
    {
        if (reviveButton != null)
        {
            reviveButton.gameObject.SetActive(false); // Hide the revive button
        }

        isRevived = true; // Mark as revived
        Time.timeScale = 1f; // Resume the game
        player.enabled = true;

        // Reset player to the saved position or checkpoint
        if (player != null)
        {
            player.transform.position = lastCheckPointPos;
            player.direction = Vector3.zero; // Reset movement
        }

        // Hide game-over UI
        if (gameOver != null)
        {
            gameOver.SetActive(false);
        }
        //enable temporary invicibility
        //revive button-obstacle delay
        StartCoroutine(EnableInvincibility());
        //revive button-obstacle delay ends
    }
    //revive button ends

    public void GameOver()
    {
        

        if (!isRevived)
        {
            if (gameOver != null)
            {
                gameOver.SetActive(true);
            }
        //change pause
        /*if (gameOver != null)
        {
            gameOver.SetActive(true);
        }*/

        Time.timeScale = 0f; // Pause the game
        player.enabled = false;

         highScore.gameObject.SetActive(false);

        // Update the score and high score
        if (scoreManager != null)
        {
            scoreManager.EndGameCoinsNow(); // Ensure score is saved
            float currentScore = PlayerPrefs.GetFloat("CurrentScore", 0f);
            string highScoreKey = "HighScore_" + SceneManager.GetActiveScene().name;
            float highScore = PlayerPrefs.GetFloat(highScoreKey, 0f);

            // Update UI elements
            if (currentScoreText != null)
                currentScoreText.text = "Score: " + Mathf.Round(currentScore).ToString();

            if (highScoreText != null)
                highScoreText.text = "High Score: " + Mathf.Round(highScore).ToString();
        }


        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(false);
        }//change pause

         if (reviveButton != null)
            {
                reviveButton.gameObject.SetActive(true);
            }

         scoreManager.EndGameCoinsNow();
        //sound
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySFX("gameover");
        }
        else
        {
             isRevived = false; 
        
        //gameOver.SetActive(true);

        //commented paise() method here
        //Pause();

        
        //progressbar
            EndGame();
        }

    }

    //progressbar
    public void EndGame()
    {
        scoreManager.EndGameCoinsNow(); // Save the score when the game is over
    }


    //revive button
    

    //change pause
    public void Respawn()
    {
        //commented resumegame()
        //ResumeGame();
        if (player != null)
        {
            // Restore the player's saved position
            player.transform.position = savedPosition;

            //added resumegame() here
            ResumeGame();
        }
    }

    public void RestartGame()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);// Reload the current scene
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        player.enabled = true;

        if (pauseBoard != null)
        {
            pauseBoard.SetActive(false);
        }

        if (respawnButton != null)
        {
            respawnButton.gameObject.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
        }

        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(true);
        }


    }

    public void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            ResumeGame();
        }
        else
        {
            Pause();
        }
    }//change pause

    public bool IsGamePlaying()
    {
        return Time.timeScale > 0 && !gameOver.activeSelf;
    }

    public IEnumerator DisplayPowerUpTimer(float duration)
    {
        float timeLeft = duration;

        while (timeLeft > 0)
        {
            //powerUpTimerText.text = $"Powerup: {timeLeft:F1}s";
            yield return new WaitForSeconds(0.1f);
            timeLeft -= 0.1f;
        }

       // powerUpTimerText.text = "";
    }
    

    //character
    public void SelectCharacter(GameObject character)
    {
        SelectedCharacter = character; // Assign selected character
    }
    //character


    //revive button-obstacle delay
    private IEnumerator EnableInvincibility()
    {
        isInvincible = true; // Enable invincibility

        // Optionally, visually indicate invincibility (e.g., make the bird flash)
        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            elapsed += Time.deltaTime;
            // Add visual feedback here if needed, like toggling the sprite renderer
            yield return null;
        }

        isInvincible = false; // Disable invincibility
    }
    //revive button-obstacle delay ends
}