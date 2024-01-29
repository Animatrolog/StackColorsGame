using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class RetryButton : MonoBehaviour
{
    private Button _restartButton;

    private void Awake()
    {
        _restartButton = GetComponent<Button>();
        _restartButton.onClick.AddListener(Restart);
    }

    private void Restart()
    {
        GameManager.Instance.RestartGame();
    }
}
