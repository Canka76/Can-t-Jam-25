using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public GameObject specialTargetPrefab;
    public float spawnInterval = 0.5f;
    public float spawnRadius = 5f;
    public Transform player;

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
        // Sabit yön sağdan gelsin
        Vector2 playerPos = player.position;

        // Belirli dikdörtgen bir alan (ön taraf)
        float spawnX = playerPos.x + Random.Range(10f, 15f);         // Oyuncunun sağında
        float spawnY = playerPos.y + Random.Range(-2f, 2f);        // Yükseklik aralığı
        Vector2 spawnPos = new Vector2(spawnX, spawnY);

        GameObject prefabToSpawn;

        if (!GameManager.Instance.specialTargetSpawned && Random.value < 0.2f)
        {
            prefabToSpawn = specialTargetPrefab;
            GameManager.Instance.specialTargetSpawned = true; // ❗ Tekrar spawn olmasın
            Debug.Log("Özel hedef spawn edildi!");
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
