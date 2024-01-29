using TMPro;
using UnityEngine;

public class FinishPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private CoinsAnimation _coinsAnimation;
    [Header("Localization")]
    [SerializeField] private TranslatedText _levelTranslation;
    [SerializeField] private TranslatedText _completeTranslation;
    [SerializeField] private TranslatedText _scoreTranslation;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        int currentLevel = LevelManager.Instance.CurrentLevel;
        _levelText.text = _levelTranslation.Text + " " + currentLevel.ToString() + " " + _completeTranslation.Text;
        _scoreText.text = _scoreTranslation.Text + " " + _gameManager.FinalLevelScore.ToString();
        _coinsText.text = "+" + _gameManager.FinalLevelScore.ToString();
        _coinsAnimation.Animate(Mathf.CeilToInt((float)_gameManager.FinalLevelScore / 100));
    }

    public void UpdateScoreText(int multiplier)
    {
        var gameManager = GameManager.Instance;
        _coinsText.text = "+" + (gameManager.FinalLevelScore * multiplier).ToString();
    }

}
