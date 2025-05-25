using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Random = UnityEngine.Random;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [Header("References")]
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private RectTransform itemPrefab;
    [SerializeField] private RectTransform CursorsTransform;
    public PuzzleManager puzzleManager;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Manual Boundaries")]
    [SerializeField] private RectTransform upperLeftBoundary;
    [SerializeField] private RectTransform lowerLeftBoundary;
    [SerializeField] private RectTransform upperRightBoundary;
    [SerializeField] private RectTransform lowerRightBoundary;

    [Header("Timing")]
    [SerializeField] private float failTimeout = 5f;

    [SerializeField] private float padding = 40f;

    [Header("Delay Settings")]
    [SerializeField] private float randomizeDelay = 1f;  // Delay before randomizing UI

    private Quaternion cursorInitialRotation;
    private bool randomizedByPlayer = false;

    public int Succeed = 0;

    void Start()
    {
        if (cursorTransform != null)
        {
            cursorInitialRotation = cursorTransform.localRotation;
        }

        StartCoroutine(RandomizeWithDelayCoroutine());
    }

    private void OnEnable()
    {
        if (cursorTransform != null)
        {
            cursorInitialRotation = cursorTransform.localRotation;
        }
        
        StartCoroutine(RandomizeWithDelayCoroutine());
        StartCoroutine(WatchForPlayerRandomization());

        // BUG Fix Slider
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            RectTransform droppedItem = eventData.pointerDrag.GetComponent<RectTransform>();
            if (droppedItem != null)
            {
                droppedItem.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                randomizedByPlayer = true;

                Succeed = 2;

                StartCoroutine(RandomizeWithDelayCoroutine());
                puzzleManager.currentProgress++;
            }
        }
    }

    // Public method to start randomization with delay
    public void RandomizeUIPositionAndRotation()
    {
        StartCoroutine(RandomizeWithDelayCoroutine());
    }

    // Coroutine that waits before randomizing
    private IEnumerator RandomizeWithDelayCoroutine()
    {
        yield return new WaitForSeconds(randomizeDelay);
        _RandomizeUIPositionAndRotation();
    }

    // Actual randomization logic
    private void _RandomizeUIPositionAndRotation()
    {
        Succeed = 0;

        if (CursorsTransform != null)
        {
            // Set fixed anchored position every time
            CursorsTransform.anchoredPosition = new Vector2(-170f, 0f);
            CursorsTransform.localRotation = cursorInitialRotation;
        }

        if (itemPrefab == null)
        {
            Debug.LogWarning("Missing itemPrefab!");
            return;
        }

        Vector2 objectSize = itemPrefab.rect.size * itemPrefab.lossyScale;
        GetManualBounds(out float minX, out float maxX, out float minY, out float maxY, objectSize);

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        float randomZ = Random.Range(-360f, 360f);

        itemPrefab.anchoredPosition = new Vector2(randomX, randomY);
        itemPrefab.localRotation = Quaternion.Euler(0f, 0f, randomZ);
    }

    private IEnumerator WatchForPlayerRandomization()
    {
        while (true)
        {
            randomizedByPlayer = false;
            yield return new WaitForSeconds(failTimeout);

            if (!randomizedByPlayer)
            {
                Debug.LogWarning("Player failed. Randomizing again.");

                Succeed = 1;

                StartCoroutine(RandomizeWithDelayCoroutine());
                puzzleManager.currentProgress--;
            }
        }
    }

    private void GetManualBounds(out float minX, out float maxX, out float minY, out float maxY, Vector2 objectSize)
    {
        minX = Mathf.Max(upperLeftBoundary.anchoredPosition.x, lowerLeftBoundary.anchoredPosition.x) + objectSize.x / 2f + padding;
        maxX = Mathf.Min(upperRightBoundary.anchoredPosition.x, lowerRightBoundary.anchoredPosition.x) - objectSize.x / 2f - padding;
        minY = Mathf.Max(lowerLeftBoundary.anchoredPosition.y, lowerRightBoundary.anchoredPosition.y) + objectSize.y / 2f + padding;
        maxY = Mathf.Min(upperLeftBoundary.anchoredPosition.y, upperRightBoundary.anchoredPosition.y) - objectSize.y / 2f - padding;
    }

    void OnDrawGizmosSelected()
    {
        if (upperLeftBoundary && upperRightBoundary && lowerRightBoundary && lowerLeftBoundary)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(upperLeftBoundary.position, upperRightBoundary.position);
            Gizmos.DrawLine(upperRightBoundary.position, lowerRightBoundary.position);
            Gizmos.DrawLine(lowerRightBoundary.position, lowerLeftBoundary.position);
            Gizmos.DrawLine(lowerLeftBoundary.position, upperLeftBoundary.position);
        }
    }
}
