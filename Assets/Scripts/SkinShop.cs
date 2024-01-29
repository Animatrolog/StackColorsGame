using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SkinShop : MonoBehaviour
{
    [SerializeField] private SkinButtonSpawner _skinButtonSpawner;
    [SerializeField] private UICurrentPlayerSkin _currentSkinUi;
    [SerializeField] private ShopPageSwitch _shopPageSwitch;
    [SerializeField] private Transform _basicSkinsContainer;
    [SerializeField] private Transform _epicSkinsContainer;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private RewardedCoinsButton _rewardButton;
    [SerializeField] private Image _inputBlock;
    [Header("Roulette")]
    [SerializeField] private float _rouletteDuration = 2f;
    [SerializeField] private float _rouletteFrequency = 10f;
    [Header("Prices")]
    [SerializeField] private int _defaultBasicSkinPrice = 1000;
    [SerializeField] private float _perBasicSkinPrice = 1f;
    [SerializeField] private int _defaultEpicSkinPrice = 2000;
    [SerializeField] private float _perEpicSkinPrice = 1f;

    private int _currentPage = 0;

    private void Start()
    {
        _skinButtonSpawner.SpawnSkinButtons(0, 5, _basicSkinsContainer);
        _skinButtonSpawner.SpawnSkinButtons(6, 11, _epicSkinsContainer);
        _currentSkinUi.ChangeSkinInstance(GameDataManager.GameData.SkinIndex);
        
        _skinButtonSpawner.HighlightCurrentSkinButton();
        foreach (var button in _skinButtonSpawner.Buttons)
            button.OnClick += HandleSkinButtonClick;
        int startPage = YandexGame.savesData.SkinIndex < 6 ? 0 : 1;
        _shopPageSwitch.SwitchPage(startPage);
    }

    private void OnEnable()
    {
        _shopPageSwitch.OnPageChanged += HandlePageSwitch;
        _buyButton.onClick.AddListener(HandleBuyButtonClick);
        GameDataManager.OnCoinAmountChange += UpdatePrice;
        _skinButtonSpawner.HighlightCurrentSkinButton();
        foreach (var button in _skinButtonSpawner.Buttons)
            button.OnClick += HandleSkinButtonClick;
        int startPage = YandexGame.savesData.SkinIndex < 6 ? 0 : 1;
        _shopPageSwitch.SwitchPage(startPage);
    }

    private void OnDisable()
    {
        _shopPageSwitch.OnPageChanged -= HandlePageSwitch;
        GameDataManager.OnCoinAmountChange -= UpdatePrice;
        _buyButton.onClick.RemoveListener(HandleBuyButtonClick);
        foreach (var button in _skinButtonSpawner.Buttons)
            button.OnClick -= HandleSkinButtonClick;
    }

    private void UpdatePrice(int coins)
    {
        _buyButton.interactable = UpdatePriceText(_currentPage);
        _rewardButton.UpdateData();
    }

    private int GetPrice(int pageId)
    {
        if (pageId == 0)
        {
            int unlockedSkins = 6 - CountClosedSkins(0, 5);
            int price = (int)(_defaultBasicSkinPrice + 
                (_perBasicSkinPrice * (unlockedSkins - 1)));
            return price;
        }
        else
        {
            int unlockedSkins = 6 - CountClosedSkins(6, 11);
            int price = (int)(_defaultEpicSkinPrice + 
                (_perEpicSkinPrice * unlockedSkins));
            return price;
        }
    }

    private bool UpdatePriceText(int pageId)
    {
        int price = GetPrice(pageId);
        string priceText = price.ToString();
        if (price >= 1000) priceText = ((float)price / 1000).ToString("F1") + "k";
        _priceText.text = priceText;
        return YandexGame.savesData.Coins >= price;
    }

    private void HandlePageSwitch(int pageId)
    {
        _currentPage = pageId;
        
        _buyButton.interactable = UpdatePriceText(pageId);

        if(pageId == 0)
        {
            if(CountClosedSkins(0, 5) == 0)
            {
                _buyButton.gameObject.SetActive(false);
            }
            else
                _buyButton.gameObject.SetActive(true);
        }
        else if (pageId == 1)
        {
            if (CountClosedSkins(6, 11) == 0)
            {
                _buyButton.gameObject.SetActive(false);
            }
            else
                _buyButton.gameObject.SetActive(true);
        }
    }

    private void HandleSkinButtonClick(int skinId)
    {
        GameManager.Instance.Player.SkinChanger.ChangeSkin(skinId);
        _currentSkinUi.ChangeSkinInstance(GameDataManager.GameData.SkinIndex);
        _skinButtonSpawner.HighlightCurrentSkinButton();
    }

    private void HandleBuyButtonClick()
    {
        if(_currentPage == 0)
            StartCoroutine(OpenSkinCoroutine(0, 5));
        else
            StartCoroutine(OpenSkinCoroutine(6, 11));
    }

    private IEnumerator OpenSkinCoroutine(int from, int to)
    {
        _inputBlock.gameObject.SetActive(true);
        List<SkinButton> buttons = _skinButtonSpawner.Buttons;
        int price = GetPrice(_currentPage);
        float progress = 0f;
        int randomId = 1;
        int resultId = -1;
        int prevId = -1;
        float valueForShow = 0.1f;

        while (progress < 1)
        {
            progress += Time.deltaTime / _rouletteDuration;

            while(randomId == prevId || buttons[randomId].Interactable)
            {
                if(CountClosedSkins(from, to) < 2)
                {
                    Debug.Log("Closed skins count " + CountClosedSkins(from, to));
                    randomId = GetLastClosedSkin(from, to);
                    progress = 1f;
                    break;
                }
                randomId = Random.Range(from, to + 1);
            }

            if(progress >= valueForShow)
            {
                if(prevId != -1) buttons[prevId].UnlockVisuals(false);
                buttons[randomId].UnlockVisuals(true);
                prevId = randomId;
                valueForShow += progress * _rouletteDuration / _rouletteFrequency;
                resultId = prevId;
            }
           
            yield return null;
        }

        YandexGame.savesData.unlockSkins[resultId] = true;
        buttons[resultId].Interactable = true;
        GameDataManager.AddCoins(-price);
        YandexGame.SaveProgress();
        HandlePageSwitch(_currentPage);
        _inputBlock.gameObject.SetActive(false);
    }

    private int GetLastClosedSkin(int from, int to)
    {
        int closedSkinId = -1;
        for (int i = from; i <= to; i++)
            if (!YandexGame.savesData.unlockSkins[i]) closedSkinId = i;
        return closedSkinId;
    }

    private int CountClosedSkins(int from, int to)
    {
        int closedSkinsCount = 0;
        for(int i = from; i <= to; i++)
            if(!YandexGame.savesData.unlockSkins[i]) closedSkinsCount++;
        return closedSkinsCount;
    }
}
