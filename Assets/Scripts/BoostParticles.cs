using UnityEngine;

public class BoostParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _boostParticles;
    [SerializeField] private Player _player;

    private FinishMiniGame _finishMiniGame;

    private void OnEnable()
    {
        _player.SuperMode.OnSuperMode += HandleSuperMode;
        GameStateManager.OnStateChange += HandleGameStateChange;
    }

    private void Start()
    {
        _finishMiniGame = FinishMiniGame.Instance;
        _finishMiniGame.OnMiniGameStateChanged += HandleFinishMinigame;
        GameStateManager.OnStateChange -= HandleGameStateChange;
    }

    private void OnDisable()
    {
        if(_finishMiniGame != null)
            _finishMiniGame.OnMiniGameStateChanged += HandleFinishMinigame;
        _player.SuperMode.OnSuperMode -= HandleSuperMode;
    }

    private void HandleSuperMode(bool isSuperMode)
    {
        var main = _boostParticles.main;
        main.loop = isSuperMode;

        if (isSuperMode && !_boostParticles.isPlaying)
        {
            main.loop = true;
            _boostParticles.Play();
        }
        else
        {
            main.loop = false;
            _boostParticles.Stop();
        }
    }

    private void HandleFinishMinigame(FinishMiniGame.MiniGameState state)
    {
        var main = _boostParticles.main;
        switch (state)
        {
            case FinishMiniGame.MiniGameState.Charging:
                main.loop = true;
                _boostParticles.Play();
                break;
            case FinishMiniGame.MiniGameState.SlowMo:
                main.loop = false;
                _boostParticles.Stop();
                break;
        }
    }

    private void HandleGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Defeat:
                if (_boostParticles.isPlaying) _boostParticles.Stop();
                break;
        }
    }
}
