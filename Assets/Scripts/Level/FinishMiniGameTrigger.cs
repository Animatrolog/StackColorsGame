using UnityEngine;

public class FinishMiniGameTrigger : MonoBehaviour
{
    [SerializeField] private FinishLineGenerator _generator;
    [SerializeField] private GameObject _blcok;

    public void TriggerFinishMinigame() //Выполняется из триггера сплайна 1
    {
        FinishMiniGame.Instance.StartMiniGame(_generator);
    }

    public void SecondStage() //Выполняется из триггера сплайна 2
    {
        FinishMiniGame.Instance.SetState(FinishMiniGame.MiniGameState.SlowMo);
        _blcok.SetActive(false);
    }

    public void LastStage() //Выполняется из триггера сплайна 3
    {
        FinishMiniGame.Instance.SetState(FinishMiniGame.MiniGameState.Counting);
    }
}
