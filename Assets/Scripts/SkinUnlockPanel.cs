using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinUnlockPanel : MonoBehaviour
{
    [SerializeField] private GameObject _palne;
    [SerializeField] private ActiveSkinsPool _activeSkinsPool;
    [SerializeField] private Image _backImage;
    [SerializeField] private Slider _skinSlider;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private float _animationDuration = 1.5f;
    [SerializeField] private float _skinUnlockStep = 0.25f;
    [SerializeField] private GameObject _unlockEffect;
    [Header("Unlock and lose buttons")]
    [SerializeField] private Button _getSkinButton;
    [SerializeField] private Slider _timerSlider;
    [SerializeField] private float _timerDuration = 4;
    [SerializeField] private float _durationPercentLoseShow = 0.5f;
    [SerializeField] private Button _loseSkinButton;
    [Header("Delays")]
    [SerializeField] private float _animationStartDelay = 0.5f;
    [SerializeField] private float _restartDelay = 1f;
    [Header("Localization")]
    [SerializeField] private TranslatedText _unlockedTranslation;

    private Coroutine _timerCoroutine;
    private List<int> _lockedSkins = new List<int>();

    private void OnEnable()
    {
        if (!HasLockedSkins())
        {
            GameManager.Instance.RestartGame();
            return;
        }
        CheckSkinId();
        StartCoroutine(UnlockAnimation());
    }

    private void Restart()
    {
        StartCoroutine(RestartDelay());
    }

    private IEnumerator RestartDelay()
    {
        yield return new WaitForSeconds(_restartDelay);
        GameManager.Instance.RestartGame();
    }

    private IEnumerator UnlockAnimation()
    {
        float progress = 0f;
        float skinProgress = GameDataManager.GameData.skinUnlockProgress;
        float sliderStartvalue = 1f - skinProgress;
        int textStartValue = Mathf.RoundToInt(skinProgress * 100);
        int skinIndex = GameDataManager.GameData.skinToUnlockId;
        _backImage.sprite = _activeSkinsPool.ActiveSkins[skinIndex].SkinSprite;
        
            GameDataManager.GameData.skinUnlockProgress += _skinUnlockStep;
        GameDataManager.SaveProgress();

        _progressText.text = textStartValue.ToString() + "%";
        _skinSlider.value = sliderStartvalue;

        yield return new WaitForSeconds(_animationStartDelay);

        while (progress <= 1) 
        {
            _progressText.text = (textStartValue + Mathf.RoundToInt(progress * _skinUnlockStep * 100)).ToString() + "%";
            _skinSlider.value = sliderStartvalue - (progress * _skinUnlockStep);
            progress += Time.deltaTime / _animationDuration;
            yield return null;
        }

        skinProgress = GameDataManager.GameData.skinUnlockProgress;

        if (skinProgress >= 1f)
        {
            _unlockEffect.SetActive(true);
            _getSkinButton.gameObject.SetActive(true);
            _getSkinButton.onClick.AddListener(HandleButtonClick);
            _timerCoroutine = StartCoroutine(TimerCoroutine());
        }
        else Restart();
    }

    private void CheckSkinId()
    {
        int skinToUnlock = GameDataManager.GameData.skinToUnlockId;
        bool[] skins = GameDataManager.GameData.unlockSkins;
        float skinProgress = GameDataManager.GameData.skinUnlockProgress;

        if (skinProgress >= 1) GameDataManager.GameData.skinUnlockProgress = 0f;

        Debug.Log("Skin to unlock id = " + skinToUnlock + " status unlocked = " + skins[skinToUnlock]);
        if (skins[skinToUnlock])
        {
            skinToUnlock = _lockedSkins[0];
            Debug.Log("New skin to unlock id = " + skinToUnlock);
            GameDataManager.GameData.skinToUnlockId = skinToUnlock;
            GameDataManager.SaveProgress();
        }
        //int randomId = skinToUnlock;
        //if (skins[randomId])
        //{
        //    while(skins[randomId])
        //    {
        //        randomId = _lockedSkins[Random.Range(0, _lockedSkins.Count)];
        //    }
        //    Debug.Log("New skin to unlock id = " + randomId);
        //    GameDataManager.GameData.skinToUnlockId = randomId;
        //    GameDataManager.SaveProgress();
        //}

    }

    private bool HasLockedSkins()
    {
        bool[] skins = GameDataManager.GameData.unlockSkins;
        for (int i = 0; i < skins.Length; i++)
            if (!skins[i]) _lockedSkins.Add(i); 
        return _lockedSkins.Count > 0;
    }

    private void HandleButtonClick()
    {
        RewardedAd.ShowAd(HandleAdRearded);
    }

    private void HandleAdRearded()
    {
        int skinIndex = GameDataManager.GameData.skinToUnlockId;
        GameDataManager.GameData.unlockSkins[skinIndex] = true;
        Debug.Log("Skin with id = " + skinIndex + " unlocked!");
        GameDataManager.GameData.SkinIndex = skinIndex;
        GameDataManager.SaveProgress();
        _progressText.text = _unlockedTranslation.Text;
        _getSkinButton.onClick.RemoveListener(HandleButtonClick);
        _getSkinButton.gameObject.SetActive(false); 
        if(_timerCoroutine != null) StopCoroutine(_timerCoroutine);
        _loseSkinButton.gameObject.SetActive(false);
        Restart();
    }

    private IEnumerator TimerCoroutine()
    {
        float progress = 0;

        while (progress <= 1)
        {
            _timerSlider.value = 1 - progress;

            if(progress >= _durationPercentLoseShow)
                if(!_loseSkinButton.IsActive())
                    _loseSkinButton.gameObject.SetActive(true);

            progress += Time.deltaTime / _timerDuration;
            yield return null;
        }

        _timerCoroutine = null;
        _loseSkinButton.gameObject.SetActive(false);
        _getSkinButton.gameObject.SetActive(false);
        Restart();
    }
}
