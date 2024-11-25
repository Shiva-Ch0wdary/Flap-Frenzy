using UnityEngine;
using UnityEngine.UI;
public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel; // Popup panel GameObject
    public Text popupMessage;     // Text component for the message

    void Start()
    {
        // Ensure the popup is inactive at the start of the game
        popupPanel.SetActive(false);
    }

    public void ShowPopup(string message)
    {
        popupMessage.text = message;
        popupPanel.SetActive(true); // Show the popup
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false); // Hide the popup
    }
}
