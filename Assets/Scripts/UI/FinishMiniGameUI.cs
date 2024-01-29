using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishMiniGameUI : MonoBehaviour
{
    [SerializeField] private Slider _chargeSlider;
    [SerializeField] private Image _sliderFillImage;
    [SerializeField] private TMP_Text _multiplyerText;

    public void ShowPowerSlider(bool show)
    {
        _chargeSlider.gameObject.SetActive(show);
    }

    public void UpdateSlider(float currentCharge)
    {
        _chargeSlider.value = currentCharge;
        Color color = Color.Lerp(Color.green, Color.red, _chargeSlider.value);
        _sliderFillImage.color = color;
    }

    public void ShowMultiplyerText(float multiplyer)
    {
        _multiplyerText.gameObject.SetActive(true);
        _multiplyerText.text = "X" + multiplyer.ToString("F1");
        if (_multiplyerText.TryGetComponent(out CounterAnimation animation))
            animation.Animate();

    }
}
