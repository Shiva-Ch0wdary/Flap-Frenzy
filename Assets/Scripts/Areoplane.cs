using UnityEngine;

public class Areoplane : MonoBehaviour
{
    public float speed = 5f;                // Speed of the airplane
    public float verticalAmplitude = 0.5f;   // Amplitude for vertical movement
    public float lifespan = 5f;              // How long the airplane should exist
    public float minHeight = -1f;            // Minimum height for the airplane
    public float maxHeight = 2f;             // Maximum height for the airplane
    public float verticalSpeed = 2f;         // Speed of vertical movement (up and down)

    private float initialYPosition;          // To store the initial vertical position at spawn



    private void Start()
    {
        // Destroy the airplane after its lifespan expires
        Destroy(gameObject, lifespan);

        // Store the initial vertical position at the time of spawn
        initialYPosition = transform.position.y;
    }

    private void Update()
    {
        // Move the airplane horizontally (leftward)
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // Calculate the vertical position based on a sine function
        // The sine wave naturally oscillates between -1 and 1
        float sineValue = Mathf.Sin(Time.time * verticalSpeed) * verticalAmplitude;

        // Calculate the new Y position based on the sine wave, clamping between minHeight and maxHeight
        float newYPosition = Mathf.Lerp(minHeight, maxHeight, (sineValue + 1) / 2); // Convert the sine value (-1 to 1) to a value between minHeight and maxHeight

        // Update the airplane's position
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check for collision with other objects
        if (other.CompareTag("Player"))
        {
            // Handle interaction with the player
            Debug.Log("Player collided with the airplane!");
            Destroy(gameObject); // Destroy the airplane on collision with the player
        }
        else if (other.CompareTag("Obstacle"))
        {
            // Handle interaction with obstacles
            Debug.Log("Airplane hit an obstacle!");
            Destroy(gameObject); // Destroy the airplane on collision with obstacles
        }
    }
}
