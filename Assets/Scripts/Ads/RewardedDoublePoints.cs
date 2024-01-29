using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RewardedDoublePoints : MonoBehaviour
{
    [SerializeField] private int _rewardMultiplier = 5;
    [SerializeField] private float _duration = 5f;
    [SerializeField] private Button _button;
    [SerializeField] private Slider _skinSlider;
    [SerializeField] private FinishPanel _finishPanel;
    [SerializeField] private CoinsAnimation _coinsAnimation;

    private void OnEnable()
    {
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
        gameObject.SetActive(false);
        _finishPanel.UpdateScoreText(_rewardMultiplier);
        int levelScore = GameManager.Instance.FinalLevelScore;
        GameDataManager.AddCoins(levelScore * (_rewardMultiplier - 1));
        _coinsAnimation.Animate(Mathf.CeilToInt((float)levelScore / 100));
    }

    private IEnumerator TimerCoroutine()
    {
        float progress = 1f;
        
        while (progress > 0) 
        {
            _skinSlider.value = progress;
            progress -= Time.deltaTime / _duration;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
