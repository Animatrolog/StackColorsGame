using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private int _bufferedValues = 3;

    private List<int> _valueBuffer;
    private int _avgFPS = 0;

    private void Start()
    {
        _avgFPS = (int)(1f / Time.deltaTime);
        _valueBuffer = new List<int>();
        for (int i = 0; i < _bufferedValues; i++)
            _valueBuffer.Add(_avgFPS);
        _text.text = _avgFPS.ToString();
    }

    private int _currentFrame = 0;

    void Update()
    {
        _valueBuffer[_currentFrame] = (int)(1f / Time.unscaledDeltaTime);
        _currentFrame++;
        if(_currentFrame >= _valueBuffer.Count) 
            _currentFrame = 0;
        int summ = 0;
        for (int i = 0; i < _valueBuffer.Count; i++)
            summ += _valueBuffer[i];
        _avgFPS = summ / _valueBuffer.Count;
        _text.text = _avgFPS.ToString();
    }
}
