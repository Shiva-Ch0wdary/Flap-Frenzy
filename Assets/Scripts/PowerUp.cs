using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;  // Type of power-up, defined in PowerUpType.cs
    public float duration = 5f;      // Duration for which the power-up is active

    private void OnTriggerEnter2D(Collider2D other)  // Use OnTriggerEnter2D for 2D physics
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.ActivatePowerUp(powerUpType, duration);
                Destroy(gameObject); // Destroy the power-up object after it is collected
            }
        }
    }
}
