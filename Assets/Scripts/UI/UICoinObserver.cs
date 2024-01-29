using TMPro;
using UnityEngine;

public class UICoinObserver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private CounterAnimation _counterAnimation;

    private void OnEnable()
    {
        GameDataManager.OnCoinAmountChange += UpdateText;
    }

    private void OnDisable()
    {
        GameDataManager.OnCoinAmountChange -= UpdateText;
    }

    private void Start()
    {
        UpdateText(GameDataManager.GameData.Coins);
    }

    private void UpdateText(int amount)
    {
        _coinsText.text = amount.ToString();
        _counterAnimation.Animate();
    }
}
