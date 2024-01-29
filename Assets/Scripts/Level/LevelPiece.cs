using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelPiece : MonoBehaviour
{
    [SerializeField] private SplineComputer _mainSpline;
    [SerializeField] private List<SplineComputer> _splines;
    [SerializeField] private float _shupermodeReactionDistance = 100f;

    private Player _player;
    private Coroutine _superModeCheckCoroutine;
    private LevelPiece _prevPiece;

    public bool isPlayerOnThisPeace;
    public Action OnPlayerNearInSuperMode;
    public SplineComputer MainSpline => _mainSpline;
    public int LastGateColorId = 0;

    public Action<int> OnLastGateColorChanged;

    public void SetLastGateColor(int newId)
    {
        LastGateColorId = newId;
        OnLastGateColorChanged?.Invoke(newId);
    }

    public void Initialize(Player player)
    {
        _player = player;
        _player.SuperMode.OnSuperMode += HandleSuperMode;
        Level level = transform.parent.GetComponent<Level>();

        var gate = FindGateInChild();

        if(gate != null)
        {
            LastGateColorId = gate.ColorChanger.ColorIndex;
        }
        else if (level.LevelPieces.Count > 0)
        {
            int prevPieceIndex = level.LevelPieces.Count - 1;
            _prevPiece = level.LevelPieces[prevPieceIndex];
            LastGateColorId = _prevPiece.LastGateColorId;
            _prevPiece.OnLastGateColorChanged += SetLastGateColor;
        }
    }

    private ColorGate FindGateInChild()
    {
        for(int i = 0; i < transform.childCount; i++) 
        {
            if(transform.GetChild(i).TryGetComponent(out ColorGate gate))
                return gate;
        }
        return null;
    }

    private void OnDisable()
    {
        if(_prevPiece != null)
            _prevPiece.OnLastGateColorChanged -= SetLastGateColor;
        if (_player  != null)
            _player.SuperMode.OnSuperMode -= HandleSuperMode;
    }

    private void HandleSuperMode(bool isSuperMode)
    {
        if (isSuperMode)
            _superModeCheckCoroutine ??= StartCoroutine(SuperModeCheckCoroutine());
        else if (_superModeCheckCoroutine != null)
        {
            StopCoroutine(_superModeCheckCoroutine);
            _superModeCheckCoroutine = null;
        }
    }

    public SplineComputer GetSpline(Vector3 playerPosition)
    {
        if (_splines.Count < 1) return _mainSpline;
        return GetNearestSpline(playerPosition);
    }

    private IEnumerator SuperModeCheckCoroutine()
    {
        while(true)
        {
            float distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            if (isPlayerOnThisPeace || distanceToPlayer < _shupermodeReactionDistance)
            {
                OnPlayerNearInSuperMode?.Invoke();
                yield break;
            }
            yield return null;
        }
    }

    private SplineComputer GetNearestSpline(Vector3 playerPosition)
    {
        float nearestDistance = float.MaxValue;
        int nearestIndex = 0;

        for(int i = 0; i < _splines.Count; i++)
        {
            Vector3 splinePointPos = _splines[i].EvaluatePosition(0);
            float distanceToSpline = Vector3.Distance(playerPosition,splinePointPos);
            if (distanceToSpline < nearestDistance)
            {
                nearestDistance = distanceToSpline;
                nearestIndex = i;
            }
        }
        return _splines[nearestIndex];
    }
}
