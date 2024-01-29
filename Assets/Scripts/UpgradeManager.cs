using System;
using System.Collections;
using UnityEngine;

using static GameDataManager;

public class UpgradeManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _rewardedDelay = 1;
    [SerializeField] private NMKSStruct _score;
    [Header("Price settings")]
    [SerializeField] private NMKSStruct _kickPricing;
    [SerializeField] private NMKSStruct _scorePricing;
    [SerializeField] private NMKSStruct _speedPricing;

    private UIUpgrades _ui;
    private Coroutine _rewardedDelayCoroutine;
    private bool _delayed;
    public float FinishMultiplier {  get; private set; }

    public static UpgradeManager Instance;

    public Action<UpgradeType> OnUpgrade;

    private void OnEnable()
    {
        OnCoinAmountChange += UpdateUI;
    }

    private void OnDisable()
    {
        OnCoinAmountChange -= UpdateUI;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void UpdateUI(int i = 0)
    {
        UpdateUIButton(UpgradeType.Kick);
        UpdateUIButton(UpgradeType.Score);
        UpdateUIButton(UpgradeType.Speed);
        _delayed = false;
    }

    private void UpdateUIButton(UpgradeType buttonType)
    {
        bool canBuy = CheckCanBuy(buttonType, out int price);

        if (buttonType == UpgradeType.Score)
        {
            FinishMultiplier = GetScore(GameData.ScoreLevel);
        }

        _ui.ShowAdAtButton(buttonType, false);
        _ui.UpdateButton(buttonType, price, canBuy, FinishMultiplier);

        if (!canBuy)
        {
            if (!_delayed)
            {
                if (_rewardedDelayCoroutine == null)
                    StartCoroutine(RewardedDelay());
            }
            else
            {
                _ui.ShowAdAtButton(buttonType, true);
            }
        }
    }

    private IEnumerator RewardedDelay()
    {
        yield return new WaitForSeconds(_rewardedDelay);
        _delayed = true;
        UpdateUI();
        _rewardedDelayCoroutine = null;
    }

    private int GetPrice(int level, NMKSStruct nMKS)
    {
        float price = 1f;

        for (int i = 0; i < level - 1; i++)
        {
            int multiplier = Mathf.FloorToInt((i + 1f) / nMKS.K);
            float divider = 1f;
            if (multiplier > 0f)
                divider = nMKS.S * multiplier;
            price += price * (nMKS.M / divider);
        }
        return Mathf.RoundToInt(nMKS.N * price);
    }

    public void InitUI(UIUpgrades ui)
    {
        _ui = ui;
        UpdateUI();
    }

    public void TryToUpgrade(UpgradeType upgrade, bool free = false)
    {
        CheckCanBuy(upgrade, out int price);
        switch (upgrade)
        {
            case UpgradeType.Kick: GameData.KickLevel++; break;
            case UpgradeType.Score: GameData.ScoreLevel++; break;
            case UpgradeType.Speed: GameData.SpeedLevel++; break;
        }
        if (!free)
        {
            if (GameData.Coins >= price) 
                AddCoins(-price);
        }
        else UpdateUI();

        OnUpgrade?.Invoke(upgrade);
        GameDataManager.SaveProgress();

        if(upgrade == UpgradeType.Score)
            Debug.Log("UPGRADE - Score on current level => " + FinishMultiplier.ToString("F1"));
    }

    private bool CheckCanBuy(UpgradeType upgrade, out int price)
    {
        int coins = GameData.Coins;
        price = 0;
        switch (upgrade)
        {
            case UpgradeType.Kick:
                price = GetPrice(GameData.KickLevel, _kickPricing);
                return coins >= price;
            case UpgradeType.Score:
                price = GetPrice(GameData.ScoreLevel, _scorePricing);
                return coins >= price;
            case UpgradeType.Speed:
                price = GetPrice(GameData.SpeedLevel, _speedPricing);
                return coins >= price;
        }
        return false;
    }

    private float GetScore(int level)
    {
        float value = 0;
        float lastK = 0.2f;
        for (int i = 0; i < level - 1; i++)
        {
            int multiplier = Mathf.FloorToInt((i + 1f) / _score.K);
            lastK = _score.S * multiplier + _score.M;
            value += lastK;
        }
        return _score.N + value;
    }


}

public enum  UpgradeType
{
    Kick,
    Score,
    Speed
}

[Serializable]
public struct NMKSStruct
{
    public int N;
    public float M;
    public int K;
    public float S;
}