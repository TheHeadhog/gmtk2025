using System;
using DG.Tweening;
using UnityEngine;

public class GameHeader : MonoBehaviour
{
    [SerializeField] private float _slideDuration = 0.5f;
    [SerializeField] private Ease _slideEase = Ease.OutCubic;
    [SerializeField] private float _offset = 200f;

    private RectTransform _rect;
    private Vector2 _visiblePos;
    private Vector2 _hiddenPos;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _visiblePos = _rect.anchoredPosition;
        _hiddenPos = _visiblePos + new Vector2(0f, _offset + _rect.rect.height);
        _rect.anchoredPosition = _hiddenPos;
    }

    private void OnEnable()
    {
        GameEvents.OnGameStart += Open;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= Open;
    }

    private void Open()
    {
        _rect.DOAnchorPos(_visiblePos, _slideDuration).SetEase(_slideEase);
    }

    private void Close()
    {
        _rect.DOAnchorPos(_hiddenPos, _slideDuration).SetEase(_slideEase);
    }
}