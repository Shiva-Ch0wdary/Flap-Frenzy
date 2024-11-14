using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Pipes pipePrefabA;                     // Pipe A prefab reference (pipe up)
    public Pipes pipePrefabB;                     // Pipe B prefab reference (2 pipes)
    public Pipes pipePrefabC;                     // Pipe C prefab reference (pipe down)
    public Pipes pipePrefabD;                     // Pipe D prefab reference (2 pipes)
    public Pipes pipePrefabE;                     // Pipe E prefab reference (single pipe moving up and down)

    public float spawnRate = 2f;                  // Delay between pipe spawns (in seconds)
    public float minHeight = -1f;                 // Minimum height for pipes
    public float maxHeight = 2f;                  // Maximum height for pipes
    public float verticalGap = 3f;                // Vertical gap between pipes

    private Coroutine spawnCoroutine;             // Reference to the spawn coroutine
    private List<GameObject> spawnedObjects = new List<GameObject>(); // Track all spawned objects

    private void OnEnable()
    {
        StartSpawning(); // Start spawning when the game is enabled
    }

    private void OnDisable()
    {
        StopSpawning(); // Stop spawning when the game is disabled (game over)
    }

    // Method to start spawning
    private void StartSpawning()
    {
        if (spawnCoroutine == null) // Ensure we don't start multiple coroutines
        {
            spawnCoroutine = StartCoroutine(SpawnWithFixedDelay());
        }
    }

    // Method to stop spawning
    private void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine); // Stop the current spawn coroutine
            spawnCoroutine = null; // Reset the coroutine reference
        }
    }

    // Coroutine to spawn objects with a fixed delay and randomly choose a prefab
    private IEnumerator SpawnWithFixedDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate); // Fixed delay between spawns

            // Randomly choose a prefab to spawn
            int prefabChoice = Random.Range(0, 5); // Options 0-4 for prefabs A-E
            switch (prefabChoice)
            {
                case 0:
                    SpawnPipePrefab(pipePrefabA); // Prefab A
                    break;
                case 1:
                    SpawnPipePrefab(pipePrefabB); // Prefab B (2 pipes)
                    break;
                case 2:
                    SpawnPipePrefab(pipePrefabC); // Prefab C
                    break;
                case 3:
                    SpawnPipePrefab(pipePrefabD); // Prefab D (2 pipes)
                    break;
                case 4:
                    SpawnPipePrefab(pipePrefabE); // Prefab E (single pipe moving up and down)
                    break;
            }
        }
    }

    // Spawn pipe prefabs
    private void SpawnPipePrefab(Pipes pipePrefab)
    {
        Pipes pipeInstance = Instantiate(pipePrefab, transform.position, Quaternion.identity);
        float randomHeight = Random.Range(minHeight, maxHeight);
        pipeInstance.transform.position += Vector3.up * randomHeight;

        pipeInstance.gap = verticalGap;
        spawnedObjects.Add(pipeInstance.gameObject); // Track the spawned pipe

        // Set the tag to "Pipe" for collision detection with the bird
        pipeInstance.gameObject.tag = "Pipe";
    }

    // Call this method to handle the game over logic
    public void GameOver()
    {
        StopSpawning(); // Stop spawning pipes
        DestroySpawnedObjects(); // Destroy all currently spawned objects
        // You can also add any UI/game over handling code here, such as showing a game over screen
    }

    // Call this method to restart the game
    public void RestartGame()
    {
        DestroySpawnedObjects(); // Destroy any existing objects before restarting
        StartSpawning(); // Restart the spawning logic
    }

    // Destroy all spawned objects currently in the scene
    private void DestroySpawnedObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj); // Destroy the object
            }
        }
        spawnedObjects.Clear(); // Clear the list of spawned objects
    }
}
