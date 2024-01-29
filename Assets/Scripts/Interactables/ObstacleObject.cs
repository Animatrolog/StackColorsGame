using Dreamteck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleObject : MonoBehaviour
{
    [SerializeField] bool _isLethal;

    private List<CollectableTile> _collidedTiles = new();
    private Coroutine _waitCoroutine;
    private LevelPiece _levelPiece;

    private void Awake()
    {
        _levelPiece = GetComponentInParent<LevelPiece>();
    }

    private void OnEnable()
    {
        _levelPiece.OnPlayerNearInSuperMode += HideObstacle;
    }

    private void OnDisable()
    {
        _levelPiece.OnPlayerNearInSuperMode -= HideObstacle;
    }

    private void HideObstacle()
    {
        if (TryGetComponent(out ObstacleDisappear disappear))
            disappear.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isLethal)
        {
            if (other.TryGetComponent(out TileCollector tilecollector))
            {
                if (tilecollector.Player.SuperMode.IsInSuperMode) return;
                tilecollector.CollapseTiles(tilecollector.CollectedTiles);
            }
            else if (other.TryGetComponent(out CollectableTile tileLethal))
            {
                if (tileLethal.WasDropped || tileLethal.Collector == null) return;
                if (tileLethal.Collector.Player.SuperMode.IsInSuperMode) return;
                tileLethal.Collector.CollapseTiles(tileLethal.Collector.CollectedTiles);
            }
            return;
        }
        else if(other.TryGetComponent(out CollectableTile tile))
        {
            if (tile.WasDropped || tile.Collector == null) return;
            if (tile.Collector.Player.SuperMode.IsInSuperMode) return;
            _collidedTiles.Add(tile);
            _waitCoroutine ??= StartCoroutine(WaitForAllCollisionsCoroutine()); 
        }
    }

    private IEnumerator WaitForAllCollisionsCoroutine()
    {
        yield return new WaitForFixedUpdate();
        _collidedTiles[0].Collector.CollapseTiles(_collidedTiles);
    }
}
