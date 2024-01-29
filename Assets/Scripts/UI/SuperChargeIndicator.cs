using UnityEngine;
using UnityEngine.UI;

public class SuperChargeIndicator : MonoBehaviour
{
    [SerializeField] private Slider _chargeSlider;
    [SerializeField] private float _hideThreshold = 0.15f;
    [SerializeField] private Image _sliderFillImage;

    private void OnEnable()
    {
        GameManager.Instance.Player.SuperMode.OnChargeChanged += UpdateSlider;
    }

    private void OnDisable()
    {
        GameManager.Instance.Player.SuperMode.OnChargeChanged -= UpdateSlider;
    }

    public void UpdateSlider(float value, int colorId)
    {
        _chargeSlider.gameObject.SetActive(value >= _hideThreshold || (colorId == 2 && value > 0));
        _chargeSlider.value = value;

        Color color = Color.green;
        switch (colorId)
        {
            case 1:
                color = Color.red;
                break;
            case 2:
                color = Color.yellow;
                break;
        }
        _sliderFillImage.color = color;
    }
}
