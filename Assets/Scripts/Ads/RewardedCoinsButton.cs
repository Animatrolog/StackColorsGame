using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class RewardedCoinsButton : MonoBehaviour
{
    [SerializeField] private CoinsAnimation _coinsAnimation;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private int _defaultReward = 10000;
    [SerializeField] private int _coinsRewardPerSkin = 5000;

    private int _finalReward;

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleButtonClock);
        UpdateData();
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleButtonClock);
    }

    private int CountUnlockedSkins(int from, int to)
    {
        int closedSkinsCount = 0;
        for (int i = from; i <= to; i++)
            if (GameDataManager.GameData.unlockSkins[i]) closedSkinsCount++;
        return closedSkinsCount;
    }

    public void UpdateData()
    {
        int unlockedSkins = CountUnlockedSkins(1,11);
        _finalReward = _defaultReward + (unlockedSkins * _coinsRewardPerSkin);
        string priceText = _finalReward.ToString();
        if (_defaultReward >= 1000) priceText = ((float)_finalReward / 1000).ToString("F1") + "k";
        _text.text = "+" + priceText;
    }

    private void HandleButtonClock()
    {
        RewardedAd.ShowAd(HandleAdRewad);
    }

    private void HandleAdRewad()
    {
        if(_coinsAnimation != null) _coinsAnimation.Animate(Mathf.CeilToInt((float)_finalReward/ 1000));
        GameDataManager.AddCoins(_finalReward);
        UpdateData();
    }
}
