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
    public Text coinsNeededText;
    //public PopupManager popupManager; // Reference to PopupManager



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

    public void ResetCharacterUI()
{
    // Reset character states
    selectedCharacter = 0; // Default to the first character
    PlayerPrefs.SetInt("SelectedCharacter", 0); // Save the default selection

    for (int i = 0; i < skins.Length; i++)
    {
        skins[i].SetActive(false);
        lockButtons[i].gameObject.SetActive(i != 0); // Show lock buttons for all except the first character
    }

    skins[selectedCharacter].SetActive(true); // Activate the first character skin
    UpdateUI(); // Refresh UI
}


    private void UpdateUI()
{
    // Update lock buttons and coins needed text
    for (int i = 0; i < lockButtons.Length; i++)
    {
        lockButtons[i].gameObject.SetActive(i == selectedCharacter && !IsUnlocked(i));
    }

    // Update select button interactability
    if (IsUnlocked(selectedCharacter))
    {
        selectButton.interactable = true;
        coinsNeededText.gameObject.SetActive(false); // Hide the coins needed text when unlocked
    }
    else
    {
        selectButton.interactable = false;
        int cost = unlockCosts[selectedCharacter];
        coinsNeededText.gameObject.SetActive(true); // Show the coins needed text when locked
        coinsNeededText.text = "Coins Needed: " + cost.ToString(); // Display the required coins
    }
}



    public void OnSelectCharacter()
{
    int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
    int cost = unlockCosts[selectedCharacter];

    if (IsUnlocked(selectedCharacter))
    {
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        Debug.Log(skins[selectedCharacter].name + " Selected!");
    }
    else if (totalCoins >= cost)
    {
        // Deduct coins and unlock character
        totalCoins -= cost;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.SetInt("CharacterUnlocked" + selectedCharacter, 1);
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        Debug.Log(skins[selectedCharacter].name + " Unlocked and Selected!");

        // Update coin display if needed
        StartMenuCoins coinManager = FindObjectOfType<StartMenuCoins>();
        if (coinManager != null)
        {
            coinManager.UpdateCoinDisplay();
        }

        // Refresh UI
        UpdateUI();
    }
    else
    {
        Debug.Log("Not have enough coins to unlock ");
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

        // Update UI: Hide lock button and enable the select button
        lockButtons[selectedCharacter].gameObject.SetActive(false); // Hide the lock button
        selectButton.interactable = true; // Enable the select button

        // Update the coinsNeededText to show "Character Unlocked!" message
        coinsNeededText.text = " Character Unlocked!";
        coinsNeededText.gameObject.SetActive(true); // Make sure it's visible

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
        coinsNeededText.text = "Not enough coins to unlock " + skins[selectedCharacter].name;
        coinsNeededText.gameObject.SetActive(true); // Show the "Not enough coins" message
    }
}



    private bool IsUnlocked(int characterIndex)
    {
        return PlayerPrefs.GetInt("CharacterUnlocked" + characterIndex, 0) == 1 || characterIndex == 0; // First character is always unlocked
    }
}