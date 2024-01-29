using UnityEngine;

public class ImageSlide : MonoBehaviour
{
    private RectTransform _rectTransform;
    private float _maxPosition;
    private Vector3 _originalPosition;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _maxPosition = _rectTransform.parent.GetComponent<RectTransform>().sizeDelta.x - _rectTransform.sizeDelta.x;
        _originalPosition = _rectTransform.localPosition;
    }

    public void SetNormalizedPosition(float position)
    {
        position = Mathf.Clamp01(position);
        Vector2 newPos = _rectTransform.localPosition;
        newPos.x = _originalPosition.x + (position * _maxPosition);
        _rectTransform.localPosition = newPos;
    }
}
