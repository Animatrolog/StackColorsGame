using System;
using UnityEngine;
using YG;

public class PlayerSkinChanger : MonoBehaviour
{
    [SerializeField] private int  _currentSkinId = 0;
    [SerializeField] private ActiveSkinsPool _skinsPool;

    public PlayerSkinModel CurrentSkinInstance { get; private set; }
    public int CurrentSkinId => _currentSkinId;
    public Action<int> OnSkinChanged;

    private void Start()
    {
        _currentSkinId = GameDataManager.GameData.SkinIndex;
        ChangeSkin(_currentSkinId);
    }

    public void ChangeSkin(int skinIndex)
    {
        if (CurrentSkinInstance != null) Destroy(CurrentSkinInstance.gameObject);
        _currentSkinId = skinIndex;
        CurrentSkinInstance = Instantiate(_skinsPool.ActiveSkins[skinIndex], transform);
        OnSkinChanged?.Invoke(skinIndex);
        GameDataManager.GameData.SkinIndex = skinIndex;
        YandexGame.SaveProgress();
    }
}
