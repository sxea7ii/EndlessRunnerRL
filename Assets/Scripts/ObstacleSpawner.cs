using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnInterval = 1.5f;
    public float spawnRangeX = 3f;
    public float spawnDistanceAhead = 30f;
    public Transform player;

    void Start()
    {
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true) // infinite loop
        {
            if (player != null)
            {
                SpawnSingleObstacle();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnSingleObstacle()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float spawnZ = player.position.z + spawnDistanceAhead;

        Vector3 spawnPosition = new Vector3(randomX, 0.5f, spawnZ);
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }
}

