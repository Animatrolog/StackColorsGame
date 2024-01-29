using TMPro;
using UnityEngine;

public class UILevelObserver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;

    private void Start()
    {
        UpdateText(LevelManager.Instance.CurrentLevel);
    }

    private void UpdateText(int amount)
    {
        _levelText.text = amount.ToString();
    }
}
