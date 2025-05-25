using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class RitimKaydirma : MonoBehaviour, IPointerDownHandler, IEndDragHandler, IBeginDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float returnIdleDelay = 1f; // Delay in seconds before returning to idle

    private RectTransform _rectTransform;
    private Animator _animator;

    public ItemSlot itemSlot;
    public GameObject itemPrefab;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        _animator = GetComponent<Animator>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _animator.ResetTrigger("isTaken");
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        _rectTransform.anchoredPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _animator.SetTrigger("isTaken");

        if (itemSlot.Succeed == 2)
        {
            _animator.SetBool("isSuccess", true);
            Debug.LogWarning("SuccessDrag");

            DisableImageRenderer();
        }
        else
        {
            _animator.SetBool("isSuccess", false);
        }

        // Delay the return to idle animation
        StartCoroutine(DelayedReturnToIdle(returnIdleDelay));

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    private IEnumerator DelayedReturnToIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToIdle();
    }

    private void ReturnToIdle()
    {
        _animator.SetTrigger("returnIdle");
        EnableImageRenderer();
    }

    public void DisableImageRenderer()
    {
        if (itemPrefab != null)
        {
            Image img = itemPrefab.GetComponent<Image>();
            if (img != null)
            {
                img.enabled = false; // Disable the Image component rendering
            }
            else
            {
                Debug.LogWarning("No Image component found on target GameObject.");
            }
        }
    }
    
    public void EnableImageRenderer()
    {
        if (itemPrefab != null)
        {
            Image img = itemPrefab.GetComponent<Image>();
            if (img != null)
            {
                img.enabled = true; // Disable the Image component rendering
            }
            else
            {
                Debug.LogWarning("No Image component found on target GameObject.");
            }
        }
    }
}
