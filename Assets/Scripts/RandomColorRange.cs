using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ColorChanger))]
public class RandomColorRange : MonoBehaviour
{
    [SerializeField] private List<int> _allowedIndexes;
    [SerializeField] private ColorGate _colorGate;

    private ColorChanger _colorChanger;

    private void Start()
    {
        if (_allowedIndexes.Count == 0) return;

        LevelGenerator generator = GetComponentInParent<LevelGenerator>();
        if (generator == null) return;

        LevelPiece levelPiece = GetComponentInParent<LevelPiece>();
        if (levelPiece == null) return;

        _colorChanger = GetComponent<ColorChanger>();

        int randomIndex = -1;

        while(!_allowedIndexes.Contains(randomIndex) || randomIndex == levelPiece.LastGateColorId)
            randomIndex = Random.Range(0, 4);

        _colorChanger.SetColor(randomIndex);

        if (_colorGate == null) return;
        
        levelPiece.SetLastGateColor(randomIndex);

    }
}
