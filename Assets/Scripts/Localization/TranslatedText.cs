using UnityEngine;
using YG;
using UnityEngine.Events;

public class TranslatedText : MonoBehaviour
{
    [SerializeField] private string _ru, _en;

    public string Ru => _ru;
    public string En => _en;

    public int CurrentLangIndex { get; private set; }
    public  string Text {  get; private set; }

    public UnityAction OnTranslationComplete;

    private void OnEnable()
    {
        if(YandexGame.SDKEnabled) SwitchLanguage(YandexGame.savesData.language);
        YandexGame.SwitchLangEvent += SwitchLanguage;
    }
    private void OnDisable() => YandexGame.SwitchLangEvent -= SwitchLanguage;

    public void SwitchLanguage(string lang)
    {
        if (lang == "ru")
            Text = _ru;
        else if (lang == "en")
            Text = _en;
        OnTranslationComplete?.Invoke();
    }
}