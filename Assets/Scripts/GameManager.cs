using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private GameDataManager _gameDataManager;

    public static GameManager Instance;

    public int FinalLevelScore { get; private set; }
    public Player Player {  get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        _levelManager.InstantiateLevel();
        Player = _levelManager.PlayerInstance;
    }

    private void OnEnable()
    {
        GameStateManager.OnStateChange += HangleGameStateChange;
    }

    private void OnDisable()
    {
        GameStateManager.OnStateChange -= HangleGameStateChange;
    }

    public void FinishState()
    {
        FinalLevelScore = Player.PointCounter.Points;
        GameStateManager.Instance.SetState(GameState.Finish);
    }

    private void HangleGameStateChange(GameState state)
    {
        if(state == GameState.Game)
        {
            Player.Movement.AllowMovement(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
