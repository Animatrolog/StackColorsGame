using UnityEngine;

public class TileCollectorSuperModeScaler : MonoBehaviour
{
    [SerializeField] private Transform _collectorMesh;
    [SerializeField] private BoxCollider _collectorCollider;
    [SerializeField] private Player _player;
    [SerializeField] private float _supermodeXScale = 2f;

    private Vector3 _meshOriginalScale;
    private Vector3 _colliderOriginalScale;

    private void Awake()
    {
        _meshOriginalScale = _collectorMesh.localScale;
        _colliderOriginalScale = _collectorCollider.size;
    }

    private void OnEnable()
    {
        _player.SuperMode.OnSuperMode += HandleSuperMode;
    }

    private void OnDisable()
    {
        _player.SuperMode.OnSuperMode -= HandleSuperMode;
    }

    private void HandleSuperMode(bool isSupermode)
    {
        if (isSupermode)
        {
            Vector3 newMeshScale = _collectorMesh.localScale;
            newMeshScale.x *= _supermodeXScale;
            _collectorMesh.localScale = newMeshScale;

            Vector3 newColliderScale = _collectorCollider.size;
            newColliderScale.x *= _supermodeXScale;
            _collectorCollider.size = newColliderScale;
        }
        else
        {
            _collectorMesh.localScale = _meshOriginalScale;
            _collectorCollider.size = _colliderOriginalScale;
        }
    }
}
