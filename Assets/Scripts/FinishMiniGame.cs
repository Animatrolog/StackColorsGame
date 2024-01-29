using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class FinishMiniGame : MonoBehaviour
{
    [SerializeField] private FinishMiniGameUI _ui;
    [SerializeField] private float _finishRunSpeedMultiplier = 1f;
    [SerializeField] private float _finishChargeSpeedMultiplier = 1f;
    [SerializeField] private float _lastStagePlayerSpeed = 10f;
    [Header("Upgrades")]
    [SerializeField] private NMKSStruct _kickForceUpgrade;
    
    private FinishLineGenerator _generator;
    private Coroutine _miniGameCoroutine;
    private Player _player;

    public Action<MiniGameState> OnMiniGameStateChanged;
    public static FinishMiniGame Instance;
    public MiniGameState CurrentState {  get; private set; }
    [HideInInspector] public List<CollectableTile> DroppedTiles = new();

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        UpgradeManager.Instance.OnUpgrade += HandleUpgrade;
        GameStateManager.OnStateChange += HandleGameStateChange;
    }

    private void OnDisable()
    {
        UpgradeManager.Instance.OnUpgrade -= HandleUpgrade;
        GameStateManager.OnStateChange -= HandleGameStateChange;
    }

    private void HandleUpgrade(UpgradeType upgrade)
    {
        if(upgrade == UpgradeType.Kick)
        {
            UpdateUpgradeStats();
        }
    }

    private void HandleGameStateChange(GameState state)
    {
        if (state == GameState.Game)
        {
            UpdateUpgradeStats();
        }
    }

    private void UpdateUpgradeStats()
    {
        float kickLevelForce = _kickForceUpgrade.N * GetUpgradeValue(YandexGame.savesData.KickLevel, _kickForceUpgrade);
        Debug.Log("UPGRADE - Kick force on current level => " + kickLevelForce);
    }

    public void StartMiniGame(FinishLineGenerator generator)
    {
        if (_miniGameCoroutine != null) return;
        _player = GameManager.Instance.Player;
        if (!_player.CheckMinigameAllowed()) return;
        _generator = generator;
        SetState(MiniGameState.Charging);
    }

    private IEnumerator MiniGameCoroutine()
    {
        float currentCharge = 0.2f;
        
        _ui.gameObject.SetActive(true);

        while (CurrentState == MiniGameState.Charging)
        {
            if (Input.GetMouseButtonDown(0))
                currentCharge += 0.12f;
            else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                currentCharge += 0.12f;

            if (Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Began)
                currentCharge += 0.12f;

            if (currentCharge > 0.2f) currentCharge -= Time.deltaTime * 0.25f;
            currentCharge = Mathf.Clamp(currentCharge, 0.2f, 1f);
            _ui.UpdateSlider(currentCharge);
            float acceleration = (float)_player.Movement.SplineFollower.GetPercent();
            _player.Movement.TargetSpeed = 0.2f;
            _player.Movement.CurrentSpeed = _lastStagePlayerSpeed * 
                (0.2f + (acceleration * _finishRunSpeedMultiplier) + (currentCharge * _finishChargeSpeedMultiplier));
            yield return null;
        }

        _player.Movement.TargetSpeed = _lastStagePlayerSpeed;
        _player.Movement.CurrentSpeed = _lastStagePlayerSpeed;

        _player.Animation.StartKickAnimation();
        while (CurrentState == MiniGameState.SlowMo)
        {
            yield return null;
        }

        float kickLevelForce = GetUpgradeValue(YandexGame.savesData.KickLevel, _kickForceUpgrade);
        _player.TileCollector.FinishDrop(this, currentCharge * _kickForceUpgrade.N * kickLevelForce);
        yield return new WaitForSeconds(3f);

        while (CurrentState == MiniGameState.Counting)
        {
            int movingTiles = DroppedTiles.Count;

            foreach (var tile in DroppedTiles)
            {
                Vector3 tileVelocity = tile.GetComponent<Rigidbody>().velocity;
                tileVelocity.y = 0f;
                if (tile.transform.position.y < -0.2f)
                {
                    DroppedTiles.Remove(tile);
                    Destroy(tile.gameObject);
                    break;
                }
                if (tileVelocity.magnitude < 1f) movingTiles--;
                else break;
            }

            float multiplyer = 1f;

            if (movingTiles <= 0)
            {
                yield return new WaitForSeconds(1f);

                multiplyer = _generator.FinalReachedPiece.Multiplyer;
                var pointCounter = _player.PointCounter;

                pointCounter.AddPoints((int)(pointCounter.Points * (multiplyer - 1)));
                pointCounter.ConvertPointsToCoins();

                _generator.FinalReachedPiece.Blink();
                _ui.ShowMultiplyerText(multiplyer);
                yield return new WaitForSeconds(2f);
                SetState(MiniGameState.Finish);
                yield break;
            }
            yield return null;
        }
    }

    private float GetUpgradeValue(int level, NMKSStruct nMKS)
    {
        float value = 1f;

        for (int i = 0; i < level - 1; i++)
        {
            float multiplier = Mathf.Floor((float)i / nMKS.K);
            float divider = 1f;
            if (multiplier > 0f)
                divider = nMKS.S * multiplier;
            value += value * (nMKS.M / divider);
        }
        return value;
    }

    public void SetState(MiniGameState state)
    {
        OnMiniGameStateChanged?.Invoke(state);
        var playerCamera = _player.CameraMovement;
        CurrentState = state;
        switch (state) 
        {
            case MiniGameState.Charging:
                GameManager.Instance.Player.Movement.IsFinishMinigameStarted = true;
                _ui.ShowPowerSlider(true);
                playerCamera.SwitchToCloseView();
                _miniGameCoroutine = StartCoroutine(MiniGameCoroutine());
                break;

            case MiniGameState.SlowMo:
                playerCamera.SwitchToSideView();
                Time.timeScale = 0.4f;
                _ui.ShowPowerSlider(false);
                break;

            case MiniGameState.Counting:
                _player.Animation.StartIdleAnimation();
                _player.Movement.AllowMovement(false);
                Time.timeScale = 1f;
                break;

            case MiniGameState.Finish:
                _ui.gameObject.SetActive(false);
                GameManager.Instance.FinishState();
                break;
        }
    }

    public enum MiniGameState
    {
        Charging,
        SlowMo,
        Counting,
        Finish
    }
}
