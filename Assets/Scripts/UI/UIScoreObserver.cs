using TMPro;
using UnityEngine;

public class UIScoreObserver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private CounterAnimation _counterAnimation;

    private GameManager _gameManager;

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;
        _gameManager.Player.PointCounter.OnPointCountChange += UpdateText;
    }

    private void OnDisable()
    {
        _gameManager.Player.PointCounter.OnPointCountChange -= UpdateText;
    }

    private void Start()
    {
        int points = _gameManager.Player.PointCounter.Points;
        _pointsText.text = points.ToString();
    }

    private void UpdateText(int points)
    {
        _pointsText.text = points.ToString();
        _counterAnimation.Animate();
    }

}
