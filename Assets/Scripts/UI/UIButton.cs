using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _coloredImage;
    [SerializeField] private CounterAnimation _animation;
    [SerializeField] private bool _animateNotInteractable = true;
    
    private Color _defaultColor;
    private bool _interactable = true;

    public Image ColoredImage => _coloredImage;
    public CounterAnimation Animation => _animation;

    public bool Interactable
    {
        get => _interactable;
        set
        {
            _interactable = value;
            _coloredImage.color = value ? _defaultColor : Color.gray;
        }
    }

    public Action OnClick;

    private void Awake()
    {
        _defaultColor = _coloredImage.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_animateNotInteractable && !_interactable) return;
        _animation.Animate();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_interactable) return;
        OnClick?.Invoke();
    }
}
