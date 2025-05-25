using UnityEngine;

public class TargetSpawnerRune : MonoBehaviour
{
    public GameObject targetPrefab;
    public GameObject specialTargetPrefab;
    public float spawnInterval = 0.5f;
    public Transform player;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnTarget();
            timer = 0f;
        }
    }

    void SpawnTarget()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("Spawn point atanmadı!");
            return;
        }

        // Rastgele bir spawn point seç
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector2 spawnPos = spawnPoint.position;

        GameObject prefabToSpawn;

        if (!GameManager.Instance.specialTargetSpawned && Random.value < 0.2f)
        {
            prefabToSpawn = specialTargetPrefab;
            GameManager.Instance.specialTargetSpawned = true;
            Debug.Log("💠 Özel hedef spawn edildi!");
        }
        else
        {
            prefabToSpawn = targetPrefab;
        }

        GameObject target = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        TargetMovement moveScript = target.GetComponent<TargetMovement>();
        if (moveScript != null)
        {
            moveScript.player = player;
        }
    }
}
