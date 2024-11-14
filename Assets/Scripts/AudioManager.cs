using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private string currentMusicName = "Theme"; // Keeps track of the currently playing music

    void Awake()
    {
        // Ensure there is only one instance of AudioManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps AudioManager when switching scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
        PlayMainMenuMusic(); // Play the main menu music initially
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play the appropriate music when a scene is loaded
        if (scene.name == "Start Menu")
        {
            PlayMainMenuMusic();
        }
        else // Adjust to the actual name of your gameplay scene
        {
            PlayGameplayMusic();
        }
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic("Theme"); // Play the main menu background music
    }

    public void PlayGameplayMusic()
    {
        musicSource.Stop();  // Stop any currently playing music   
        PlayMusic("Levels"); // Play the gameplay background music
    }

    public void PlayMusic(string name)
    {
        // If the requested music is already playing, don't restart it
        if (currentMusicName == name && musicSource.isPlaying)
        {
            return;
        }

        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound not Found");
            return;
        }

        // Update the current music track name
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
