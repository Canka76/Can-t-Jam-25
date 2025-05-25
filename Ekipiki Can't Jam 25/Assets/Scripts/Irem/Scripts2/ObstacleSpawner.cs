using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public float spawnInterval = 2f;
    public Transform spawnPoint;

    private float timer = 0f;
    public bool spawningEnabled = true;
    void Update()
    {
        if (!spawningEnabled) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    void SpawnObstacle()
    {
        // Kontrol: SpawnPoint veya Prefab listesi atanmamýþsa dur
        if (spawnPoint == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("SpawnPoint veya obstaclePrefabs eksik!");
            return;
        }

        int index = Random.Range(0, obstaclePrefabs.Length);
        Instantiate(obstaclePrefabs[index], spawnPoint.position, Quaternion.identity);
    }
}
