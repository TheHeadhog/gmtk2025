using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
                                       IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    private enum SpriteMode { NoneProvided, Partial, Full }
    private enum SpriteState { Normal, Hovered, Clicked }

    [Header("Sprite States")]
    [SerializeField] Sprite _normalSprite;
    [SerializeField] Sprite _hoveredSprite;
    [SerializeField] Sprite _clickedSprite;

    [Header("Animation Settings")]
    [SerializeField] float _animationDuration = 0.2f;
    [SerializeField] bool _shouldPlayClickedAnimation;
    [SerializeField] bool _shouldPlayHoverAnimation;

    [Tooltip("Scale relative to the default scale when clicked.")]
    [SerializeField]
    protected float _clickedTargetScaleRelativeTo1 = 0.8f;

    [Tooltip("Scale relative to the default scale when hovered.")]
    [SerializeField]
    protected float _hoveredTargetScaleRelativeTo1 = 1.1f;

    [Header("Anti-Spam")]
    [SerializeField] private bool _addBuffer = true;

    private Button _button;
    private Image _image;
    private EventSystem _eventSystem;

    protected Vector3 _defaultScale;
    private Color _originalColor;

    protected bool _hoverLocked = false;
    private bool _isOnCooldown = false;

    private SpriteMode _spriteMode;
    private Sprite _initialSprite;

    protected virtual void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _eventSystem = EventSystem.current;
        _defaultScale = transform.localScale;
        _originalColor = _image.color;
        _initialSprite = _image.sprite;

        DetermineSpriteMode();
        ApplyState(SpriteState.Normal);

        if (_spriteMode == SpriteMode.NoneProvided)
        {
            _normalSprite = _hoveredSprite = _clickedSprite = _initialSprite;
        }
        if (_normalSprite != null)
        {
            _image.sprite = _normalSprite;
        }

        _clickedTargetScaleRelativeTo1 *= _defaultScale.x;
        _hoveredTargetScaleRelativeTo1 *= _defaultScale.x;
    }

    private void DetermineSpriteMode()
    {
        bool hasNormal = _normalSprite != null;
        bool hasHover = _hoveredSprite != null;
        bool hasClicked = _clickedSprite != null;

        int count = (hasNormal ? 1 : 0) + (hasHover ? 1 : 0) + (hasClicked ? 1 : 0);

        _spriteMode = count switch
        {
            0 => SpriteMode.NoneProvided,
            3 => SpriteMode.Full,
            _ => SpriteMode.Partial
        };
    }
    
    public void AddOnClickListener(UnityAction method)
    {
        if (!_button) _button = GetComponent<Button>();
        _button.onClick.AddListener(method);
    }

    public void ShowError()
    {
        _image.DOKill();
        _image.DOColor(Color.red, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(() => _image.color = _originalColor);
    }

    public void LockHover()
    {
        _hoverLocked = true;
        HandleHoverEntering(false);
    }

    public void UnlockHover()
    {
        _hoverLocked = false;
        HandleHoverExiting();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_hoverLocked) HandleHoverEntering();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HandleClickEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_hoverLocked) HandleHoverExiting();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bool isPointerOnCurrentObject = eventData.pointerCurrentRaycast.gameObject == gameObject;
        HandleClickExit(isPointerOnCurrentObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!_hoverLocked) HandleHoverEntering();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!_hoverLocked) HandleHoverExiting();
    }

    protected virtual void HandleClickEnter(Action callback = null)
    {
        if (_addBuffer && _isOnCooldown) return;
        if (_addBuffer)
        {
            StartCooldown();
        }

        ApplyState(SpriteState.Clicked);

        if (_shouldPlayClickedAnimation)
        {
            transform.DOScale(_clickedTargetScaleRelativeTo1, _animationDuration * 0.5f).SetEase(Ease.OutSine).OnComplete(() => callback?.Invoke());
        }
        else callback?.Invoke();
    }

    private void StartCooldown()
    {
        _isOnCooldown = true;
        DOVirtual.DelayedCall(_animationDuration, () => _isOnCooldown = false).SetLink(gameObject);
    }

    protected virtual void HandleHoverEntering(bool shouldPlaySound = true)
    {
        ApplyState(SpriteState.Hovered);

        if (_shouldPlayHoverAnimation)
        {
            transform.DOScale(_hoveredTargetScaleRelativeTo1, _animationDuration).SetEase(Ease.OutSine);
        }
    }

    protected virtual void HandleHoverExiting()
    {
        ApplyState(SpriteState.Normal);

        if (_shouldPlayHoverAnimation)
        {
            transform.DOScale(_defaultScale, _animationDuration).SetEase(Ease.OutSine);
        }
    }

    protected virtual void HandleUISubmit(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!_eventSystem || _eventSystem.currentSelectedGameObject != gameObject) return;

        HandleClickEnter(() => HandleClickExit(true));
        _button.onClick.Invoke();
    }

    protected virtual void HandleClickExit(bool isPointerOnCurrentObject)
    {
        if (isPointerOnCurrentObject)
        {
            HandleHoverEntering(true);
        }
        else
        {
            HandleHoverExiting();
        }
    }

    private void ApplyState(SpriteState state)
    {
        switch (_spriteMode)
        {
            case SpriteMode.NoneProvided:
                return;

            case SpriteMode.Full:
                if (state == SpriteState.Normal && _normalSprite != null)
                {
                    _image.sprite = _normalSprite;
                }
                else if (state == SpriteState.Hovered && _hoveredSprite != null)
                {
                    _image.sprite = _hoveredSprite;
                }
                else if (state == SpriteState.Clicked && _clickedSprite != null)
                {
                    _image.sprite = _clickedSprite;
                }
                SetAlpha(1f);
                break;

            case SpriteMode.Partial:
                if (state == SpriteState.Normal)
                {
                    if (_normalSprite != null) { _image.sprite = _normalSprite; SetAlpha(1f); }
                    else SetAlpha(0f);
                }
                else if (state == SpriteState.Hovered)
                {
                    if (_hoveredSprite != null) { _image.sprite = _hoveredSprite; SetAlpha(1f); }
                    else SetAlpha(0f);
                }
                else if (state == SpriteState.Clicked)
                {
                    if (_clickedSprite != null) { _image.sprite = _clickedSprite; SetAlpha(1f); }
                    else SetAlpha(0f);
                }
                break;
        }
    }

    private void SetAlpha(float a)
    {
        Color c = _image.color;
        c.a = a;
        _image.color = c;
    }
}
