using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class ContinueButton : MonoBehaviour
{
    [SerializeField] private GameObject _skinUnlockPanel;
    private Button _button;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(HandleButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleButtonClick);
    }

    private void HandleButtonClick()
    {
        if(GameDataManager.HasLockedSkins())
            _skinUnlockPanel.SetActive(true);
        else
            GameManager.Instance.RestartGame();
    }
}
