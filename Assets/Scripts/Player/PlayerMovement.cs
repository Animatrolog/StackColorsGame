using Dreamteck.Splines;
using UnityEngine;
using YG;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float _slideSpeed = 1f;
    [SerializeField] private InputHandler _input;
    [SerializeField] private SplineFollower _splineFollower;
    [SerializeField] private float _accelerationTime = 1f;
    [SerializeField] private Rigidbody _rigidbody;
    [Header("Damage")]
    [SerializeField] private float _damageSpeedMultiplier = 0.25f;
    [Header("SuperMode")]
    [SerializeField] private float _superModeSpeedMultiplyer = 2f;
    [Header("Upgrades")]
    [SerializeField] private NMKSStruct _speedUpgrade;
    [Header("Bonus Level")]
    [SerializeField] private float _bonusLevelSpeedMultiplier = 1.5f;
    
    private float _defaultFollowSpeed = 10f;
    private float _targetDistance;
    private Transform _tileCollectorTransform;
    private Vector3 _offset;
    private Player _player;
    private float _leveledUpSpeed;

    public float TotalTraveledDistance { get; private set; } = 0f;
    public float TraveledSplineDistance { get; private set; } = 0f;

    [HideInInspector] public float TargetSpeed = 0;
    public float DefaultSpeed => _defaultFollowSpeed;
    public float CurrentSpeed { get => _splineFollower.followSpeed; set => _splineFollower.followSpeed = value; }
    public SplineFollower SplineFollower => _splineFollower;
    [HideInInspector] public bool IsFinishMinigameStarted;

    private void OnEnable()
    {
        GameStateManager.OnStateChange += HandleGameStateChange;
        UpgradeManager.Instance.OnUpgrade += HandleUpgrade;
    }

    private void OnDisable()
    {
        GameStateManager.OnStateChange -= HandleGameStateChange;
        UpgradeManager.Instance.OnUpgrade -= HandleUpgrade;
        if (_player == null) return;
        _player.SuperMode.OnSuperMode -= HandeleSuperMode;
        _player.TileCollector.OnWrongTileCollected -= HandeleDamage;
    }

    private void UpdateUpgradeStats()
    {
        _leveledUpSpeed = GetUpgradeValue(YandexGame.savesData.SpeedLevel, _speedUpgrade);
        _defaultFollowSpeed = _speedUpgrade.N * _leveledUpSpeed;
        if (YandexGame.savesData.Level % 5 == 0)
            _defaultFollowSpeed *= _bonusLevelSpeedMultiplier;

        Debug.Log("UPGRADE - Speed on current level => " + _defaultFollowSpeed);
    }

    private void HandleUpgrade(UpgradeType upgrade)
    {
        if (upgrade == UpgradeType.Speed)
        {
            UpdateUpgradeStats();
        }
    }

    private void HandleGameStateChange(GameState state)
    {
        if(state == GameState.Game)
        {
            UpdateUpgradeStats();
            Debug.Log("UPGRADE - Score on current level => " + UpgradeManager.Instance.FinishMultiplier.ToString("F1"));
        }     
    }

    private float GetUpgradeValue(int level, NMKSStruct nMKS)
    {
        float value = 1f;

        for (int i = 0; i < level - 1; i++)
        {
            int multiplier = Mathf.FloorToInt((i + 1f) / nMKS.K);
            float divider = 1f;
            if (multiplier > 0f)
                divider = nMKS.S * multiplier;
            value += value * (nMKS.M / divider);
        }
        return value;
    }


    public void AllowMovement(bool allow)
    {
        TargetSpeed = allow ? _defaultFollowSpeed : 0;
        _input.enabled = allow;
        _splineFollower.follow = allow;
    }

    public void Initialize(Player player)
    {
        _player = player;
        _player.SuperMode.OnSuperMode += HandeleSuperMode;
        var tileCollector = _player.TileCollector;
        tileCollector.OnWrongTileCollected += HandeleDamage;
        _tileCollectorTransform = tileCollector.transform;
    }

    private void Start()
    {
        _targetDistance = transform.localPosition.x;
    }

    private void Update()
    {
        _targetDistance = _input.TargetDistance;
        if (IsFinishMinigameStarted) _targetDistance = 0f;
        Move();
        HandleSpeedChange();

        if (GameStateManager.CurrentGameState != GameState.Game || _splineFollower.spline == null) return;

        TraveledSplineDistance = (float)(_splineFollower.spline.CalculateLength() * _splineFollower.result.percent);
    }

    public void ChangeSpline(SplineComputer spline)
    {
        if (_splineFollower.spline != null) TotalTraveledDistance += _splineFollower.spline.CalculateLength();
        Vector3 newOffset = transform.InverseTransformVector(transform.position - spline.EvaluatePosition(0f));
        float underShoot = newOffset.z;
        newOffset.y = 0f;
        newOffset.z = 0f;
        _offset = transform.rotation * newOffset;
        _splineFollower.spline = spline;
        _splineFollower.RebuildImmediate();
        float excessDistance = (CurrentSpeed * Time.deltaTime) + underShoot;
        _splineFollower.SetPercent(_splineFollower.Travel(0, excessDistance, _splineFollower.direction));
    }

    private void HandleSpeedChange()
    {
        if (CurrentSpeed != TargetSpeed)
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, Time.deltaTime / _accelerationTime);
    }

    protected virtual void Move()
    {
        if (!_splineFollower.follow) return;
        float maxDistance = _splineFollower.result.size / 2f;
        Vector3 inversedPos = transform.InverseTransformPoint(_splineFollower.result.position);
        float maxX = inversedPos.x + maxDistance;
        float minX = inversedPos.x - maxDistance;

        Vector3 targetPosition = _tileCollectorTransform.localPosition;

        _targetDistance = Mathf.Clamp(_targetDistance, minX, maxX);
        
        targetPosition.x = Mathf.Lerp(targetPosition.x, _targetDistance, _slideSpeed * Time.deltaTime);
        _tileCollectorTransform.localPosition = targetPosition;
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(Vector3.Lerp(_rigidbody.position, 
            _splineFollower.result.position + _offset, Time.deltaTime * 10f));
    }

    private void HandeleDamage()
    {
        CurrentSpeed = DefaultSpeed * _damageSpeedMultiplier;
    }

    private void HandeleSuperMode(bool isSupermode)
    {
        TargetSpeed = DefaultSpeed * (isSupermode? _superModeSpeedMultiplyer : 1f);
        if (isSupermode) CurrentSpeed = 4f;
    }
}
