using System.Collections.Generic;
using UnityEngine;
using YG;
using static UpgradeManager;

public class UIUpgrades : MonoBehaviour
{
    [SerializeField] private UpgradeButton _kickButton;
    [SerializeField] private UpgradeButton _scoreButton;
    [SerializeField] private UpgradeButton _speedButton;
    [SerializeField] private TranslatedText _lvlText;

    private UpgradeManager _upgradeManager;

    private bool _kickRewarded;
    private bool _scoreRewarded;
    private bool _speedRewarded;

    private void OnEnable()
    {
        _upgradeManager = Instance;
        _kickButton.OnClick += HandleKickUpgradeClick;
        _scoreButton.OnClick += HandleScoreUpgradeClick;
        _speedButton.OnClick += HandleSpeedUpgradeClick;
    }

    private void OnDisable()
    {
        _kickButton.OnClick -= HandleKickUpgradeClick;
        _scoreButton.OnClick -= HandleScoreUpgradeClick;
        _speedButton.OnClick -= HandleSpeedUpgradeClick;
    }

    private void Start()
    {
        _upgradeManager.InitUI(this);
    }

    public void UpdateButton(UpgradeType upgrade, int price, bool canBuy, float multiplier = 1f)
    {
        int level = -1;
        switch (upgrade)
        {
            case UpgradeType.Kick:
                level = YandexGame.savesData.KickLevel;
                _kickButton.PriceText.text = price.ToString();
                _kickButton.LevelText.text = _lvlText.Text + " " + level.ToString();
                _kickButton.Interactable = canBuy;
                break;

            case UpgradeType.Score:
                _scoreButton.PriceText.text = price.ToString();
                _scoreButton.LevelText.text = "X " + multiplier.ToString("F1");
                _scoreButton.Interactable = canBuy;
                break;

            case UpgradeType.Speed:
                level = YandexGame.savesData.SpeedLevel;
                _speedButton.PriceText.text = price.ToString();
                _speedButton.LevelText.text = _lvlText.Text + " " + level.ToString();
                _speedButton.Interactable = canBuy;
                break;
        }
    }

    public void ShowAdAtButton(UpgradeType upgrade, bool show)
    {
        switch (upgrade)
        {
            case UpgradeType.Kick:
                _kickButton.PriceText.gameObject.SetActive(!show);
                _kickButton.AdText.gameObject.SetActive(show);
                if(show) _kickButton.Interactable = true;
                _kickRewarded = show;
                break;

            case UpgradeType.Score:
                _scoreButton.PriceText.gameObject.SetActive(!show);
                _scoreButton.AdText.gameObject.SetActive(show);
                if (show) _scoreButton.Interactable = true;
                _scoreRewarded = show;
                break;

            case UpgradeType.Speed:
                _speedButton.PriceText.gameObject.SetActive(!show);
                _speedButton.AdText.gameObject.SetActive(show);
                if (show) _speedButton.Interactable = true;
                _speedRewarded = show;
                break;
        }
    }

    private void HandleKickUpgradeClick()
    {
        if (_kickRewarded) RewardedAd.ShowAd(KickRewardeshown);
        else _upgradeManager.TryToUpgrade(UpgradeType.Kick, _kickRewarded);
    }

    private void KickRewardeshown()
    {
        _upgradeManager.TryToUpgrade(UpgradeType.Kick, _kickRewarded);
    }

    private void HandleScoreUpgradeClick()
    {
        if (_scoreRewarded) RewardedAd.ShowAd(ScoreRewardeshown);
        else _upgradeManager.TryToUpgrade(UpgradeType.Score, _scoreRewarded);
    }

    private void ScoreRewardeshown()
    {
        _upgradeManager.TryToUpgrade(UpgradeType.Score, _scoreRewarded);
    }

    private void HandleSpeedUpgradeClick()
    {
        if (_speedRewarded) RewardedAd.ShowAd(SpeedRewardeshown);
        else _upgradeManager.TryToUpgrade(UpgradeType.Speed, _speedRewarded);
    }

    private void SpeedRewardeshown()
    {
        _upgradeManager.TryToUpgrade(UpgradeType.Speed, _speedRewarded);
    }
}
