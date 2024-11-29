using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel; // Popup panel GameObject
    public Text popupMessage;     // Text component for the message
    public float popupDuration = 2f; // Duration for the popup to remain visible

    private Coroutine hidePopupCoroutine; // Reference to the active coroutine

    void Start()
    {
        // Ensure the popup is inactive at the start of the game
        popupPanel.SetActive(false);
    }

    public void ShowPopup(string message)
    {
        popupMessage.text = message;
        popupPanel.SetActive(true); // Show the popup

        // Stop any previous hide popup coroutine
        if (hidePopupCoroutine != null)
        {
            StopCoroutine(hidePopupCoroutine);
        }

        // Start a new coroutine to hide the popup after a delay
        hidePopupCoroutine = StartCoroutine(HidePopupAfterDelay());
    }

    private IEnumerator HidePopupAfterDelay()
    {
        yield return new WaitForSeconds(popupDuration); // Wait for the duration
        popupPanel.SetActive(false); // Hide the popup
        hidePopupCoroutine = null; // Reset coroutine reference
    }

    public void HidePopup()
    {
        // Manually hide the popup and stop the coroutine if it's running
        if (hidePopupCoroutine != null)
        {
            StopCoroutine(hidePopupCoroutine);
            hidePopupCoroutine = null;
        }
        popupPanel.SetActive(false);
    }
}