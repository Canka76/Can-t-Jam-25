using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RitimKaydirma : MonoBehaviour, IPointerDownHandler, IEndDragHandler, IBeginDragHandler, IDragHandler, IDropHandler
{
   [SerializeField] private Canvas canvas;
   [SerializeField] private CanvasGroup canvasGroup;
   private RectTransform _rectTransform;
   

   private void Awake()
   {
      _rectTransform = GetComponent<RectTransform>();
      canvasGroup = GetComponent<CanvasGroup>();
   }

   public void OnPointerDown(PointerEventData eventData)
   {
      Debug.Log("OnPointerDown");
   }

   public void OnEndDrag(PointerEventData eventData)
   {
      canvasGroup.blocksRaycasts = true;
      canvasGroup.alpha = 1;
   }

   public void OnBeginDrag(PointerEventData eventData)
   {
      canvasGroup.blocksRaycasts = false;
      canvasGroup.alpha = .6f;
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

   public void OnDrop(PointerEventData eventData)
   {
      throw new NotImplementedException();
   }
}