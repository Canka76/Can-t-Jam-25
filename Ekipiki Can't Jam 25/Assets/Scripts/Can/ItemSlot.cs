using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public RectTransform canvasRectTransform; // Assign in Inspector
    public RectTransform itemPrefab;          // Assign in Inspector
    public RectTransform CursorsTransform;    // Assign in Inspector

    private Vector2 cursorInitialPosition;
    private Quaternion cursorInitialRotation;

    private bool randomizedByPlayer = false;
    [SerializeField] float failTimeout = 5f; // Time to wait before logging failure

    void Start()
    {
        // Store Cursor's initial transform
        if (CursorsTransform != null)
        {
            cursorInitialPosition = CursorsTransform.anchoredPosition;
            cursorInitialRotation = CursorsTransform.localRotation;
        }

        RandomizeUIPositionAndRotation();

        // Start coroutine to monitor repeatedly
        StartCoroutine(WatchForPlayerRandomization());
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            RectTransform droppedItem = eventData.pointerDrag.GetComponent<RectTransform>();
            if (droppedItem != null)
            {
                droppedItem.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                Debug.Log("Successfully dropped item");

                // Mark that player triggered the randomization
                randomizedByPlayer = true;
                RandomizeUIPositionAndRotation();
            }
        }
    }

    public void RandomizeUIPositionAndRotation()
    {
        // Reset the cursor's transform
        if (CursorsTransform != null)
        {
            CursorsTransform.anchoredPosition = cursorInitialPosition;
            CursorsTransform.localRotation = cursorInitialRotation;
        }
        
        if (itemPrefab == null || canvasRectTransform == null)
        {
            Debug.LogWarning("Missing itemPrefab or canvasRectTransform!");
            return;
        }

        Vector2 objectSize = itemPrefab.rect.size * itemPrefab.lossyScale;
        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;

        float padding = 20f;

        float minX = -canvasWidth / 2f + objectSize.x / 2f + padding;
        float maxX = canvasWidth / 2f - objectSize.x / 2f - padding;
        float minY = -canvasHeight / 2f + objectSize.y / 2f + padding;
        float maxY = canvasHeight / 2f - objectSize.y / 2f - padding;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        float randomZ = Random.Range(-360f, 360f);

        itemPrefab.anchoredPosition = new Vector2(randomX, randomY);
        itemPrefab.localRotation = Quaternion.Euler(0f, 0f, randomZ);
    }

    private IEnumerator WatchForPlayerRandomization()
    {
        while (true) // keep monitoring indefinitely
        {
            randomizedByPlayer = false;

            yield return new WaitForSeconds(failTimeout);

            if (!randomizedByPlayer)
            {
                Debug.LogWarning("Player failed to trigger RandomizeUIPositionAndRotation within the time limit.");
                RandomizeUIPositionAndRotation();   
            }
            else
            {
                Debug.Log("Player triggered randomization within the time limit, restarting check.");
            }
        }
    }
}
