using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private int _tilesOnStart;
    [SerializeField] private CollectableTile _tilePrefab;

    private List<CollectableTile> _spawnedTiles = new();
    private Player _player;

    private void OnEnable()
    {
        GameStateManager.OnStateChange += HandleGameStateChange;
    }

    private void OnDisable()
    {
        GameStateManager.OnStateChange -= HandleGameStateChange;
    }

    private void Start()
    {
        SpawnTiles();
    }

    public void SpawnTiles()
    {
        Level level = LevelManager.Instance.CurrentLevelInstance;
        _player = level.PlayerInstance;
        for (int i =0; i < _tilesOnStart; i++)
        {
            var tile = Instantiate(_tilePrefab, level.LevelPieces[0].transform);
            _spawnedTiles.Add(tile);
        }
    }

    private void HandleGameStateChange(GameState state)
    {
        if(state == GameState.Game)
        {
            foreach(var tile in _spawnedTiles)
                _player.TileCollector.AddNewTile(tile);
        }
    }
}
