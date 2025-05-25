using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 20f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            foreach (Transform spawn in spawnPoints)
            {
                Instantiate(enemyPrefab, spawn.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
