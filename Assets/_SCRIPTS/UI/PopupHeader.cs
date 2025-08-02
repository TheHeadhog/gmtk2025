using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public sealed class PopupHeader : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField] private Popup popup;

    private RectTransform popupRect;
    private RectTransform canvasRect;
    private Vector2 offset;

    private void Awake()
    {
        popupRect = popup.GetComponent<RectTransform>();
        canvasRect = popupRect.parent as RectTransform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out var localPointerPos);

        offset = popupRect.anchoredPosition - localPointerPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out var localPointerPos);

        popupRect.anchoredPosition = localPointerPos + offset;
        popup.ClampToScreen();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}