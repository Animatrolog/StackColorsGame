using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private TileCollector _tileCollector;
    [SerializeField] private PointCounter _pointCounter;
    [SerializeField] private PlayerColorChanger _colorChanger;
    [SerializeField] private PlayerSuperMode _superMode;
    [SerializeField] private PlayerAnimation _animation;
    [SerializeField] private CameraMovement _cameraMovement;
    [SerializeField] private PlayerSkinChanger _skinChanger;
    [SerializeField] private PlayerSounds _sounds;

    public PlayerMovement Movement => _movement;
    public TileCollector TileCollector => _tileCollector;
    public PointCounter PointCounter => _pointCounter;
    public PlayerColorChanger ColorChanger => _colorChanger;
    public PlayerAnimation Animation => _animation;
    public CameraMovement CameraMovement => _cameraMovement;
    public PlayerSuperMode SuperMode => _superMode;
    public PlayerSkinChanger SkinChanger => _skinChanger;
    public PlayerSounds Sounds => _sounds;

    private void Awake()
    {
        _tileCollector.Initialize(this);
        _superMode.Initialize(this);
        _movement.Initialize(this); 
        _pointCounter.Initialize(this);
        _colorChanger.Initialize(this);
        _animation.Initialize(this);

        _cameraMovement.StartFollowPlayer();
        _cameraMovement.TargetRotation = _cameraMovement.transform.localRotation;
    }

    private void OnEnable()
    {
        _tileCollector.OnAllTilesCollapsed += Defeat;
    }

    private void OnDisable()
    {
        _tileCollector.OnAllTilesCollapsed -= Defeat;
    }

    public bool CheckMinigameAllowed()
    {
        _superMode.ResetSuperMode();
        return true;
    }

    public void Defeat()
    {
        Movement.AllowMovement(false);
        _superMode.ResetSuperMode();
        GameStateManager.Instance.SetState(GameState.Defeat);
    }
}
