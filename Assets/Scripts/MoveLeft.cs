using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [SerializeField] private float speed = 2f;              // Speed at which the object moves left
    private float leftEdge;

    private void Start()
    {
        // Calculate the left boundary of the screen
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    private void Update()
    {
        // Move the object left at a constant speed (negative X direction)
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Destroy the object when it goes out of bounds on the left
        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }
}
