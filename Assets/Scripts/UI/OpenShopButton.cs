using UnityEngine;

public class OpenShopButton : MonoBehaviour
{
    [SerializeField] private UIButton _button;
    [SerializeField] private GameObject _shopPanel;

    private void OnEnable()
    {
        _button.OnClick += HandleButtonClick;
    }

    private void OnDisable()
    {
        _button.OnClick -= HandleButtonClick;
    }

    private void HandleButtonClick()
    {
        _shopPanel.SetActive(true);
    }
}
