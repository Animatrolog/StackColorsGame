using GameAnalyticsSDK;
using UnityEngine;

public class GAManager : MonoBehaviour
{

    private GameState _prevGameState;
    private int _level;

    private void Awake()
    {
        GameAnalytics.Initialize();
    }

    private void OnEnable()
    {
        _prevGameState = GameStateManager.CurrentGameState;
        GameStateManager.OnStateChange += HandleGameStateChange;
    }

    private void OnDisable()
    {
        GameStateManager.OnStateChange -= HandleGameStateChange;
    }

    private void HandleGameStateChange(GameState state)
    {
        switch (state) 
        {
            case GameState.Game:
                _level = GameDataManager.GameData.Level;
                if (_prevGameState == GameState.Defeat) return;
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, _level.ToString());
                Debug.Log("GA Level Start " + _level.ToString());
                break;
            case GameState.Defeat:
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, _level.ToString());
                Debug.Log("GA Level Fail " + _level.ToString());
                break;
            case GameState.Finish:
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, _level.ToString());
                Debug.Log("GA Level Complete " + _level.ToString());
                break;
        }
        _prevGameState = state;
    }
}
