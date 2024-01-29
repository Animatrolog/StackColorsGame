using System.Collections;
using TMPro;
using UnityEngine;

public class TileStreakCounter : MonoBehaviour
{
    [SerializeField] private float _timeForTile = 2.5f;
    [SerializeField] private int _tilesForNice = 4;
    [SerializeField] private int _tilesForGreat = 8;
    [SerializeField] private TMP_Text _streakGradeText;
    [SerializeField] private CounterAnimation _counterAnimation;
    [SerializeField] private float _gradeShowTime = 1f;
    [Header("Localization")]
    [SerializeField] private TranslatedText _niceTranslation;
    [SerializeField] private TranslatedText _perfectTranslation;

    private Player _player;
    private Coroutine _streakCounterCoroutine;
    private float _timer;
    private int _tileCollected;
    private bool _isPlayerInSuperMode;

    private void OnEnable()
    {
        _player = GameManager.Instance.Player;
        _player.TileCollector.OnTileCollected += HandleTileCollected;
        _player.TileCollector.OnWrongTileCollected += BreakStreak;
        _player.SuperMode.OnSuperMode += SetSuperMode;
    }

    private void OnDisable()
    {
        _player.TileCollector.OnTileCollected -= HandleTileCollected;
        _player.TileCollector.OnWrongTileCollected -= BreakStreak;
        _player.SuperMode.OnSuperMode -= SetSuperMode;
    }

    private void HandleTileCollected(int i)
    {
        _timer = _timeForTile / _player.Movement.CurrentSpeed;
        _streakCounterCoroutine ??= StartCoroutine(StreakCounter());
        _tileCollected++;
    }

    private void SetSuperMode(bool isSupermode)
    {
        _isPlayerInSuperMode = isSupermode;
        if (isSupermode) 
        {
            _streakGradeText.text = "";
            _delayCoroutine = null;
        }
    }

    private void BreakStreak()
    {
        if (_streakCounterCoroutine != null)
        {
            StopCoroutine(_streakCounterCoroutine);
            _streakCounterCoroutine = null;
        }    
        
        _tileCollected = 0;
        _timer = 0;
    }

    private IEnumerator StreakCounter()
    {
        while(_timer > 0)
        {
            _timer -= Time.deltaTime;
            yield return null;
        }
        
        if(_tileCollected > _tilesForNice)
        {
            if(_tileCollected > _tilesForGreat)
            {
                ShowGrade(_perfectTranslation.Text);
                ResetTimer();
                yield break;
            }

            ShowGrade(_niceTranslation.Text);
            yield return null;
        }
        ResetTimer();
    }

    private void ResetTimer()
    {
        _tileCollected = 0;
        _timer = 0;
        _streakCounterCoroutine = null;
    }

    private void ShowGrade(string grade)
    {
        if (_isPlayerInSuperMode) return;
        _waitProgress = 0f;
        _streakGradeText.text = grade;
        _counterAnimation.Animate();
        _delayCoroutine ??= StartCoroutine(DelayCoroutine());
    }

    private Coroutine _delayCoroutine;
    float _waitProgress = 0f;

    private IEnumerator DelayCoroutine()
    {
        while (_waitProgress < 1f)
        {
            _waitProgress += Time.deltaTime / _gradeShowTime;
            yield return null;
        }
        _streakGradeText.text = "";
        _delayCoroutine = null;
    }
}
