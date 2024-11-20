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
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private Text powerUpTimerText;
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

    //progress bar
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private ProgressBar progressBar;


    private void Awake()
    {
        if (Instance != null )
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    public void IncreaseCoinCount()
    {
        player.coinCount++; // Access the coinCount in Player script
        PlayerPrefs.SetInt("CoinCount", player.coinCount); // Save updated coin count
        PlayerPrefs.Save();
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
        // Check if the loaded scene is "Day Caves"
        if (scene.name == "Day Caves")
        {
            // Disable the progress bar for the "DayCaves" scene
            if (progressBar != null)
            {
                progressBar.gameObject.SetActive(false); // Disable the progress bar
            }
        }
        else
        {
            // Ensure the progress bar is active for other scenes
            if (progressBar != null)
            {
                progressBar.gameObject.SetActive(true); // Enable the progress bar
            }
        }

        // Disable the ScoreManager in "DaySunny" and "DayWinter" scenes
        if (scene.name == "Day Sunny" || scene.name == "Day Winter")
        {
            if (scoreManager != null)
            {
                scoreManager.gameObject.SetActive(false); // Disable the ScoreManager
            }
        }
        else
        {
            // Enable the ScoreManager in other scenes
            if (scoreManager != null)
            {
                scoreManager.gameObject.SetActive(true); // Enable the ScoreManager
            }
        }
  
    }

    private void Start()
    {
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
        if (respawnButton != null)
        {
            respawnButton.onClick.AddListener(Respawn);
        }

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
        progressBar.ResetProgress();

        Time.timeScale = 1f;
        player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();
        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    public void GameOver()
    {
      
        //change pause
        if (gameOver != null)
        {
            gameOver.SetActive(true);
        }

        Time.timeScale = 0f; // Pause the game
        player.enabled = false;

        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(false);
        }//change pause

        //sound
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySFX("gameover");

        gameOver.SetActive(true);
        Pause();
        //progressbar
        EndGame();

    }

    //progressbar
    public void EndGame()
    {
        scoreManager.EndGameCoinsNow(); // Save the score when the game is over
    }


    //change pause
    public void Respawn()
    {
        ResumeGame();
        if (player != null)
        {
            // Restore the player's saved position
            player.transform.position = savedPosition;
        }
    }

    public void RestartGame()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
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
            powerUpTimerText.text = $"Powerup: {timeLeft:F1}s";
            yield return new WaitForSeconds(0.1f);
            timeLeft -= 0.1f;
        }

        powerUpTimerText.text = "";
    }
    
}