using DG.Tweening;
using UnityEngine;

public class PopupsHandler : MonoBehaviour
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
    }

    private void OnEnable()
    {
        GameEvents.OnPauseGameKeyPressed += HandleOpenClose;
    }

    private void OnDisable()
    {
        GameEvents.OnPauseGameKeyPressed -= HandleOpenClose;
    }

    private void HandleOpenClose(bool isOpening)
    {
        if (isOpening)
        {
            Close();
        }
        else
        {
            Open();
        }
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
