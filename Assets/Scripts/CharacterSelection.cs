using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characterPrefabs; // List of character prefabs
    public Transform displayPosition;      // Position to display character
    public Button leftButton, rightButton, selectButton;

    private int currentCharacterIndex = 0; // Index of currently selected character
    private GameObject currentCharacter;   // Instantiated character instance

    void Start()
    {
        UpdateCharacterDisplay();
        leftButton.onClick.AddListener(PreviousCharacter);
        rightButton.onClick.AddListener(NextCharacter);
        selectButton.onClick.AddListener(SelectCharacter);
    }

    void UpdateCharacterDisplay()
    {
        // Destroy current character if one exists
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        // Instantiate selected character at display position
        currentCharacter = Instantiate(characterPrefabs[currentCharacterIndex], displayPosition.position, Quaternion.identity);
    }

    void PreviousCharacter()
    {
        currentCharacterIndex = (currentCharacterIndex - 1 + characterPrefabs.Length) % characterPrefabs.Length;
        UpdateCharacterDisplay();
    }

    void NextCharacter()
    {
        currentCharacterIndex = (currentCharacterIndex + 1) % characterPrefabs.Length;
        UpdateCharacterDisplay();
    }

    void SelectCharacter()
    {
        // Save the selected character index
        PlayerPrefs.SetInt("SelectedCharacter", currentCharacterIndex);
        PlayerPrefs.Save();

        // Load the main game scene (replace "GameScene" with the actual scene name)
        SceneManager.LoadScene("GameScene");
    }
}
