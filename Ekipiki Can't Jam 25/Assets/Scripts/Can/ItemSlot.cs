using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class ItemSlotSettings
{
    public float failTimeout = 5f;
    public float padding = 40f;
    public Vector2 positionRange = new Vector2(100f, 100f);
}

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [Header("References")]
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private RectTransform[] puzzleItems;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private RectTransform draggableItem;
    [SerializeField] private RectTransform cursorContainer;

    [Header("Events")]
    public UnityEvent onSuccess;
    public UnityEvent onFail;

    [Header("Settings")]
    public ItemSlotSettings settings;

    private Vector2 cursorInitialPosition;
    private Quaternion cursorInitialRotation;
    private bool playerInteracted;
    private int activeItemIndex = -1;
    private Coroutine timeoutRoutine;

    private void Awake()
    {
        ValidateReferences();
        StoreInitialCursorState();
    }

    private void Start()
    {
        ActivateRandomPuzzleItem();
        StartTimeoutWatch();
    }

    private void OnEnable() => StartTimeoutWatch();
    private void OnDisable() => StopTimeoutWatch();

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        playerInteracted = true;
        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        
        onSuccess?.Invoke();
        ResetPuzzleState();
    }

    private void ValidateReferences()
    {
        if (puzzleItems == null || puzzleItems.Length == 0 || canvasRect == null)
        {
            Debug.LogError("Essential references missing in ItemSlot!", this);
            enabled = false;
        }
    }

    private void StoreInitialCursorState()
    {
        if (cursor != null)
        {
            cursorInitialPosition = cursor.anchoredPosition;
            cursorInitialRotation = cursor.localRotation;
        }
    }

    private void ActivateRandomPuzzleItem()
    {
        DeactivateAllItems();
        activeItemIndex = Random.Range(0, puzzleItems.Length);
        puzzleItems[activeItemIndex].gameObject.SetActive(true);
        RandomizeItemPosition(puzzleItems[activeItemIndex]);
        RandomizeItemPosition(cursorContainer);
    }

    private void DeactivateAllItems()
    {
        foreach (var item in puzzleItems)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void RandomizeItemPosition(RectTransform item)
    {
        if (canvasRect == null) return;

        Vector2 canvasSize = canvasRect.rect.size;
        Vector2 itemSize = item.rect.size * item.lossyScale;

        float xPos = Random.Range(
            -canvasSize.x / 2 + itemSize.x / 2 + settings.padding,
            canvasSize.x / 2 - itemSize.x / 2 - settings.padding
        );

        float yPos = Random.Range(
            -canvasSize.y / 2 + itemSize.y / 2 + settings.padding,
            canvasSize.y / 2 - itemSize.y / 2 - settings.padding
        );

        item.anchoredPosition = new Vector2(xPos, yPos);
        item.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        
        ResetPuzzleState();
    }

    private void ResetPuzzleState()
    {
        if (cursorContainer != null)
        {
            cursorContainer.anchoredPosition = cursorInitialPosition;
            cursorContainer.localRotation = cursorInitialRotation;
        }

        playerInteracted = false;
        ActivateRandomPuzzleItem();
        StartTimeoutWatch();
    }

    private void StartTimeoutWatch()
    {
        StopTimeoutWatch();
        timeoutRoutine = StartCoroutine(TimeoutWatch());
    }

    private void StopTimeoutWatch()
    {
        if (timeoutRoutine != null)
        {
            StopCoroutine(timeoutRoutine);
            timeoutRoutine = null;
        }
    }

    private IEnumerator TimeoutWatch()
    {
        playerInteracted = false;
        yield return new WaitForSeconds(settings.failTimeout);

        if (!playerInteracted)
        {
            onFail?.Invoke();
            ResetPuzzleState();
        }
    }
}