using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RewardedResurrection : MonoBehaviour
{
    [SerializeField] private int _resurrectionsLimit = 1;
    [SerializeField] private float _duration = 5f;
    [SerializeField] private Button _button;
    [SerializeField] private Slider _slider;

    private int _resurrections = 0;

    private void OnEnable()
    {
        if(_resurrections >= _resurrectionsLimit)
        {
            gameObject.SetActive(false);
            return;
        }
        _button.onClick.AddListener(HandleButtonClick);
        StartCoroutine(TimerCoroutine());
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleButtonClick);
    }

    private void HandleButtonClick()
    {
        RewardedAd.ShowAd(HandleAdRewarded);
    }

    private void HandleAdRewarded()
    {
        _resurrections++;
        var player = GameManager.Instance.Player;
        gameObject.SetActive(false);
        GameStateManager.Instance.SetState(GameState.Game);
        player.SuperMode.StartSuperMode();
        player.TileCollector.RestoreDroppedTiles();
    }

    private IEnumerator TimerCoroutine()
    {
        float progress = 1f;

        while (progress > 0)
        {
            _slider.value = progress;
            progress -= Time.deltaTime / _duration;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
