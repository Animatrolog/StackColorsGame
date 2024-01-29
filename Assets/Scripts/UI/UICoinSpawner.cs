using UnityEngine;

public class UICoinSpawner : MonoBehaviour
{
    [SerializeField] private CoinAnimation _coinPrefab;
    [SerializeField] private Transform _target;

    private Camera _camera;
    public static UICoinSpawner Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnUICoin(Vector3 worldPosition)
    {
        if(_camera == null) _camera = GameManager.Instance.Player.CameraMovement.GetComponent<Camera>();
        Vector3 canvasPosition = _camera.WorldToScreenPoint(worldPosition);
        var coin = Instantiate(_coinPrefab, transform);
        coin.transform.position = canvasPosition;
        coin.Animate(_target.position);

    }
}
