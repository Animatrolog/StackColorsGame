using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinishLineGenerator : MonoBehaviour
{
    [SerializeField] private float _fractionalValue = 0.2f;
    [SerializeField] private RainbowWayPiece _rainbowWayPicePrefab;
    [SerializeField] private RainbowWayPiece _rainbowWayPiceEndPrefab;
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private Transform _container;
    [SerializeField] private float _offset;
    [Header("Sounds")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _reachedClip;

    private List<RainbowWayPiece> _rainbowWayPieces = new();
    private float _highestReachedMultiplyer;

    public RainbowWayPiece FinalReachedPiece { get; private set; }

    private void OnEnable()
    {
        GameStateManager.OnStateChange += HandleStateChange; 
    }

    private void OnDisable()
    {
        GameStateManager.OnStateChange -= HandleStateChange;
    }

    private void HandleStateChange(GameState state)
    {
        if (state == GameState.Game)
        {
            float multimplyer = UpgradeManager.Instance.FinishMultiplier;
            GenerateRainbowWay(multimplyer);
        }
    }

    public void GenerateRainbowWay(float maxMultiplyer)
    {
        int length = Mathf.RoundToInt( 1 + (maxMultiplyer - 1f) * (1f / _fractionalValue));

        float colorStep = 1f / length;

        Vector3 newCenter = _collider.center;
        newCenter.z = (_offset * length) / 2f;
        newCenter.z -= _offset / 2f;
        Vector3 newSize = _collider.size;
        newSize.z = (_offset * length);
        _collider.center = newCenter;
        _collider.size = newSize;
        //_collider.size = 

        for (int i = 0; i < length; i++)
        {
            var piece = Instantiate(_rainbowWayPicePrefab, _container.position + 
                (_container.forward * _offset * i), _container.rotation);
            piece.transform.parent = _container;
            Color color = Color.HSVToRGB(i * colorStep, 1.0f, 1.0f);
            piece.Initialize(this, (1 + i * _fractionalValue), color);
            _rainbowWayPieces.Add(piece);
        }
        FinalReachedPiece = _rainbowWayPieces[0];
        RainbowWayPiece lastPiece = Instantiate(_rainbowWayPiceEndPrefab, _container.position + 
            (_container.forward * _offset * (length + 1)), _container.rotation);
        lastPiece.transform.parent = _container;
        lastPiece.Initialize(this, -1, Color.black);
    }

    public void AddReachedPiece(RainbowWayPiece reachedPiece)
    {
        float multiplyer = reachedPiece.Multiplyer;
        if (multiplyer > _highestReachedMultiplyer)
        {
            GameManager.Instance.Player.CameraMovement.SetFollowTarget(reachedPiece.CameraPoint);
            _highestReachedMultiplyer = reachedPiece.Multiplyer;
            FinalReachedPiece = reachedPiece;
            if(GameDataManager.GameData.IsSoundEnabled)
                _audioSource.PlayOneShot(_reachedClip);
        }
    }
}
