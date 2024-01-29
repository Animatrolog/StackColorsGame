using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YG;

public class Level : MonoBehaviour
{
    [SerializeField] protected Color[] _levelTileColors = new Color[] { Color.blue, Color.red, Color.green, Color.yellow };
    [SerializeField] private bool _isSuperModeAllowed = true;
    [SerializeField] private SplineComputer _backgroundSplinePrefab;
    [SerializeField] private float _prefabOffset = 249f;

    protected TileCollector _tileCollector;
    protected List<LevelPiece> _levelPieces = new List<LevelPiece>();
    protected int _currentPieceIndex;
    private SplineComputer _backgroundSpline;
    private GameObject _backgroundPrefab;

    public Color[] LevelColors => _levelTileColors;
    public bool IsSuperModeAllowed => _isSuperModeAllowed;
    public List<LevelPiece> LevelPieces => _levelPieces;

    public Player PlayerInstance {  get; protected set; }
    public float LevelLength { get; protected set; }

    public virtual Player Initialize(Player player, GameObject backgroundPrefab, bool shuffleColors)
    {
        if(backgroundPrefab != null)
            _backgroundPrefab = backgroundPrefab;

        if (shuffleColors)
            ShuffleColors();

        PlayerInstance = Instantiate(player, transform);

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out LevelPiece levelPiece))
            {
                levelPiece.Initialize(PlayerInstance);
                if(i != transform.childCount - 2) 
                    LevelLength += levelPiece.MainSpline.CalculateLength();
                _levelPieces.Add(levelPiece);
            }
        }

        InitBackground();

        _tileCollector = PlayerInstance.TileCollector;
        var spline = _levelPieces[0].GetSpline(Vector3.zero);
        PlayerInstance.Movement.SplineFollower.spline = spline;
        PlayerInstance.Movement.SplineFollower.onEndReached += SwitchToNextPiece;
        return PlayerInstance;
    }

    private void InitBackground()
    {
        SplinePoint[] points = new SplinePoint[_levelPieces.Count + 1];

        _backgroundSpline = Instantiate(_backgroundSplinePrefab, transform);

        for(int i = 0; i < _levelPieces.Count; i++)
        {
            Vector3 bgPosition = _levelPieces[i].transform.position;
            bgPosition.y = 0;
            points[i] = new SplinePoint();
            points[i].position = bgPosition;
        }

        int last = _levelPieces.Count;
        points[last] = new SplinePoint();
        points[last].position = _levelPieces[last - 1].transform.position + 
            (_levelPieces[last - 1].transform.forward * (_prefabOffset + 200 + 
            (15 * YandexGame.savesData.ScoreLevel)));

        _backgroundSpline.SetPoints(points);
        StartCoroutine(SpawnDelay());
    }

    private IEnumerator SpawnDelay()
    {
        yield return new WaitForEndOfFrame();
        
        Transform splineTransform = _backgroundSpline.transform;
        float splineLength = _backgroundSpline.CalculateLength();

        ObjectController objectController = _backgroundSpline.GetComponent<ObjectController>();

        objectController.objects[0] = _backgroundPrefab;

        objectController.minObjectDistance = _prefabOffset;
        objectController.maxObjectDistance = _prefabOffset;

        objectController.spawnCount = 1 + (int)(splineLength / _prefabOffset); 

        objectController.Spawn();

    }

    protected void SwitchToNextPiece(double obj = 0d)
    {
        _levelPieces[_currentPieceIndex].isPlayerOnThisPeace = false;
        _currentPieceIndex++;
        if (_currentPieceIndex >= _levelPieces.Count)
        {
            PlayerInstance.Movement.AllowMovement(false);
            return;
        }
        _levelPieces[_currentPieceIndex].isPlayerOnThisPeace = true;
        var spline = _levelPieces[_currentPieceIndex].GetSpline(_tileCollector.transform.position);
        PlayerInstance.Movement.ChangeSpline(spline);
    }

    public void ShuffleColors()
    {
        for (var i = 0; i < _levelTileColors.Length; ++i)
        {
            var r = Random.Range(i, _levelTileColors.Length);
            var tmp = _levelTileColors[i];
            _levelTileColors[i] = _levelTileColors[r];
            _levelTileColors[r] = tmp;
        }
    }
}
