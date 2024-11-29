using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public string soundName; // Name of the sound effect to play
    private AudioManager audioManager;

    private void Start()
    {
        // Find the AudioManager in the scene
        audioManager = AudioManager.Instance;

        // If AudioManager is not found, log a warning
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager not found in the scene!");
        }
    }

    public void PlayButtonSound()
    {
        // Play the sound only if AudioManager is available
        if (audioManager != null)
        {
            audioManager.PlaySFX(soundName);
        }
    }
}
