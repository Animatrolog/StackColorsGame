using System.Collections;
using TMPro;
using UnityEngine;

public class TextColorAnimation : MonoBehaviour
{
    [SerializeField] private float _animationSpeed = 1f;
    [SerializeField] private Color _targetColor;
    [SerializeField] private TMP_Text _animatedText;
    [SerializeField] private bool _animateOnStart;

    private Coroutine _animationCoroutine;

    private void OnEnable()
    {
        if (_animateOnStart)
        {
            Animate(true);
            return;
        }
    }

    private void OnDisable()
    {
        _animationCoroutine = null;
    }

    public void Animate(bool start)
    {
        if (start)
            _animationCoroutine ??= StartCoroutine(AnimationCoroutine());
        else if (_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);
    }

    public IEnumerator AnimationCoroutine()
    {
        Color originalColor = _animatedText.color;

        while (true)
        {
            float sin01 = ((1 + Mathf.Sin(Time.time * _animationSpeed)) / 2);
            _animatedText.color = Color.Lerp(originalColor, _targetColor, sin01);
            yield return null;
        }
    }
}
