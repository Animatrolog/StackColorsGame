using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnimation : MonoBehaviour
{
    [SerializeField] private float _fadeInDuration = 1f;
    [SerializeField] private float _fadeOutDuration = 1f;
    [SerializeField] private Image _image;

    private Coroutine _animationCoroutine;
    private Player _player;

    private void OnEnable()
    {
        _player = GameManager.Instance.Player;
        _player.TileCollector.OnWrongTileCollected += Animate;
        _player.SuperMode.OnSuperMode += HandleSuperMode;
    }

    private void OnDisable()
    {
        GameManager.Instance.Player.TileCollector.OnWrongTileCollected -= Animate;
    }

    public void Animate()
    {
        if(_animationCoroutine == null)
            _animationCoroutine = StartCoroutine(AnimationCoroutine());
        else
        {
            StopCoroutine(_animationCoroutine);
            _animationCoroutine = StartCoroutine(AnimationCoroutine());
        }
    }

    private void HandleSuperMode(bool isSuperMode)
    {
        if(isSuperMode)
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }

            Color newColor = _image.color;
            newColor.a = 0f;
            _image.color = newColor;   
        }
    }

    public IEnumerator AnimationCoroutine()
    {
        Color newColor = _image.color;

        while (newColor.a < 1f)
        {
            newColor.a += Time.deltaTime / _fadeInDuration;
            newColor.a = Mathf.Clamp01(newColor.a);
            _image.color = newColor;
            yield return null;
        }

        while (newColor.a > 0)
        {
            newColor.a -= Time.deltaTime / _fadeOutDuration;
            newColor.a = Mathf.Clamp01(newColor.a);
            _image.color = newColor;
            yield return null;
        }

        _animationCoroutine = null;
    }
}
