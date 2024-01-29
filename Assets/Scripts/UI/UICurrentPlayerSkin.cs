using UnityEngine;
using UnityEngine.UI;

public class UICurrentPlayerSkin : MonoBehaviour
{
    [SerializeField] private ActiveSkinsPool _activeSkinsPool;
    [SerializeField] private Transform _shinHolder;
    [SerializeField] private int _renderLayerIndex = 7;
    [SerializeField] private Camera _camera;
    [SerializeField] private RawImage _image;
    [SerializeField] private Vector2Int _renderResolution = Vector2Int.one * 400;
    [SerializeField] private Color _skinColor = Color.red;

    private RenderTexture _renderTexture;
    private PlayerSkinModel _currentSkinInstance;

    public void ChangeSkinInstance(int index)
    {
        if(_currentSkinInstance != null) 
            Destroy(_currentSkinInstance.gameObject);

        var skin = _activeSkinsPool.ActiveSkins[index];
        _currentSkinInstance = Instantiate(skin, _shinHolder);
        _currentSkinInstance.BodyRenderer.material.color = _skinColor;
        _currentSkinInstance.AnimatonRig.weight = 0f;
        _currentSkinInstance.gameObject.layer = _renderLayerIndex;
        foreach (Transform child in _currentSkinInstance.transform)
            child.gameObject.layer = _renderLayerIndex;
    }

    private void Start()
    {
        _renderTexture = new RenderTexture(_renderResolution.x, _renderResolution.y, 16);
        _camera.targetTexture = _renderTexture;
        _image.texture = _renderTexture;
    }

    private void Update()
    {
        _camera.Render();
    }
}
