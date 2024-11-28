using UnityEngine;
using UnityEngine.UI;

public class GameInstructions : MonoBehaviour
{
    public Text instructionText; // Reference to the Text component
    public string[] instructions; // Array of instruction strings
    public GameObject instructionPanel; // Panel to display instructions
    private int currentInstructionIndex = 0;
    private bool hasShownInstructions = false; // Flag to check if instructions were already shown

    void Start()
    {
        if (instructions.Length > 0)
        {
            ShowInstruction(); // Show the first instruction
        }
        else
        {
            Debug.LogError("No instructions provided!");
        }
    }

    void Update()
    {
        if (instructionPanel.activeSelf && Input.GetMouseButtonDown(0)) // Detect mouse click or tap
        {
            NextInstruction();
        }
    }

    void ShowInstruction()
    {
        if (!hasShownInstructions)
        {
            Time.timeScale = 0; // Pause the game
            instructionText.text = instructions[currentInstructionIndex];
            instructionPanel.SetActive(true);
        }
    }

    void NextInstruction()
    {
        currentInstructionIndex++;

        if (currentInstructionIndex < instructions.Length)
        {
            instructionText.text = instructions[currentInstructionIndex]; // Show the next instruction
        }
        else
        {
            instructionPanel.SetActive(false); // Hide the panel
            Time.timeScale = 1; // Resume the game
            hasShownInstructions = true; // Set flag to true
        }
    }
}
