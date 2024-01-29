using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameProgress : MonoBehaviour
{
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Image _sliderFill;
    [SerializeField] private List<RectTransform> _levelBoxes;
    [SerializeField] private List<TMP_Text> _levelTexts;
    [SerializeField] private float _currentLevelBoxSize = 1.3f;
    [SerializeField] private Color _defaultColor = Color.white;

    private LevelManager _levelManager;

    private void Start()
    {
        _levelManager = LevelManager.Instance;

        int level = _levelManager.CurrentLevel;
        
        float floating = level % 5f;

        int boxIndex = floating > 0 ? (int)floating - 1 : 4;
        _levelBoxes[boxIndex].localScale = Vector3.one * _currentLevelBoxSize;
        _sliderFill.color = _defaultColor;
        for(int i = 0; i < _levelBoxes.Count; i++)
        {
            if (i <= boxIndex)
            {
                _levelBoxes[i].GetComponent<Image>().color = _defaultColor;
                if(i < _levelTexts.Count)
                    _levelTexts[i].color = Color.white;
            }
        }
        
        UpdateLevelNUmbers(level - 1);
        _progressSlider.value = (float)boxIndex / 4;
    }

    private void UpdateLevelNUmbers(int lvlIndex)
    {
        int firstNumber = 1 + Mathf.FloorToInt((lvlIndex) / 5) * 5;
        for (int i = 0; i < 4; i++)
        {
           _levelTexts[i].text = (firstNumber + i).ToString();
        }
    }
}
