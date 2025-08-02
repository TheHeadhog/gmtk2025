using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[RequireComponent(typeof(RectTransform))]
public sealed class Popup : MonoBehaviour, IPointerDownHandler
{
    public event Action OnOpened;

    [SerializeField] private float openDuration = 0.4f;
    [SerializeField] private float initialDelay = 0.4f;
    [SerializeField] private bool shouldOpenOnGameStart = true;
    private RectTransform rect;
    private Canvas rootCanvas;
    private Vector2 canvasHalfSize;
    private Vector3 initialScale;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        rootCanvas = GetComponentInParent<Canvas>();
        canvasHalfSize = rootCanvas.GetComponent<RectTransform>().rect.size * 0.5f;
        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        GameEvents.OnGameStart += OpenPopupOnStart;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= OpenPopupOnStart;
    }

    private void OpenPopupOnStart()
    {
        if (!shouldOpenOnGameStart) return;
        
        OpenPopup();
    }
    
    public void OpenPopup() => StartCoroutine(OpenPopupCoroutine());

    private IEnumerator OpenPopupCoroutine()
    {
        yield return new WaitForSeconds(initialDelay);

        transform.DOScale(initialScale, openDuration).SetEase(Ease.OutExpo).OnComplete(() => OnOpened?.Invoke());
    }
    
    public void ClosePopup() => transform.DOScale(0, openDuration).SetEase(Ease.OutExpo);

    public void ClampToScreen()
    {
        Vector2 halfSize = new Vector2(
            rect.rect.width  * 0.5f * rect.lossyScale.x,
            rect.rect.height * 0.5f * rect.lossyScale.y);

        Vector2 pos = rect.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, -canvasHalfSize.x + halfSize.x, canvasHalfSize.x - halfSize.x);
        pos.y = Mathf.Clamp(pos.y, -canvasHalfSize.y + halfSize.y, canvasHalfSize.y - halfSize.y);

        rect.anchoredPosition = pos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }
}