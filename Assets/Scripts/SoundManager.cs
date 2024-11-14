using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Image soundOnIcon;
    [SerializeField] private Image soundOffIcon;
    [SerializeField] private AudioSource gameplayMusic;  // Add reference to the AudioSource

    private bool muted = false;

    void Start()
    {
        if (!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted", 0);
            Load();
        }
        else
        {
            Load();
        }

        //UpdateButtonIcon();
        AudioListener.pause = muted;  // Pause or unpause all audio based on the mute state
        if (muted)
        {
            gameplayMusic.Pause();  // If muted, pause the gameplay music
        }
        else
        {
            gameplayMusic.Play();   // If not muted, play the gameplay music
        }
        UpdateButtonIcon();
    }

    public void OnButtonPress()
    {
        muted = !muted;

        // Pause or play audio accordingly
        AudioListener.pause = muted;
        if (muted)
        {
            gameplayMusic.Pause();  // Pause the gameplay music if muted
        }
        else
        {
            gameplayMusic.Play();   // Play the gameplay music if unmuted
        }

        Save(); // Save the mute state
        UpdateButtonIcon(); // Update the icon
    }

    private void UpdateButtonIcon()
    {
        if (muted)
        {
            soundOnIcon.enabled = false;
            soundOffIcon.enabled = true;
        }
        else
        {
            soundOnIcon.enabled = true;
            soundOffIcon.enabled = false;
        }
    }

    private void Load()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }
}
