using UnityEngine;

public class Coin : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the coin moves
    private void Update()
    {
        // Move the coin to the left
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // Destroy the coin if it goes off-screen
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player") && !isCollected)
    //    {
    //        isCollected = true;
    //        Debug.Log("Coin collected!"); // Debug line to check if collision is detected

    //        GameManager.Instance.IncreaseCoinCount();

    //        // Update the coin UI if the player script is attached
    //        Player player = other.GetComponent<Player>();
    //        if (player != null)
    //        {
    //            player.UpdateCoinUI();
    //        }

    //        Destroy(gameObject);
    //    }
    //}
}
