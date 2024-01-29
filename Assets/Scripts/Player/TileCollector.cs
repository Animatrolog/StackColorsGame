using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollector : MonoBehaviour
{
    [SerializeField] private float _tileGap = 0.05f;
    [Header("Tilt animation")]
    [SerializeField] private float _straighteningTime = 1f;
    [SerializeField] private AnimationCurve _tiltCurve;

    private float _springyX;
    private Vector3 _previousPosition;
    private Vector3 _deltaPos;
    private PlayerColorChanger _colorChanger;
    private Player _player;

    public Action<int> OnTileCollected;
    public Action OnWrongTileCollected;
    public Action<int> OnTileDropped;
    public Action OnAllTilesCollapsed;
    public Player Player => _player;

    public List<CollectableTile> CollectedTiles { get; private set; } = new();
    public List<CollectableTile> DroppedTiles { get; private set; }

    public void Initialize(Player player)
    {
        _player = player;
        _colorChanger = player.ColorChanger;
    }

    private void CalculateDeltaPosition()
    {
        _deltaPos = (transform.position - _previousPosition) / Time.deltaTime;
        _previousPosition = transform.position;
    }

    private readonly float _oneTile = 1f / 120f;
    
    private void LateUpdate()
    {
        _springyX = Mathf.Lerp(_springyX, transform.localPosition.x, Time.deltaTime / _straighteningTime);
        for (int i = 0; i < CollectedTiles.Count; i++)
        {
            Vector3 tileNewPos = CollectedTiles[i].transform.localPosition;
            float t = _tiltCurve.Evaluate((CollectedTiles.Count - 1 - i) * _oneTile);
            tileNewPos.x = Mathf.Lerp(transform.localPosition.x, _springyX, t);
            CollectedTiles[i].transform.localPosition = tileNewPos;
        }
        CalculateDeltaPosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out CollectableTile tile))
        {
            if (tile.Collector != null || CollectedTiles.Contains(tile)) return;
            AddNewTile(tile);
        }
    }

    public void AddNewTile(CollectableTile tile)
    {

        if (tile.ColorChanger.ColorIndex != _colorChanger.ColorIndex)
        {
            if (GameStateManager.CurrentGameState == GameState.Defeat) return;
            RmoveLastTile();
            OnWrongTileCollected?.Invoke();
            return;
        }

        tile.ColorChanger.UnsubscribeFromEvents();

        foreach (var collectedTile in CollectedTiles) 
            collectedTile.transform.localPosition += Vector3.up * (tile.transform.localScale.y + _tileGap);
        tile.transform.parent = transform.parent;
        tile.transform.localPosition = transform.localPosition + Vector3.up * (tile.transform.localScale.y / 2);
        tile.transform.localRotation = Quaternion.identity;
        CollectedTiles.Add(tile);
        tile.SetCollected(this);

        OnTileCollected?.Invoke(tile.ScorePoints);
    }

    public void CollapseTiles(List<CollectableTile> hitedTiles)
    {
        if (GameStateManager.CurrentGameState == GameState.Defeat) return;
        if (CollectedTiles.Count == 0) return;
        DroppedTiles = new List<CollectableTile>();
        int highestIndex = 0;

        foreach (var tile in hitedTiles)
        {
            int tileIndex = CollectedTiles.IndexOf(tile);
            if (tileIndex > highestIndex) highestIndex = tileIndex;
        }

        for (int i = 0; i <= highestIndex; i++)
        {
            if (CollectedTiles.Count == 0) break;
            var firstTile = CollectedTiles[0];
            OnTileDropped?.Invoke(firstTile.ScorePoints);
            firstTile.MakePhysical(_deltaPos);
            DroppedTiles.Add(firstTile);
            CollectedTiles.Remove(firstTile);
        }
        OnWrongTileCollected?.Invoke();
        if (CollectedTiles.Count == 0)
            OnAllTilesCollapsed?.Invoke();
        Debug.Log("Dropped Tiles => " + DroppedTiles.Count);
    }

    public void RestoreDroppedTiles()
    {
        StartCoroutine(RestoreTiles());
    }

    private IEnumerator RestoreTiles()
    {
        for (int i = 0; i < DroppedTiles.Count; i++)
        {
            var tile = DroppedTiles[i];
            tile.MakeStatic();
            tile.ColorChanger.SetColor(_player.ColorChanger.ColorIndex);
            AddNewTile(tile);
            yield return null;
        }
    }

    private void RmoveLastTile()
    {
        if (CollectedTiles.Count == 0)
        {
            _player.Defeat();
            return;
        }

        var lastTile = CollectedTiles[CollectedTiles.Count - 1];

        foreach (var collectedTile in CollectedTiles)
            collectedTile.transform.localPosition -= Vector3.up * (lastTile.transform.localScale.y + _tileGap);
        CollectedTiles.Remove(lastTile);
        Destroy(lastTile.gameObject);
    }

    public void FinishDrop(FinishMiniGame miniGame, float finishDropForce)
    {
        float currentCharge = finishDropForce + (0.2f * CollectedTiles.Count);//внес в эт строчку дополнения (да простит меня программист)
        float chargeDicrement = currentCharge / CollectedTiles.Count;

        while(CollectedTiles.Count > 0)
        {
            var firstTile = CollectedTiles[0];
            firstTile.MakePhysical(firstTile.transform.forward * currentCharge);
            currentCharge -= chargeDicrement;
            CollectedTiles.Remove(firstTile);
            miniGame.DroppedTiles.Add(firstTile);
        }
    }
}
