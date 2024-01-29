using System;
using UnityEngine;
using YG;

public class RewardedAd : MonoBehaviour
{
    private static Action _onRewardedWatched;

    public static void ShowAd(Action onRewardedWathced)
    {
        if (!YandexGame.SDKEnabled) return;
        
        _onRewardedWatched = onRewardedWathced;
        YandexGame.RewVideoShow(0);
        YandexGame.RewardVideoEvent += OnAdsDidFinish;
        YandexGame.ErrorVideoEvent += OnAdsError;
    }

    public static void OnAdsError()
    {
        Debug.Log("Rewarded Ad error !!!");
        YandexGame.ErrorVideoEvent -= OnAdsError;
        _onRewardedWatched = null;
    }

    public static void OnAdsDidFinish(int placementId)
    {
        YandexGame.RewardVideoEvent -= OnAdsDidFinish;
        _onRewardedWatched?.Invoke();
        _onRewardedWatched = null;
        Debug.Log("Rewarded finished! :" + placementId);
    }
}
