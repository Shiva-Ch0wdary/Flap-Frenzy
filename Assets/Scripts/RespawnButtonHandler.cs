using UnityEngine;

public class RespawnButtonHandler : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    // This will be called from the Button's OnClick event in the Inspector
    public void OnRespawnClick()
    {
        if (gameManager != null)
        {
            gameManager.Respawn();
        }
    }
}