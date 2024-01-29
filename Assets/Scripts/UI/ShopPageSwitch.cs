using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopPageSwitch : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private ImageSlide _imageSlide;
    [SerializeField] private Button _basicButton;
    [SerializeField] private Button _epicButton;

    private int _currentPage;
    private bool _isDragged;

    public int CurrentPage => _currentPage;
    public Action<int> OnPageChanged;

    private void OnEnable()
    {
        _basicButton.onClick.AddListener(HandleBasicClick);
        _epicButton.onClick.AddListener(HandleEpicClick);
    }

    private void OnDisable()
    {
        _basicButton.onClick.RemoveListener(HandleBasicClick);
        _epicButton.onClick.RemoveListener(HandleEpicClick);
    }

    private void HandleBasicClick()
    {
        SwitchPage(0);
    }

    private void HandleEpicClick()
    {
        SwitchPage(1);
    }

    public void SwitchPage(int pageId)
    {
        _currentPage = pageId;
        OnPageChanged?.Invoke(pageId);
    }

    private void Update()
    {
        float curPosition = _scrollRect.horizontalNormalizedPosition;
        _imageSlide.SetNormalizedPosition(curPosition);
        if(_isDragged) return;
        _scrollRect.horizontalNormalizedPosition = Mathf.Lerp(curPosition, _currentPage, Time.deltaTime * 8f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragged = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragged = false;
        float curPosition = _scrollRect.horizontalNormalizedPosition;
        curPosition = Mathf.Clamp(curPosition, -0.4f, 1.4f);
        int newPage = (int)Mathf.Round(curPosition);
        if (newPage == _currentPage) return;
        SwitchPage(newPage);
    }
}
