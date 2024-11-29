using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private string currentMusicName = ""; // Keeps track of the currently playing music

    private void Awake()
    {
        // Singleton pattern to ensure only one AudioManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Play the start menu music when the game starts
        PlayMainMenuMusic();

        // Subscribe to sceneLoaded event to handle music changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check the scene name and play the appropriate music
        if (scene.name == "Start Menu")
        {
            PlayMainMenuMusic();
        }
        else if (scene.name == "Gameplay") // Replace with your gameplay scene's name
        {
            PlayGameplayMusic();
        }
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic("Theme"); // Ensure "Theme" is the correct name for your main menu music
    }

    public void PlayGameplayMusic()
    {
        PlayMusic("Levels"); // Ensure "Levels" is the correct name for your gameplay music
    }

    public void PlayMusic(string name)
    {
        // If the requested music is already playing, do nothing
        if (currentMusicName == name && musicSource.isPlaying)
        {
            return;
        }

        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music not found: " + name);
            return;
        }

        currentMusicName = name;
        musicSource.clip = s.clip;
        musicSource.loop = true; // Ensure the music loops
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
            currentMusicName = "";
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning("SFX not found: " + name);
            return;
        }

        sfxSource.PlayOneShot(s.clip);
    }
}
