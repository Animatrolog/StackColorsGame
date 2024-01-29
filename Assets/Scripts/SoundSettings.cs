using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private Toggle _musicToggle;

    private void OnEnable()
    {
        LoadSettingsData();
        _soundToggle.onValueChanged.AddListener(HandleSoundToggle);
        _musicToggle.onValueChanged.AddListener(HandleMusicToggle);
    }

    private void LoadSettingsData()
    {
        bool isSoundOn = YandexGame.savesData.IsSoundEnabled;
        _soundToggle.isOn = !isSoundOn;

        bool isMusicOn = YandexGame.savesData.IsMusicEnabled;
        _musicToggle.isOn = !isMusicOn;
    }

    private void OnDisable()
    {
        _soundToggle.onValueChanged.RemoveListener(HandleSoundToggle);
        _musicToggle.onValueChanged.RemoveListener(HandleMusicToggle);
    }

    private void HandleSoundToggle(bool isEnabled)
    {
        YandexGame.savesData.IsSoundEnabled = !isEnabled;
        YandexGame.SaveProgress();
    }

    private void HandleMusicToggle(bool isEnabled)
    {
        YandexGame.savesData.IsMusicEnabled = !isEnabled;
        BackgroundMusic.Instance.EnableMusic(!isEnabled);
        YandexGame.SaveProgress();
    }
}
