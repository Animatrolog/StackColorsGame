using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorChanger : MonoBehaviour
{
    [SerializeField] private int _defaultColorIndex = 0;
    [SerializeField] private List<MeshRenderer> _staticMeshes;
    [SerializeField] private PlayerSkinChanger _skinChanger;
    [SerializeField] private ParticleSystem _boostParticles;

    private SkinnedMeshRenderer _skinnedMesh;
    private TileCollector _tileCollector;
    public Action<int> OnColorChange;

    public int ColorIndex { get; private set; }

    public void Initialize(Player player)
    {
        _tileCollector = player.TileCollector;
    }

    private void OnEnable()
    {
        _skinChanger.OnSkinChanged += HandleSkinChange;
    }

    private void OnDisable()
    {
        _skinChanger.OnSkinChanged += HandleSkinChange;
    }

    private void Start()
    {
        _defaultColorIndex = LevelManager.Instance.CurrentLevelInstance.LevelPieces[0].LastGateColorId;
    }

    private void HandleSkinChange(int index)
    {
        _skinnedMesh = _skinChanger.CurrentSkinInstance.BodyRenderer;
        SetColor(_defaultColorIndex);
    }

    public void SetColor(int colorIndex)
    {
        ColorIndex = colorIndex;
        Material material = LevelManager.Instance.ColorMaterials.Materials[colorIndex];
        foreach (var renderer in _staticMeshes)
            renderer.sharedMaterial = material;

        if (_skinnedMesh != null)
        {
            _skinnedMesh.sharedMaterial = material;
        }

        if (_boostParticles != null)
        {
            var main = _boostParticles.main;
            main.startColor = material.color;
        }

        if (_tileCollector == null) return;

        foreach(var tile in _tileCollector.CollectedTiles)
        {
            tile.ColorChanger.SetColor(colorIndex);
        }
        OnColorChange?.Invoke(colorIndex);
    }
}
