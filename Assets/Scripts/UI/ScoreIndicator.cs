using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text _counterText;
    [SerializeField] private CounterAnimation _counterAnimation;

    private Coroutine _delayCoroutine;
    private GameManager _gameManager;

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;
        _gameManager.Player.TileCollector.OnTileCollected += UpdateCounter;
    }

    private void OnDisable()
    {
        _gameManager.Player.TileCollector.OnTileCollected -= UpdateCounter;
        _counterText.text = "";
        _delayCoroutine = null;
    }

    private void UpdateCounter(int count)
    {
        _waitProgress = 0f;
        _counterText.text ="+" + count.ToString();
        _counterAnimation.Animate();
        _delayCoroutine ??= StartCoroutine(DelayCoroutine());
    }

    float _waitProgress = 0f;

    private IEnumerator DelayCoroutine()
    {
        while(_waitProgress < 1f)
        {
            _waitProgress += Time.deltaTime;
            yield return null;
        }
        _counterText.text = "";
        _delayCoroutine = null;
    }
}
