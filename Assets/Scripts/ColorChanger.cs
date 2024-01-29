using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField][Range(0, 3)] private int _defaultColorIndex;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private MeshRenderer _onlyColorChangeMesh;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private ColorMaterialsSet _colorMaterialsSet;
    [SerializeField] private bool _alwaysPlayersColor = false;

    private LevelPiece _levelPiece;
    private bool _subscribed;

    public bool AlwaysPlayersColor => _alwaysPlayersColor;
    public int ColorIndex { get; private set; }

    private void Awake()
    {
        ColorIndex = _defaultColorIndex;
        _levelPiece = GetComponentInParent<LevelPiece>();
    }

    private void OnEnable()
    {
        _levelPiece.OnPlayerNearInSuperMode += SetColorToPlayerColor;
        _subscribed = true;
    }

    private void OnDisable()
    {
        if (!_subscribed) return;
        UnsubscribeFromEvents();
        //GameManager.Instance.Player.ColorChanger.OnColorChange -= SetColor;
    }

    public void UnsubscribeFromEvents()
    {
        if (_alwaysPlayersColor)
        {
            _levelPiece.OnLastGateColorChanged -= SetColor;
            _alwaysPlayersColor = false;
        }
        _levelPiece.OnPlayerNearInSuperMode -= SetColorToPlayerColor;
        _subscribed = false;
    }

    public void SetColorToPlayerColor()
    {
        int colorIndex = _levelPiece.LastGateColorId; //GameManager.Instance.Player.ColorChanger.ColorIndex;
        SetColor(colorIndex);
    }

    private void Start()
    {
        SetColor(_defaultColorIndex);


        if (_alwaysPlayersColor)
        {
            //GameManager.Instance.Player.ColorChanger.OnColorChange += SetColor;
            _levelPiece.OnLastGateColorChanged += SetColor;
            int colorIndex = _levelPiece.LastGateColorId; //GameManager.Instance.Player.ColorChanger.ColorIndex;

            SetColor(colorIndex);
        }
    }

    public void SetColor(int colorIndex)
    {
        ColorIndex = colorIndex;
        _mesh.sharedMaterial = LevelManager.Instance.ColorMaterials.Materials[colorIndex];
        if (_onlyColorChangeMesh != null) 
        { 
            Color newColor = _mesh.material.color;
            newColor.a = _onlyColorChangeMesh.material.color.a;
            _onlyColorChangeMesh.material.color = newColor;
        }

        if(_particleSystem != null)
        {
            var main = _particleSystem.main;
            main.startColor = _mesh.material.color;
        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        _mesh.material = _colorMaterialsSet.Materials[_defaultColorIndex];
    }
#endif
}
