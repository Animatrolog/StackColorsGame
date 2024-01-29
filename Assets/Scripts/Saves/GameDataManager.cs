using System;
using System.Collections;
using UnityEngine;
using YG;

public class GameDataManager : MonoBehaviour
{
    [SerializeField] private float _timeForCoin = 0.5f;

    public static GameDataManager Instance;
    public static SavesYG GameData => YandexGame.savesData;

    public static Action<int> OnCoinAmountChange;
    public static Action OnDataLoaded;

    private float _timer;
    private Coroutine _delayedSaveCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    public static void AddCoins(int amount)
    {
        GameData.Coins += amount;
        OnCoinAmountChange?.Invoke(GameData.Coins);
    }

    private void OnEnable()
    {
        OnCoinAmountChange += StartDelayTimer;
    }

    private void OnDisable()
    {
        OnCoinAmountChange -= StartDelayTimer;
        YandexGame.SaveProgress();
    }

    public static void SaveProgress()
    {
        YandexGame.SaveProgress();
    }

    private void StartDelayTimer(int coins)
    {
        _timer = _timeForCoin;
        _delayedSaveCoroutine ??= StartCoroutine(CoinCounter());
    }

    public static bool HasLockedSkins()
    {
        bool[] skins = GameData.unlockSkins;
        for (int i = 0; i < skins.Length; i++)
            if (!skins[i]) return true;
        return false;
    }

    private IEnumerator CoinCounter()
    {
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            yield return null;
        }
        ResetTimer();
    }

    private void ResetTimer()
    {
        Debug.Log("Save coins delayed");
        YandexGame.SaveProgress();
        _timer = 0;
        _delayedSaveCoroutine = null;
    }
}
