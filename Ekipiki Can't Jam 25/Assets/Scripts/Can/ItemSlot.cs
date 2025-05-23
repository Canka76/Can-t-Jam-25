using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [Header("References")]
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private RectTransform[] imageItems;
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private RectTransform itemPrefab;
    [SerializeField] private RectTransform CursorsTransform;

    [Header("Timing")]
    [SerializeField] private float failTimeout = 5f;
    
    [SerializeField]float padding = 40f;

    private Vector2 cursorInitialPosition;
    private Quaternion cursorInitialRotation;
    private bool randomizedByPlayer = false;
    private int currentActiveIndex = -1;

    void Start()
    {
        if (cursorTransform != null)
        {
            cursorInitialPosition = cursorTransform.anchoredPosition;
            cursorInitialRotation = cursorTransform.localRotation;
        }

        RandomizeActiveImage();

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

                randomizedByPlayer = true;
                RandomizeActiveImage();
                RandomizeUIPositionAndRotation(); // untouched method
            }
        }
    }

    public void RandomizeUIPositionAndRotation()
    {
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

    private void RandomizeActiveImage()
    {
        // Reset cursor
        if (cursorTransform != null)
        {
            cursorTransform.anchoredPosition = cursorInitialPosition;
            cursorTransform.localRotation = cursorInitialRotation;
        }

        if (imageItems == null || imageItems.Length == 0 || canvasRectTransform == null)
        {
            Debug.LogWarning("Missing image items or canvas reference!");
            return;
        }

        // Deactivate all images
        foreach (var img in imageItems)
        {
            img.gameObject.SetActive(false);
        }

        // Pick a random image
        currentActiveIndex = Random.Range(0, imageItems.Length);
        RectTransform activeImage = imageItems[currentActiveIndex];
        activeImage.gameObject.SetActive(true);

        Vector2 imageSize = activeImage.rect.size * activeImage.lossyScale;
        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;

        
        float minX = -canvasWidth / 2f + imageSize.x / 2f + padding;
        float maxX = canvasWidth / 2f - imageSize.x / 2f - padding;
        float minY = -canvasHeight / 2f + imageSize.y / 2f + padding;
        float maxY = canvasHeight / 2f - imageSize.y / 2f - padding;

        int attempts = 0;
        bool placed = false;

        while (attempts < 50 && !placed)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            float randomZ = Random.Range(-60f, 60f);

            activeImage.anchoredPosition = new Vector2(randomX, randomY);
            activeImage.localRotation = Quaternion.Euler(0f, 0f, randomZ);

            // Let Unity update transform matrix for next frame
            Canvas.ForceUpdateCanvases();

            if (!IsOverlapping(activeImage, itemPrefab))
            {
                placed = true;
                break;
            }

            attempts++;
        }

        if (!placed)
        {
            Debug.LogWarning("Could not find non-colliding position after 50 tries.");
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
                Debug.LogWarning("Player failed to drop in time. Randomizing again.");
                RandomizeActiveImage();
                RandomizeUIPositionAndRotation();
            }
            else
            {
                Debug.Log("Player succeeded. Resetting timer.");
            }
        }
    }

    private bool IsOverlapping(RectTransform a, RectTransform b)
    {
        if (a == null || b == null) return false;

        Vector3[] cornersA = new Vector3[4];
        Vector3[] cornersB = new Vector3[4];

        a.GetWorldCorners(cornersA);
        b.GetWorldCorners(cornersB);

        Rect rectA = new Rect(cornersA[0], cornersA[2] - cornersA[0]);
        Rect rectB = new Rect(cornersB[0], cornersB[2] - cornersB[0]);

        return rectA.Overlaps(rectB);
    }
}
