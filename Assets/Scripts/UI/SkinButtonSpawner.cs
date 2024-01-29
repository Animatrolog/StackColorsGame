using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SkinButtonSpawner : MonoBehaviour
{
    [SerializeField] private SkinButton _buttonPrefab;
    [SerializeField] private ActiveSkinsPool _activeSkinsPool;
    [SerializeField] private float _selectedScale = 1.15f;

    public List<SkinButton> Buttons {  get; private set; } = new();

    public void SpawnSkinButtons(int startIndex, int endIndex, Transform container)
    {
        var skins = _activeSkinsPool.ActiveSkins;
        var unlockedSkins = YandexGame.savesData.unlockSkins;

        for (int i = startIndex; i <= endIndex; i++)
        {
            SkinButton button = Instantiate(_buttonPrefab, container);
            button.Initialize(i, skins[i]);
            if (unlockedSkins[i])
                button.Interactable = true;
            Buttons.Add(button);
        }
    }

    public void HighlightCurrentSkinButton()
    {
        foreach(var button in Buttons)
        {
            if(button.SkinID == GameDataManager.GameData.SkinIndex)
            {
                button.GetComponent<RectTransform>().localScale = Vector3.one * _selectedScale;
                button.SetSelected(true);
            }
            else
            {
                button.GetComponent<RectTransform>().localScale = Vector3.one;
                button.SetSelected(false);
            }
        }   
    }
}
