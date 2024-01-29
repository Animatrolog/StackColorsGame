using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Player _player;
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _smalllTileCollectedClip;
    [SerializeField][Range(0f,1f)] private float _smalllTileCollectedVolume = 1f;
    [SerializeField] private AudioClip _bigTileCollectedClip;
    [SerializeField][Range(0f, 1f)] private float _bigTileCollectedVolume = 1f;
    [SerializeField] private AudioClip _colorChangeClip;
    [SerializeField][Range(0f, 1f)] private float _colorChangeVolume = 1f;
    [SerializeField] private AudioClip _coinCollectedClip;
    [SerializeField][Range(0f, 1f)] private float _coinCollectedVolume = 1f;
    [SerializeField] private AudioClip _damageClip;
    [SerializeField][Range(0f, 1f)] private float _damageVolume = 1f;

    private void OnEnable()
    {
        _player.TileCollector.OnTileCollected += HandleTileCollected;
        _player.ColorChanger.OnColorChange += HandleColorChange;
        _player.TileCollector.OnWrongTileCollected += HandleDamage;
        GameDataManager.OnCoinAmountChange += HandleCoinCollected;
    }

    private void OnDisable()
    {
        _player.TileCollector.OnTileCollected -= HandleTileCollected;
        _player.ColorChanger.OnColorChange -= HandleColorChange;
        GameDataManager.OnCoinAmountChange -= HandleCoinCollected;
    }

    private void HandleTileCollected(int score)
    {
        if (!GameDataManager.GameData.IsSoundEnabled) return;
        if(score < 5)
            _audioSource.PlayOneShot(_smalllTileCollectedClip, _smalllTileCollectedVolume);
        else
            _audioSource.PlayOneShot(_bigTileCollectedClip,_bigTileCollectedVolume);
    }

    private void HandleColorChange(int colorId)
    {
        if (!GameDataManager.GameData.IsSoundEnabled) return;
        if (GameStateManager.CurrentGameState != GameState.Game) return;
        _audioSource.PlayOneShot(_colorChangeClip, _colorChangeVolume);
    }

    private void HandleCoinCollected(int amount)
    {
        if (!GameDataManager.GameData.IsSoundEnabled) return;
        //_audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.PlayOneShot(_coinCollectedClip, _colorChangeVolume);
    }

    private void HandleDamage()
    {
        if (!GameDataManager.GameData.IsSoundEnabled) return;
        //_audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.PlayOneShot(_damageClip, _damageVolume);
    }
}
