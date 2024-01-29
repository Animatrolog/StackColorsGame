using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Transform _playerFollowPoint;
    [SerializeField] private Transform _playerFollowHighPoint;
    [SerializeField] private Transform _sideViewPoint;
    [SerializeField] private Transform _closeViewPoint;
    [SerializeField] private int _tilesForHigestCameraPoint = 100;

    private Transform _objectToFollow;
    private Transform _objectToLook;
    private Vector3 _lookOffset = Vector3.up * 1.7f;
    private TileCollector _tileCollector;

    public float Speed = 8f;
    public Vector3 Offset = Vector3.zero;
    public Quaternion TargetRotation;
    public CameraBehaviour CurrentBehaviour;

    private void OnEnable()
    {
        _tileCollector = _player.TileCollector;
    }

    private void Start()
    {
        transform.parent = _player.transform.parent;
    }

    void LateUpdate()
    {
        switch (CurrentBehaviour)
        {
            case CameraBehaviour.FollowPlayer:
                FollowPlayer();
                break;
            case CameraBehaviour.FollowCinematic:
                FollowCinematic();
                break;
            case CameraBehaviour.FollowWorld:
                FollowTargetWorld();
                break;
        }
    }

    private void FollowPlayer()
    {
        float tileFactor = _player.SuperMode.IsInSuperMode? 0: Mathf.Clamp01((float)_tileCollector.CollectedTiles.Count / _tilesForHigestCameraPoint);      
        float lerpTime = Time.deltaTime * Speed;

        Vector3 targetPosition = Vector3.Lerp(_playerFollowPoint.position, _playerFollowHighPoint.position, tileFactor);
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpTime);
        Quaternion targetRotation = Quaternion.Lerp(_playerFollowPoint.rotation, _playerFollowHighPoint.rotation, tileFactor);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpTime);
    }

    private void FollowCinematic()
    {
        if (_objectToFollow == null) return;

        Vector3 targetPosition = _objectToFollow.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * Speed);
        Quaternion targetRotation = Quaternion.LookRotation(_objectToLook.position + _lookOffset - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * Speed);
    }

    private void FollowTargetWorld()
    {
        if (_objectToFollow == null) return;
        Vector3 targetPosition = _objectToFollow.position + (transform.rotation * Offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * Speed);
        transform.rotation = Quaternion.Lerp(transform.rotation, _objectToFollow.rotation, Time.deltaTime * Speed);
    }

    public void SetFollowTarget(Transform target)
    {
        Speed = 1f;
        CurrentBehaviour = CameraBehaviour.FollowWorld;
        _objectToFollow = target;
    }

    public void StartFollowPlayer()
    {
        CurrentBehaviour = CameraBehaviour.FollowPlayer;
        _objectToFollow = _playerFollowPoint;
        Speed = _player.Movement.DefaultSpeed * 0.8f;
    }

    public void SwitchToSideView()
    {
        CurrentBehaviour = CameraBehaviour.FollowCinematic;
        _objectToFollow = _sideViewPoint;
        _objectToLook = _player.TileCollector.transform;
        Speed = 20f;
    }

    public void SwitchToCloseView()
    {
        CurrentBehaviour = CameraBehaviour.FollowCinematic;
        _objectToFollow = _closeViewPoint;
        _objectToLook = _player.TileCollector.transform;
        Speed = 3.25f;
    }

}

public enum CameraBehaviour
{
    FollowPlayer,
    FollowCinematic,
    FollowWorld,
}