using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public sealed class LogoStartAnimation : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private RectTransform _logo;

    [SerializeField] private UIButton _startButton;

    [Header("Animation")] [SerializeField] private float _expandScale = 1.25f;
    [SerializeField] private float _expandDuration = 0.45f;
    [SerializeField] private Ease _expandEase = Ease.OutBack;
    [SerializeField] private float _shrinkDuration = 0.35f;
    [SerializeField] private Ease _shrinkEase = Ease.InBack;
    [SerializeField] private float _buttonHideDuration = 0.25f;
    [SerializeField] private Ease _buttonHideEase = Ease.InBack;

    private const int _buttonOffset = 300;

    private bool _isAnimating;
    private Sequence _sequence;
    private RectTransform _buttonRect;
    private Vector2 _buttonHiddenPos;

    private void Awake()
    {
        _buttonRect = _startButton.GetComponent<RectTransform>();
        _buttonHiddenPos = _buttonRect.anchoredPosition -
                           new Vector2(0f, _buttonOffset + _buttonRect.rect.height);
    }

    private void Start()
    {
        _startButton.AddOnClickListener(StartAnimation);
    }

    private void StartAnimation()
    {
        if (_isAnimating)
        {
            return;
        }

        _isAnimating = true;

        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        _sequence.Append(_logo.DOScale(_expandScale, _expandDuration).SetEase(_expandEase));
        _sequence.Insert(0f, _buttonRect.DOAnchorPos(_buttonHiddenPos, _buttonHideDuration)
            .SetEase(_buttonHideEase));
        _sequence.Append(_logo.DOScale(0f, _shrinkDuration).SetEase(_shrinkEase));
        _sequence.OnComplete(GameEvents.RaiseGameStart);
    }
}