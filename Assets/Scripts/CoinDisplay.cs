using UnityEngine;
using UnityEngine.UI;
public class CoinDisplay : MonoBehaviour
{
    public Text coinText;  // Reference to the Text UI element

    private void Awake()
    {
        // Ensure the CoinDisplay persists across scene changes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Get the coin count when the game starts (load it from PlayerPrefs)
        int savedCoinCount = PlayerPrefs.GetInt("CoinCount", 0);
        coinText.text = "Coins: " + savedCoinCount.ToString();
    }

    // Update the coin display UI (called by Player script)
    public void UpdateCoinDisplay(int newCoinCount)
    {
        coinText.text = "Coins: " + newCoinCount.ToString();
    }
}
