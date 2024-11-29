using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Image soundOnIcon;  // Reference to the "Sound On" icon
    [SerializeField] private Image soundOffIcon; // Reference to the "Sound Off" icon
    [SerializeField] private AudioSource gameplayMusic; // Reference to the gameplay music AudioSource

    private bool isMuted = false;

    private void Start()
    {
        // Load the mute state from PlayerPrefs
        LoadMuteState();

        // Apply the mute state
        ApplyMuteState();

        // Update the button icons
        UpdateButtonIcon();
    }

    public void OnButtonPress()
    {
        // Toggle mute state
        isMuted = !isMuted;

        // Apply the mute state
        ApplyMuteState();

        // Save the mute state
        SaveMuteState();

        // Update the button icons
        UpdateButtonIcon();
    }

    private void ApplyMuteState()
    {
        // Mute or unmute audio
        AudioListener.pause = isMuted;

        // Pause or play the gameplay music
        if (gameplayMusic != null)
        {
            if (isMuted)
            {
                gameplayMusic.Pause();
            }
            else
            {
                gameplayMusic.Play();
            }
        }
    }

    private void UpdateButtonIcon()
    {
        if (soundOnIcon != null && soundOffIcon != null)
        {
            // Set the visibility of the icons based on mute state
            soundOnIcon.enabled = !isMuted;  // Show sound on icon if not muted
            soundOffIcon.enabled = isMuted;  // Show sound off icon if muted
        }
    }

    private void LoadMuteState()
    {
        // Load the mute state from PlayerPrefs (1 = muted, 0 = unmuted)
        isMuted = PlayerPrefs.GetInt("muted", 0) == 1;
    }

    private void SaveMuteState()
    {
        // Save the mute state to PlayerPrefs
        PlayerPrefs.SetInt("muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
}
