using UnityEngine;
using UnityEngine.UI;

public class BarWidenController : MonoBehaviour
{
    [Header("Bar References")]
    [SerializeField] private RectTransform playerBarRect;
    [SerializeField] private RectTransform enemyBarRect;
    [SerializeField] private ItemSlot itemSlot;

    [Header("Width Settings")]
    [SerializeField] private float widthChangeAmount = 50f;
    [SerializeField] private float maxWidth = 400f;
    [SerializeField] private float minWidth = 0f;

    [Header("Animation Settings")]
    [SerializeField] private float animationTime = 0.2f;
    [SerializeField] private LeanTweenType easeType = LeanTweenType.easeInOutElastic;

    [Header("Debug")]
    [SerializeField] private float currentAdvantage;

    private float playerCurrentWidth;
    private float enemyCurrentWidth;

    private void Awake()
    {
        ValidateReferences();
        InitializeBars();
    }

    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnregisterEvents();
    }

    private void ValidateReferences()
    {
        if (playerBarRect == null || enemyBarRect == null)
        {
            Debug.LogError("Bar references not assigned!", this);
            enabled = false;
        }
    }

    private void InitializeBars()
    {
        Vector2 pivot = new Vector2(0f, 0.5f);
        playerBarRect.pivot = pivot;
        enemyBarRect.pivot = pivot;

        playerCurrentWidth = playerBarRect.sizeDelta.x;
        enemyCurrentWidth = enemyBarRect.sizeDelta.x;
    }

    private void RegisterEvents()
    {
        if (itemSlot != null)
        {
            itemSlot.onSuccess.RemoveListener(HandleSuccess);
            itemSlot.onFail.RemoveListener(HandleFail);

            itemSlot.onSuccess.AddListener(HandleSuccess);
            itemSlot.onFail.AddListener(HandleFail);
        }
    }

    private void UnregisterEvents()
    {
        if (itemSlot != null)
        {
            itemSlot.onSuccess.RemoveListener(HandleSuccess);
            itemSlot.onFail.RemoveListener(HandleFail);
        }
    }

    private void HandleSuccess()
    {
        if (currentAdvantage >= 0)
            IncreasePlayerBar();
        else
            DecreaseEnemyBar();

        currentAdvantage = Mathf.Clamp(currentAdvantage + 1, -3, 3);
    }

    private void HandleFail()
    {
        if (currentAdvantage > 0)
            DecreasePlayerBar();
        else
            IncreaseEnemyBar();

        currentAdvantage = Mathf.Clamp(currentAdvantage - 1, -3, 3);
    }

    private void AdjustBar(RectTransform bar, ref float currentWidth, float changeAmount)
    {
        float newWidth = Mathf.Clamp(currentWidth + changeAmount, minWidth, maxWidth);

        if (Mathf.Approximately(newWidth, currentWidth)) return;

        currentWidth = newWidth;

        // Cancel any existing tween to prevent memory bloat
        LeanTween.cancel(bar.gameObject);

        LeanTween.size(bar, new Vector2(currentWidth, bar.sizeDelta.y), animationTime)
                 .setEase(easeType);
    }

    private void IncreasePlayerBar() => AdjustBar(playerBarRect, ref playerCurrentWidth, widthChangeAmount);
    private void DecreasePlayerBar() => AdjustBar(playerBarRect, ref playerCurrentWidth, -widthChangeAmount);
    private void IncreaseEnemyBar() => AdjustBar(enemyBarRect, ref enemyCurrentWidth, widthChangeAmount);
    private void DecreaseEnemyBar() => AdjustBar(enemyBarRect, ref enemyCurrentWidth, -widthChangeAmount);
}
