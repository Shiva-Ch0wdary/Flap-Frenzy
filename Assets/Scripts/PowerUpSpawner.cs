using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUps;        // Array of power-up prefabs
    public Transform player;             // Reference to the player
    public float spawnInterval = 10f;    // Time between each spawn
    public float spawnDistance = 8f;     // Distance to spawn to the right of the player

    private void Start()
    {
        StartCoroutine(SpawnPowerUpRoutine());
    }

    private IEnumerator SpawnPowerUpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Check if there are any power-ups to spawn
            if (powerUps == null || powerUps.Length == 0)
            {
                Debug.LogWarning("PowerUpSpawner: No power-ups assigned in the powerUps array.");
                continue;  // Skip this loop iteration if no power-ups are assigned
            }

            // Choose a random power-up prefab
            GameObject selectedPowerUp = powerUps[Random.Range(0, powerUps.Length)];

            // Spawn position is to the right of the player, with a specified distance
            Vector3 spawnPosition = player.position + Vector3.right * spawnDistance;

            // Instantiate the power-up
            GameObject spawnedPowerUp = Instantiate(selectedPowerUp, spawnPosition, Quaternion.identity);

            // Attach MoveLeft script if it’s not already attached
            if (!spawnedPowerUp.TryGetComponent(out MoveLeft moveLeft))
            {
                moveLeft = spawnedPowerUp.AddComponent<MoveLeft>();
            }
        }
    }
}
