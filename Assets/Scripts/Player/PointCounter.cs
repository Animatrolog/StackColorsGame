using System;
using UnityEngine;

public class PointCounter : MonoBehaviour
{
    [Header("Features")]
    [SerializeField] private bool _substractPointsOnTileDrop;

    private Player _player;
    
    public int Points {  get; private set; }
    public Action<int> OnPointCountChange;
    public Action<int> OnNewHighscore;

    public void Initialize(Player player)
    {
        _player = player;
        _player.TileCollector.OnTileCollected += AddPoints;
        _player.TileCollector.OnTileDropped += SubstractPoints;
    }

    private void OnDisable()
    {
        if(_player == null) return;
        _player.TileCollector.OnTileCollected -= AddPoints;
        _player.TileCollector.OnTileDropped -= SubstractPoints;
    }

    public void AddPoints(int points)
    {
        Points += points;
        OnPointCountChange?.Invoke(Points);
    }

    public void SubstractPoints(int points)
    {
        if (!_substractPointsOnTileDrop) return;
        Points -= points;
        OnPointCountChange?.Invoke(Points);
    }

    public void ConvertPointsToCoins()
    {
        if (GameDataManager.GameData.Highscore < Points)
            OnNewHighscore?.Invoke(Points);
        GameDataManager.AddCoins(Points);
    }
}
