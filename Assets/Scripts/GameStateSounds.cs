using UnityEngine;

public class GameStateSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [Header("GameStateCips")]
    [SerializeField] private AudioClip _startClip;
    [SerializeField][Range(0f, 1f)] private float _startVolume = 1f;
    [SerializeField] private AudioClip _defeatClip;
    [SerializeField][Range(0f, 1f)] private float _defeatVolume = 1f;
    [SerializeField] private AudioClip _finishClip;
    [SerializeField][Range(0f, 1f)] private float _finishVolume = 1f;
    [SerializeField] private AudioClip _highscoreClip;
    [SerializeField][Range(0f, 1f)] private float _highscoreVolume = 1f;

    private void OnEnable()
    {
        GameStateManager.OnStateChange += HandleGameStateChange;
    }

    private void OnDisable()
    {
        GameStateManager.OnStateChange -= HandleGameStateChange;
    }

    private void HandleGameStateChange(GameState state)
    {
        if (!GameDataManager.GameData.IsSoundEnabled) return;
        switch (state) 
        {
            case GameState.Game:
                _audioSource.PlayOneShot(_startClip, _startVolume);
                break;
            case GameState.Defeat:
                _audioSource.PlayOneShot(_defeatClip, _defeatVolume);
                break;
            case GameState.Finish:
                if(GameManager.Instance.FinalLevelScore > GameDataManager.GameData.Highscore)
                    _audioSource.PlayOneShot(_highscoreClip, _highscoreVolume);
                else _audioSource.PlayOneShot(_finishClip, _finishVolume);
                break;
        }
    }
}
