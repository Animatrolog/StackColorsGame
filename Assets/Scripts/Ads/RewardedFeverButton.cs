using UnityEngine;
using UnityEngine.UI;

public class RewardedFeverButton : MonoBehaviour
{
    [SerializeField] private UIButton _button;

    private void OnEnable()
    {
        _button.OnClick += HandleButtonClock;
    }

    private void OnDisable()
    {
        _button.OnClick -= HandleButtonClock;
    }

    private void Start()
    {
        if(!LevelManager.Instance.CurrentLevelInstance.IsSuperModeAllowed)
            gameObject.SetActive(false);
    }

    private void HandleButtonClock()
    {
        RewardedAd.ShowAd(HandleAdRewad);
    }

    private void HandleAdRewad()
    {
        GameStateManager.Instance.SetState(GameState.Game);
        GameManager.Instance.Player.SuperMode.StartSuperMode();
    }
}
