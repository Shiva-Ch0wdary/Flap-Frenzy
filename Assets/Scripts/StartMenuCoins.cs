using UnityEngine;
using UnityEngine.UI;
public class StartMenuCoins : MonoBehaviour
{
    public Text coinTextStartMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // Load the saved coin count from PlayerPrefs
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        // Display the coin count in the Start Menu
        if (coinTextStartMenu != null)
        {
            coinTextStartMenu.text = totalCoins.ToString(); // Display only the numerical value
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
