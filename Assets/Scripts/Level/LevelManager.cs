using System.Collections.Generic;
using UnityEngine;
using YG;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private ColorMaterialsSet _colorMaterials;
    [SerializeField] private int _minRepeatLevelIndex = 10;
    [SerializeField] private bool _overrideLevel;
    [SerializeField] private int _overriddenLevel;
    [SerializeField] private List<Level> _levels;
    [SerializeField] private List<Level> _bonusLevels;
    [SerializeField] private int _backgroundChangeFrequency = 5;
    [SerializeField] private List<GameObject> _backgroundPrefabs;
    //[SerializeField] private Level _randomlyGeneratedLevel;

    private int _currentLevelIndex = 0;
    
    public List<Level> Levels => _levels;
    public ColorMaterialsSet ColorMaterials => _colorMaterials;
    public Level CurrentLevelInstance { get; private set; }
    public Player PlayerInstance { get; private set; }
    public float LevelProgress {  get; private set; }
    public float MaxLevelProgress { get; private set; }
    public int CurrentLevel { get; private set; }
    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void OnEnable()
    {
        GameStateManager.OnStateChange += HangleGameStateChange;
    }

    private void OnDisable()
    {
        GameStateManager.OnStateChange -= HangleGameStateChange;
    }

    public void InstantiateLevel()
    {
        int prebuildedLevels = _levels.Count;
        CurrentLevel = GameDataManager.GameData.Level;

        if (_overrideLevel) CurrentLevel = _overriddenLevel;

        _currentLevelIndex = -1;

        for(int i = 0; i < CurrentLevel; i++)
        {
            _currentLevelIndex++;
            int bonusLevels = Mathf.FloorToInt((float)prebuildedLevels / 4);
            if (_currentLevelIndex - bonusLevels > prebuildedLevels - 1)
            {
                bonusLevels = Mathf.FloorToInt((float)_minRepeatLevelIndex / 4);
                _currentLevelIndex = _minRepeatLevelIndex + bonusLevels;
            }
        }
        CurrentLevelInstance = SpawnPrebuildedlevel(_currentLevelIndex);

        int backgroundIndex = Mathf.FloorToInt((float)(CurrentLevel - 1) / _backgroundChangeFrequency);
        if(backgroundIndex >= _backgroundPrefabs.Count)
            backgroundIndex -= _backgroundPrefabs.Count * Mathf.FloorToInt((float)backgroundIndex / _backgroundPrefabs.Count);
        Debug.Log("Background with index => " + backgroundIndex);
        PlayerInstance = CurrentLevelInstance.Initialize(_playerPrefab, _backgroundPrefabs[backgroundIndex], false);
        SetMaterialsColors(CurrentLevelInstance);
    }

    private Level SpawnPrebuildedlevel(int index)
    {
        int curLevel = index + 1;
        int levelIndex = index - (int)((float)curLevel / 5);

        if (CurrentLevel % 5 == 0)
        {
            int bonusIndex = Mathf.FloorToInt((float)(CurrentLevel - 1) / 5);
            if (bonusIndex >= _bonusLevels.Count)
                bonusIndex -= _bonusLevels.Count * Mathf.FloorToInt((float)bonusIndex / _bonusLevels.Count);
            Debug.Log("Loaded prebuilded bonus level with index => " + bonusIndex);
            return Instantiate(_bonusLevels[bonusIndex], transform);
        }
        Debug.Log("Loaded prebuilded level with index => " + levelIndex);
        return Instantiate(Levels[levelIndex], transform);
    }

    private int _prevRandomIndex = 0;

    private Level GetRandomBonusLevel()
    {
        int randomIndex = Random.Range(0, _bonusLevels.Count);
        Debug.Log("Loaded prebuilded random bonus level with index => " + randomIndex);
        return Instantiate(_bonusLevels[randomIndex], transform);
    }

    //private Level RandomlyChooseLevel()
    //{
    //    if (YandexGame.savesData.Level % 5 == 0)
    //    {
    //        return GetRandomBonusLevel();
    //    }
    //    else
    //    {
    //        int randomIndex = 0;
    //        while(randomIndex == _prevRandomIndex)
    //            randomIndex = Random.Range(_minRepeatLevelIndex, _levels.Count);
    //        Debug.Log("Level randomly chosen index => " + randomIndex);
    //        _prevRandomIndex = randomIndex;
    //        return Instantiate(Levels[randomIndex], transform);
    //    }
    //}

    private void SetMaterialsColors(Level level)
    {
        for (int i=0; i < 4; i++)
        {
            ColorMaterials.Materials[i].color = level.LevelColors[i];
        }
    }

    private void HangleGameStateChange(GameState state)
    {
        if (state == GameState.Finish)
        {
            YandexGame.savesData.Level ++;
            YandexGame.SaveProgress();
        }
    }
}
