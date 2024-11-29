using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CoinsUnlock : MonoBehaviour
{
    public Button cavesUnlockButton; // Unlock button for Caves
    public Button cavesPlayButton;   // Play button for Caves
    public Button winterUnlockButton; // Unlock button for Winter
    public Button winterPlayButton;   // Play button for Winter
    public PopupManager popupManager;
    public int coinsRequiredForCaves =10; // Coins needed to unlock Caves
    public int coinsRequiredForWinter = 5; // Coins needed to unlock Winter

    private string sunnyKey = "HighScore_Day Sunny";
    private string winterKey = "HighScore_Day Winter";
    private string cavesKey = "HighScore_Day Caves";

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Update Caves buttons
        bool isCavesUnlocked = PlayerPrefs.GetInt("CavesUnlocked", 0) == 1;
        cavesUnlockButton.gameObject.SetActive(!isCavesUnlocked);
        cavesPlayButton.gameObject.SetActive(isCavesUnlocked);

        // Update Winter buttons
        bool isWinterUnlocked = PlayerPrefs.GetInt("WinterUnlocked", 0) == 1;
        winterUnlockButton.gameObject.SetActive(!isWinterUnlocked);
        winterPlayButton.gameObject.SetActive(isWinterUnlocked);
    }

    public void UnlockCaves()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        if (totalCoins >= coinsRequiredForCaves)
        {
            PlayerPrefs.SetInt("TotalCoins", totalCoins - coinsRequiredForCaves); // Deduct coins
            PlayerPrefs.SetInt("CavesUnlocked", 1); // Mark Caves as unlocked
            PlayerPrefs.Save(); // Save changes to PlayerPrefs
            Debug.Log("Caves level unlocked!");
            UpdateUI(); // Refresh UI
            popupManager.ShowPopup("Hurray Sucessfully Unlocked This Level!");
        }
        else
        {
            popupManager.ShowPopup("Not Have enough coins to unlock this level!");
        }
    }

    public void ResetLocks()
{
    // Resetting level locks
    PlayerPrefs.SetInt("CavesUnlocked", 0); // Lock Caves
    PlayerPrefs.SetInt("WinterUnlocked", 0); // Lock Winter

    // Resetting character locks
    CharacterSelect characterSelect = FindObjectOfType<CharacterSelect>();
    if (characterSelect != null)
    {
        for (int i = 1; i < characterSelect.skins.Length; i++) // Starting from 1 since the first character is always unlocked
        {
            PlayerPrefs.SetInt("CharacterUnlocked" + i, 0); // Lock all characters except the first
        }

        // Reset selected character to default
        PlayerPrefs.SetInt("SelectedCharacter", 0); // Reset to the first character
        characterSelect.ResetCharacterUI(); // Update character UI
    }
    else
    {
        Debug.LogWarning("CharacterSelect instance not found. Character locks not reset.");
    }

    // Optionally reset total coins (if needed)
    PlayerPrefs.SetInt("TotalCoins", 0); // Reset coin count

    // Save changes
    PlayerPrefs.Save();

    popupManager.ShowPopup("Game locks have been reset!");
    UpdateUI(); // Update level UI
}



    public void UnlockWinter()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        if (totalCoins >= coinsRequiredForWinter)
        {
            PlayerPrefs.SetInt("TotalCoins", totalCoins - coinsRequiredForWinter); // Deduct coins
            PlayerPrefs.SetInt("WinterUnlocked", 1); // Mark Winter as unlocked
            PlayerPrefs.Save(); // Save changes to PlayerPrefs
            Debug.Log("Winter level unlocked!");
            UpdateUI(); // Refresh UI
            popupManager.ShowPopup("Hurray Sucessfully Unlocked This Level!");

        }
        else
        {
            popupManager.ShowPopup("Not enough coins to unlock this level!");
        }
    }

    public void PlayCaves()
    {
        if (PlayerPrefs.GetInt("CavesUnlocked", 0) == 1)
        {
            SceneManager.LoadScene("Day Caves"); // Load Caves scene
        }
        else
        {
            popupManager.ShowPopup("Caves level is locked!");
        }
    }

    public void PlayWinter()
    {
        if (PlayerPrefs.GetInt("WinterUnlocked", 0) == 1)
        {
            SceneManager.LoadScene("Day Winter"); // Load Winter scene
        }
        else
        {
            popupManager.ShowPopup("Winter level is locked!");
        }
    }
    public void ResetAllLocks()
    {
        // Resetting lock states
        PlayerPrefs.SetInt("CavesUnlocked", 0); // Lock Caves
        PlayerPrefs.SetInt("WinterUnlocked", 0); // Lock Winter

        // Optionally reset total coins (if needed)
        PlayerPrefs.SetInt("TotalCoins", 0); // Reset coin count
        PlayerPrefs.SetFloat(sunnyKey, 0f); // Reset Sunny high score
        PlayerPrefs.SetFloat(winterKey, 0f); // Reset Winter high score
        PlayerPrefs.SetFloat(cavesKey, 0f); // Reset Caves high score
        // Save changes
        PlayerPrefs.Save();

        // Assuming you have a reference to StartMenuHighScores
        StartMenuHighScores highScores = FindObjectOfType<StartMenuHighScores>();
        if (highScores != null)
        {
            highScores.UpdateHighScores(); // Update the high score UI
        }
        popupManager.ShowPopup("Game locks have been reset!");
        UpdateUI(); // Update UI to reflect changes
    }

}
