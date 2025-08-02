using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public sealed class PopupHeader : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Popup popup;
    private RectTransform popupRect;
    private Vector2 offset;

    private void Awake()
    {
        popupRect = popup.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(popupRect, e.position, e.pressEventCamera,
            out Vector2 local);
        offset = popupRect.anchoredPosition - local;
    }

    public void OnDrag(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(popupRect.parent as RectTransform, e.position,
            e.pressEventCamera, out Vector2 localParent);
        popupRect.anchoredPosition = localParent + offset;
        popup.ClampToScreen();
    }

    public void OnEndDrag(PointerEventData e)
    {
    }
}