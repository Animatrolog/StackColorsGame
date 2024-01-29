using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTutorial : MonoBehaviour
{
    [SerializeField] private Button _panelButton;
    [SerializeField] private List<UIButton> _buttons;

    private void OnEnable()
    {
        _panelButton.onClick.AddListener(HandleButtonClick);
        foreach (var button in _buttons) 
            button.OnClick += HandleButtonClick;
    }

    private void OnDisable()
    {
        _panelButton.onClick.RemoveListener(HandleButtonClick);
        foreach (var button in _buttons)
            button.OnClick -= HandleButtonClick;
    }

    private void HandleButtonClick()
    {
        gameObject.SetActive(false);
    }
}
