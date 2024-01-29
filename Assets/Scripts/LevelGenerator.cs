using Dreamteck.Splines;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : Level
{
    [SerializeField] private LevelPiece _startPiece;
    [SerializeField] private LevelPiece _finishPiece;
    [SerializeField] private int _levelPiecesCount = 5;
    [SerializeField] private List<LevelPiece> _levePiecesPool;
    [SerializeField] private SplineComputer _bGSplineComputer;

    public override Player Initialize(Player player, GameObject backgroundPrefab, bool shuffleColors)
    {
        ShuffleColors();
 
        PlayerInstance = Instantiate(player, transform);

        SplinePoint[] points = new SplinePoint[_levelPiecesCount + 2];
        LevelPiece startPiece = Instantiate(_startPiece, transform);
        startPiece.LastGateColorId = Random.Range(0, 4);
        _levelPieces.Add(startPiece);
        //points[0] = new SplinePoint();
        //points[0].position = startPiece.transform.position;

        for (int i = 1; i <= _levelPiecesCount; i++)
        {
            int randomId = Random.Range(0, _levePiecesPool.Count);
            LevelPiece levelPiece = Instantiate(_levePiecesPool[randomId], transform);
            LevelPiece prevPiece = _levelPieces[i - 1];
            var splineSample = prevPiece.MainSpline.Evaluate(1f);
            levelPiece.transform.position = prevPiece.transform.position + 
                (prevPiece.transform.rotation * splineSample.position);

            Vector3 bgPosition = levelPiece.transform.position;
            bgPosition.y = 0;
            points[i-1] = new SplinePoint();
            points[i-1].position = bgPosition;
            
            levelPiece.transform.rotation = Quaternion.LookRotation(prevPiece.transform.rotation * splineSample.forward);
            levelPiece.Initialize(PlayerInstance);
            LevelLength += levelPiece.MainSpline.CalculateLength();
            _levelPieces.Add(levelPiece);
        }

        int lastIndex = _levelPieces.Count - 1;
        LevelPiece lastPiece = _levelPieces[lastIndex];
        var sample = lastPiece.MainSpline.Evaluate(lastPiece.MainSpline.pointCount);
        LevelPiece finishPiece = Instantiate(_finishPiece, transform);

        finishPiece.transform.position = lastPiece.transform.position + 
            (lastPiece.transform.rotation * sample.position);
        finishPiece.transform.rotation = Quaternion.LookRotation(lastPiece.transform.rotation * sample.forward);

        points[lastIndex] = new SplinePoint();
        points[lastIndex].position = finishPiece.transform.position;

        points[lastIndex + 1] = new SplinePoint();
        points[lastIndex + 1].position = finishPiece.transform.position + finishPiece.transform.forward * 250;

        _levelPieces.Add(finishPiece);

        _bGSplineComputer.SetPoints(points);
        _tileCollector = PlayerInstance.TileCollector;
        var spline = _levelPieces[0].GetSpline(Vector3.zero);
        PlayerInstance.Movement.SplineFollower.spline = spline;
        PlayerInstance.Movement.SplineFollower.onEndReached += SwitchToNextPiece;
        return PlayerInstance;
    }
}
