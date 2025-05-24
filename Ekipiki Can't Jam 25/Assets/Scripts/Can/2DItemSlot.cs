using UnityEngine;
using System.Collections;

public class SquareSlot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] squareItems;
    [SerializeField] private Transform cursor;
    [SerializeField] private Transform squarePrefab;

    [Header("Timing")]
    [SerializeField] private float failTimeout = 5f;

    [Header("Bounds")]
    [SerializeField] private float areaWidth = 10f;
    [SerializeField] private float areaHeight = 5f;
    [SerializeField] private float padding = 0.5f;

    private Vector3 cursorInitialPosition;
    private Quaternion cursorInitialRotation;
    private bool randomizedByPlayer = false;
    private int currentActiveIndex = -1;

    private void Start()
    {
        if (cursor != null)
        {
            cursorInitialPosition = cursor.position;
            cursorInitialRotation = cursor.rotation;
        }

        RandomizeActiveSquare();
        StartCoroutine(WatchForPlayerRandomization());
    }

    public void OnPlayerDrop(GameObject droppedSquare)
    {
        if (droppedSquare != null)
        {
            droppedSquare.transform.position = transform.position;
            Debug.Log("Successfully dropped square!");

            randomizedByPlayer = true;
            RandomizeActiveSquare();
            RandomizeCursorPosition();
        }
    }

    private void RandomizeCursorPosition()
    {
        if (cursor != null)
        {
            cursor.position = cursorInitialPosition;
            cursor.rotation = cursorInitialRotation;
        }

        if (squarePrefab == null) return;

        Vector2 halfSize = GetColliderSize(squarePrefab) / 2f;

        float minX = -areaWidth / 2f + halfSize.x + padding;
        float maxX = areaWidth / 2f - halfSize.x - padding;
        float minY = -areaHeight / 2f + halfSize.y + padding;
        float maxY = areaHeight / 2f - halfSize.y - padding;

        Vector2 randomPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        float randomRot = Random.Range(-360f, 360f);

        squarePrefab.position = randomPos;
        squarePrefab.rotation = Quaternion.Euler(0f, 0f, randomRot);
    }

    private void RandomizeActiveSquare()
    {
        if (cursor != null)
        {
            cursor.position = cursorInitialPosition;
            cursor.rotation = cursorInitialRotation;
        }

        foreach (var square in squareItems)
        {
            square.gameObject.SetActive(false);
        }

        if (squareItems.Length == 0)
        {
            Debug.LogWarning("No square items assigned.");
            return;
        }

        currentActiveIndex = Random.Range(0, squareItems.Length);
        Transform activeSquare = squareItems[currentActiveIndex];
        activeSquare.gameObject.SetActive(true);

        Vector2 halfSize = GetColliderSize(activeSquare) / 2f;

        float minX = -areaWidth / 2f + halfSize.x + padding;
        float maxX = areaWidth / 2f - halfSize.x - padding;
        float minY = -areaHeight / 2f + halfSize.y + padding;
        float maxY = areaHeight / 2f - halfSize.y - padding;

        int attempts = 0;
        bool placed = false;

        while (attempts < 50 && !placed)
        {
            Vector2 randomPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            float randomRot = Random.Range(-60f, 60f);
            activeSquare.position = randomPos;
            activeSquare.rotation = Quaternion.Euler(0f, 0f, randomRot);

            if (!IsOverlapping(activeSquare, squarePrefab))
            {
                placed = true;
            }

            attempts++;
        }

        if (!placed)
        {
            Debug.LogWarning("Failed to place non-overlapping square.");
        }
    }

    private IEnumerator WatchForPlayerRandomization()
    {
        while (true)
        {
            randomizedByPlayer = false;
            yield return new WaitForSeconds(failTimeout);

            if (!randomizedByPlayer)
            {
                Debug.LogWarning("Player failed to drop in time. Randomizing.");
                RandomizeActiveSquare();
                RandomizeCursorPosition();
            }
            else
            {
                Debug.Log("Player succeeded. Timer reset.");
            }
        }
    }

    private bool IsOverlapping(Transform a, Transform b)
    {
        BoxCollider2D colA = a.GetComponent<BoxCollider2D>();
        BoxCollider2D colB = b.GetComponent<BoxCollider2D>();

        if (colA == null || colB == null) return false;

        return colA.bounds.Intersects(colB.bounds);
    }

    private Vector2 GetColliderSize(Transform obj)
    {
        BoxCollider2D collider = obj.GetComponent<BoxCollider2D>();
        return collider != null ? collider.size : Vector2.one;
    }
}
