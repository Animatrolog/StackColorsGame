using UnityEngine;

public class FingerAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _finger;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private float _animationSpeed;
    [SerializeField] private float _distance = 40f;

    private Vector2 _startPosition;

    private void Start()
    {
        _startPosition = _finger.anchoredPosition;
    }

    void Update()
    {
        float progress = Mathf.Sin(Time.time * _animationSpeed);
        Vector2 newPosition = _startPosition;
        newPosition.x = _startPosition.x + _distance * progress;
        _finger.anchoredPosition = newPosition;
    }
}
