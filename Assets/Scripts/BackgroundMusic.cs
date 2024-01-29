using System.Collections;
using UnityEngine;
using YG;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _volumeFadeDuration = 1f;
    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] private AudioClip _miniGameMusic;

    public static BackgroundMusic Instance;

    private float _defaultVolume;
    private FinishMiniGame _finishMiniGame;
    private Coroutine _volumeFadeCoroutine;
    private bool _isMusicEnabled;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        _defaultVolume = _audioSource.volume;
        _isMusicEnabled = YandexGame.savesData.IsMusicEnabled;
        _audioSource.volume = _isMusicEnabled ? _defaultVolume : 0;
        GameStateManager.OnStateChange += HandleGameStateChange;
    }

    private void Start()
    {
        _finishMiniGame = FinishMiniGame.Instance;
        _finishMiniGame.OnMiniGameStateChanged += HandleFinishMiniGame;
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        if (!GameDataManager.GameData.IsMusicEnabled) return;
        _audioSource.clip = _backgroundMusic;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    private void OnDisable()
    {
        GameStateManager.OnStateChange -= HandleGameStateChange;
        if (_finishMiniGame != null)
            _finishMiniGame.OnMiniGameStateChanged -= HandleFinishMiniGame;
    }

    private void HandleFinishMiniGame(FinishMiniGame.MiniGameState state)
    {
        if (!GameDataManager.GameData.IsMusicEnabled) return;
        switch (state)
        {
            case FinishMiniGame.MiniGameState.Charging:
                _audioSource.clip = _miniGameMusic;
                _audioSource.loop = false;
                _audioSource.Play();
                break;
            case FinishMiniGame.MiniGameState.SlowMo:
                _audioSource.loop = false;
                FadeVolume(0);
                break;
        }
    }

    private void HandleGameStateChange(GameState gameState)
    {
        if (!GameDataManager.GameData.IsMusicEnabled)
        {
            _audioSource.volume = 0f;
            return;
        }
        switch (gameState) 
        {
            case GameState.Menu:
                FadeVolume(_defaultVolume);
                break;
            case GameState.Game:
                FadeVolume(_defaultVolume);
                if(!_audioSource.isPlaying)
                    PlayBackgroundMusic();
                break;
            case GameState.Defeat:
                FadeVolume(0); ;
                break;
        }
    }

    public void EnableMusic(bool enabled)
    {
        FadeVolume(enabled ? _defaultVolume : 0);
        if(enabled && !_audioSource.isPlaying)
            PlayBackgroundMusic();
    }

    private void FadeVolume(float targetVolume)
    {
        if (_audioSource.volume == targetVolume) return;

        _volumeFadeCoroutine ??= StartCoroutine(VolumeFade(targetVolume));
    }

    private IEnumerator VolumeFade(float targetVolume)
    {
        while(_audioSource.volume != targetVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume,
                targetVolume, Time.deltaTime / _volumeFadeDuration);

            yield return null;
        }
        _volumeFadeCoroutine = null;
    }
}
