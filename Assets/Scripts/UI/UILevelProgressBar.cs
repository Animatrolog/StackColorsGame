using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UILevelProgressBar : MonoBehaviour
{
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Image _sliderFillImage;
    [SerializeField] private Color _sliderColor;

    private PlayerMovement _playerMovement;
    private LevelManager _levelManager;
    private Coroutine _progressCoroutine;

    private void Start()
    {
        _playerMovement = GameManager.Instance.Player.Movement;
        _levelManager = LevelManager.Instance;
        _sliderFillImage.color = _sliderColor;
    }

    private void UpdateSliderValue()
    {
        float traveledDistance = _playerMovement.TotalTraveledDistance + _playerMovement.TraveledSplineDistance;
        _progressSlider.value = traveledDistance / _levelManager.CurrentLevelInstance.LevelLength; 
    }

    private void Update()
    {
        UpdateSliderValue();
    }
}
