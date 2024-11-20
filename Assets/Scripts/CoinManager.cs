using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;  // Singleton instance
    public int coinCount = 0;  // Tracks the total coin count

    void Awake()
    {
        // Ensure only one instance of CoinManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instance
        }

        // Load the saved coin count from PlayerPrefs
        coinCount = PlayerPrefs.GetInt("CoinCount", 0);
    }

    
    public int GetCoinCount()
    {
        return coinCount;
    }
}
