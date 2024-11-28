using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] skins; // List of character skins
    public Button[] lockButtons; // Lock buttons for each character
    public Button selectButton; // Button to select the character
    public int selectedCharacter;

    public int[] unlockCosts; // Coins required to unlock each character
    public GameObject CharacterPrefab;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("CharacterUnlocked0"))
        {
            PlayerPrefs.SetInt("CharacterUnlocked0", 1); // Ensure the first character is always unlocked.
            PlayerPrefs.SetInt("SelectedCharacter", 0); // Default selected character.
            PlayerPrefs.SetInt("TotalCoins", 100); // Set an initial coin balance.
        }
        selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);

        // Initialize skins and lock buttons
        for (int i = 0; i < skins.Length; i++)
        {
            skins[i].SetActive(false);
            lockButtons[i].gameObject.SetActive(false);
        }

        skins[selectedCharacter].SetActive(true);
        UpdateUI();
    }
    public void ChangeNext()
    {
        skins[selectedCharacter].SetActive(false);
        selectedCharacter++;
        if (selectedCharacter == skins.Length)
            selectedCharacter = 0;

        skins[selectedCharacter].SetActive(true);
        UpdateUI();
    }

    public void ChangePrevious()
    {
        skins[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter == -1)
            selectedCharacter = skins.Length - 1;

        skins[selectedCharacter].SetActive(true);
        UpdateUI();
    }
    private void UpdateUI()
    {
        // Update lock buttons
        for (int i = 0; i < lockButtons.Length; i++)
        {
            lockButtons[i].gameObject.SetActive(i == selectedCharacter && !IsUnlocked(i));
        }

        // Update select button
        if (IsUnlocked(selectedCharacter))
        {
            selectButton.interactable = true;
        }
        else
        {
            selectButton.interactable = false;
        }
    }

    public void OnSelectCharacter()
    {
        if (IsUnlocked(selectedCharacter))
        {
            PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
            Debug.Log(skins[selectedCharacter].name + " Selected!");
        }
    }

    public void OnUnlockCharacter()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0); // Get current coin count
        int cost = unlockCosts[selectedCharacter]; // Get unlock cost for the selected character

        // Check if there are enough coins
        if (totalCoins >= cost && !IsUnlocked(selectedCharacter))
        {
            totalCoins -= cost; // Deduct coins
            PlayerPrefs.SetInt("TotalCoins", totalCoins); // Save updated coin count
            PlayerPrefs.SetInt("CharacterUnlocked" + selectedCharacter, 1); // Mark character as unlocked

            Debug.Log(skins[selectedCharacter].name + " unlocked!");

            // Update UI: Destroy the unlock button and enable the select button
            lockButtons[selectedCharacter].gameObject.SetActive(false); // Hide the lock button
            selectButton.interactable = true; // Enable the select button

            // Optionally, update coin display (if in the same scene as StartMenuCoins)
            StartMenuCoins coinManager = FindObjectOfType<StartMenuCoins>();
            if (coinManager != null)
            {
                coinManager.UpdateCoinDisplay();
            }
        }
        else
        {
            Debug.Log("Not enough coins to unlock " + skins[selectedCharacter].name);
        }
    }

    private bool IsUnlocked(int characterIndex)
    {
        return PlayerPrefs.GetInt("CharacterUnlocked" + characterIndex, 0) == 1 || characterIndex == 0; // First character is always unlocked
    }
}
