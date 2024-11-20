using UnityEngine;
using System.Collections;
public class CoinsSpawner : MonoBehaviour
{
    public GameObject coinPrefab; // Assign the Coin prefab in the Inspector
    public float spawnRate = 3f;
    public float spawnRadius = 0.5f;
    private float nextSpawnTime;

    private void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnCoin();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    private void SpawnCoin()
    {
        float randomY = Random.Range(-2f, 3f); // Adjust Y range as needed
        Vector3 spawnPosition = new Vector3(10f, randomY, 0f);
        if (Physics2D.OverlapCircle(spawnPosition, spawnRadius) == null)
        {
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
