using TMPro;
using UnityEngine;

public class HighscorePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _highscoreText;
    [SerializeField] private TextColorAnimation _colorAniation;
    [Header("Localization")]
    [SerializeField] private TranslatedText _highscoreTranslation;
    [SerializeField] private TranslatedText _newTranslation;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        int highscore = GameDataManager.GameData.Highscore;
        
        if (highscore == 0 && _gameManager.FinalLevelScore == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        if (_gameManager.FinalLevelScore > highscore)
        {
            _highscoreText.text = _newTranslation.Text 
                + " " + _highscoreTranslation.Text + "!!! " + _gameManager.FinalLevelScore.ToString();
            _colorAniation.Animate(true);
            GameDataManager.GameData.Highscore = _gameManager.FinalLevelScore;
            GameDataManager.SaveProgress();
        }
        else if (highscore > 0)
        {
            _highscoreText.text = _highscoreTranslation.Text + " " + highscore.ToString();
        }
    }
}
