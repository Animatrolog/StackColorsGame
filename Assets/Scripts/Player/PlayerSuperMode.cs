using System;
using System.Collections;
using UnityEngine;

public class PlayerSuperMode : MonoBehaviour
{
    [SerializeField] private int _defaultFullChargePoints = 60;
    [SerializeField] private float _perLevelPercentAddition = 0.01f;
    [SerializeField] private float _superModeDuration;
    [SerializeField] private BoxCollider _coinCollectCollider;

    private Player _player;
    private float _targetSuperCharge = 0f;
    private Coroutine _superModeCoroutine;
    private int _fullChargePointsLevel;

    public Action<float, int> OnChargeChanged;
    public Action<bool> OnSuperMode;

    public bool IsInSuperMode { get; private set; }
    public float CurrentSuperCharge { get; private set; }

    public void Initialize(Player player)
    {
        _player = player;

        _player.TileCollector.OnTileCollected += AddCharge;
        _player.TileCollector.OnWrongTileCollected += ResetCharge;

        int curLevel = GameDataManager.GameData.Level;
        _fullChargePointsLevel = (int)(_defaultFullChargePoints * (1 + (curLevel * _perLevelPercentAddition)));
        Debug.Log("Points for super mode => " + _fullChargePointsLevel);
    }

    private void Start()
    {
        if (LevelManager.Instance.CurrentLevelInstance.IsSuperModeAllowed) return;
        _player.TileCollector.OnTileCollected -= AddCharge;
        _player.TileCollector.OnWrongTileCollected -= ResetCharge;
    }

    private void OnDisable()
    {
        if (_player == null) return;
        _player.TileCollector.OnTileCollected -= AddCharge;
        _player.TileCollector.OnWrongTileCollected -= ResetCharge;
    }

    private void AddCharge(int points)
    {
        if (_superModeCoroutine != null) return;
        _targetSuperCharge += (float)points / _fullChargePointsLevel;
        if (CurrentSuperCharge >= 1f) StartSuperMode();
    }

    public void StartSuperMode()
    {
        CurrentSuperCharge = 1f;
        _superModeCoroutine ??= StartCoroutine(SuperMode());
    }

    private void ResetCharge()
    {
        _targetSuperCharge = 0;
    }

    private void Update()
    {
        if (GameStateManager.CurrentGameState != GameState.Game) return;
        if (CurrentSuperCharge >= 1)
            _superModeCoroutine ??= StartCoroutine(SuperMode());
        if (CurrentSuperCharge != _targetSuperCharge && !IsInSuperMode)
        {
            CurrentSuperCharge = Mathf.Lerp(CurrentSuperCharge, _targetSuperCharge, Time.deltaTime * 10f);
            int colorId = CurrentSuperCharge <= _targetSuperCharge ? 0 : 1;
            OnChargeChanged?.Invoke(CurrentSuperCharge, colorId);
        }
    }

    private IEnumerator SuperMode()
    {
        float progress = 0f;
        OnSuperMode?.Invoke(true);
        IsInSuperMode = true;
        _coinCollectCollider.gameObject.SetActive(true);
        while (progress < 1f)
        {
            progress += Time.deltaTime / _superModeDuration;
            progress = Mathf.Clamp01(progress);
            CurrentSuperCharge = 1f - progress;
            OnChargeChanged?.Invoke(CurrentSuperCharge, 2);
            yield return null;
        }
        _coinCollectCollider.gameObject.SetActive(false);
        _targetSuperCharge = 0f;
        CurrentSuperCharge = 0f;
        _superModeCoroutine = null;
        OnSuperMode?.Invoke(false);
        IsInSuperMode = false;
    }

    public void ResetSuperMode()
    {
        if (_superModeCoroutine != null) StopCoroutine(_superModeCoroutine);
        _targetSuperCharge = 0f;
        CurrentSuperCharge = 0f;
        OnChargeChanged?.Invoke(CurrentSuperCharge, 0);
        OnSuperMode?.Invoke(false);
    }
}
