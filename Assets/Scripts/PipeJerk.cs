using UnityEngine;

public class PipeJerk : MonoBehaviour
{
    public float verticalAmplitude = 0.5f;    // Amplitude of the oscillation (how far the pipe moves)
    public float verticalSpeed = 2f;          // Speed of oscillation (how fast it moves up/down)
    public float minHeight = -1f;             // Minimum height for oscillation
    public float maxHeight = 2f;              // Maximum height for oscillation

    private float initialYPosition;           // Store the initial Y position of the pipe

    void Start()
    {
        // Store the original Y position at the time of spawn
        initialYPosition = transform.position.y;
    }

    void Update()
    {
        // Calculate the oscillation using a sine wave to move the pipe up and down
        float oscillation = Mathf.Sin(Time.time * verticalSpeed) * verticalAmplitude;

        // Calculate the new Y position using the initial position + the oscillation value
        float newYPosition = Mathf.Lerp(minHeight, maxHeight, (Mathf.Sin(Time.time * verticalSpeed) + 1) / 2); // Convert the sine value (-1 to 1) into a range between minHeight and maxHeight

        // Apply the calculated oscillation
        transform.position = new Vector3(transform.position.x, newYPosition + oscillation, transform.position.z);
    }
}
