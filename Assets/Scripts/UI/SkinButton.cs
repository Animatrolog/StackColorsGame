using System;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Button _button;
    [SerializeField] private Image _frameImage;
    [SerializeField] private Image _lockedImage;
    [SerializeField] private Color _selectedFrameColor = Color.red;

    private Color _defaultFrameColor;
    public int SkinID {  get; private set; }

    public bool Interactable 
    { 
        get => _button.interactable;
        set
        {
            _button.interactable = value;
            UnlockVisuals(value);
            _lockedImage.gameObject.SetActive(!value);
        }
    }

    public Action<int> OnClick;

    private void Awake()
    {
        _defaultFrameColor = _frameImage.color;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleClick);
    }

    public void SetSelected(bool selected)
    {
        Color currentColor = _selectedFrameColor;
        _frameImage.color = selected ? currentColor : _defaultFrameColor;
    }

    public void UnlockVisuals(bool unlocked)
    {
        _frameImage.gameObject.SetActive(unlocked);
    }

    private void HandleClick()
    {
        OnClick?.Invoke(SkinID);
    }

    public void Initialize(int skinID, PlayerSkinModel skin)
    {
        SkinID = skinID;
        _icon.sprite = skin.SkinSprite;
    }
}
