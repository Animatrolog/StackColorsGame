using System.Collections;
using UnityEngine;

public class SineSizeAnimation : MonoBehaviour
{
    [SerializeField] private float _animationSpeed = 1f;
    [SerializeField] private float _targetScale;
    [SerializeField] private Transform _animatedObject;
    [SerializeField] private bool _animateOnStart;

    private Coroutine _animationCoroutine;
    private PlayerSuperMode _superMode;

    private void OnEnable()
    {
        if (_animateOnStart)
        {
            Animate(true);
            return;
        }
        _superMode = GameManager.Instance.Player.SuperMode;
        _superMode.OnSuperMode += Animate;
    }

    private void OnDisable()
    {
        _animationCoroutine = null;
        if (_animateOnStart) return;
        _superMode.OnSuperMode -= Animate;
    }

    public void Animate(bool start)
    {
        _animatedObject.gameObject.SetActive(start);

        if (start)
            _animationCoroutine ??= StartCoroutine(AnimationCoroutine());
        else if(_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);
    }

    public IEnumerator AnimationCoroutine()
    {
        Vector3 originalScale = _animatedObject.localScale;
        Vector3 scaleDiff = (Vector3.one * _targetScale) - originalScale;

        while (true)
        {
            _animatedObject.localScale = originalScale + (scaleDiff * ((1 + Mathf.Sin(Time.time * _animationSpeed)) / 2));
            yield return null;
        }
    }
}
