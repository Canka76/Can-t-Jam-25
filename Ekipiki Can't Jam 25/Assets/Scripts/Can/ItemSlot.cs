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

    private Vector2 cursorInitialPosition;
    private Quaternion cursorInitialRotation;
    private bool randomizedByPlayer = false;

    void Start()
    {
        if (cursorTransform != null)
        {
            cursorInitialPosition = cursorTransform.anchoredPosition;
            cursorInitialRotation = cursorTransform.localRotation;
        }

        RandomizeUIPositionAndRotation();
        StartCoroutine(WatchForPlayerRandomization());
    }

    private void OnEnable()
    {
        cursorInitialPosition = cursorTransform.anchoredPosition;
        cursorInitialRotation = cursorTransform.localRotation;
        
        RandomizeUIPositionAndRotation();
        StartCoroutine(WatchForPlayerRandomization());
        
        //BUG Fix Slider
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

                RandomizeUIPositionAndRotation();
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
                RandomizeUIPositionAndRotation();
                puzzleManager.currentProgress--;
            }
            else
            {
                puzzleManager.currentProgress++;
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
