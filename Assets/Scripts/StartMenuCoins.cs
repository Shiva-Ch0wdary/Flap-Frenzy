using UnityEngine;
using UnityEngine.UI;
public class StartMenuCoins : MonoBehaviour
{
    public Text coinTextStartMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // Load the saved coin count from PlayerPrefs
        UpdateCoinDisplay();
    }
    // Display the coin count in the Start Menu
    public void UpdateCoinDisplay()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        if (coinTextStartMenu != null)
        {
            coinTextStartMenu.text = totalCoins.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
