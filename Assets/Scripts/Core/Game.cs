using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Game : MonoBehaviour, ICleanUp
{
    [SerializeField] private LevelFactory _levelFactory;
    [SerializeField] private EnemyFactory _enemyFactory;

    [SerializeField] private Player _playerPrefab;

    [SerializeField] private GameHud _gameHud;
    [SerializeField] private GameResultWindow _gameResultWindow;

    [SerializeField] private CameraController _cameraController;

    private Player _player;

    private Level _level;
    private int _levelNumber;
    private int _coinsCount;

    public IEnumerable<GameObjectFactory> Factories => new GameObjectFactory[] { _levelFactory, _enemyFactory };

    public string SceneName => Constants.Scenes.GAME;

    public void Initialize()
    {
        _gameResultWindow.Initialize(Restart, GoToMainMenu);

        _coinsCount = 0;
        _player = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity, transform);
        _player.Initialize(PlayerDie);

        _levelNumber = 0;
        _level = _levelFactory.Get(_levelNumber);
        _level.Initialize(LevelCompleted, _enemyFactory, _player);

        _cameraController.Initialize(_player);

        _gameHud.StartCountdown(() => _gameHud.Initialize(_player.ChangedInput, OnPauseClicked, GoToMainMenu));
    }

    private void FixedUpdate()
    {
        _gameHud.GameUpdate();
        _player?.GameUpdate();
        _cameraController.GameUpdate();
        _level?.GameUpdate();
    }

    private void OnPauseClicked(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
    }

    private void GoToMainMenu()
    {
        OnPauseClicked(false);
        var operations = new Queue<ILoadingOperation>();
        operations.Enqueue(new ClearGameOperation(this));
        LoadingScreen.Instance.Load(operations);
    }

    private void LevelCompleted()
    {
        _coinsCount += _level.AccumulatedCoins;

        _level.Recycle();
        ++_levelNumber;
        if (_levelNumber < _levelFactory.LevelsCount)
        {
            _level = _levelFactory.Get(_levelNumber);
            _level.Initialize(LevelCompleted, _enemyFactory, _player);
        }
        else
        {
            _gameResultWindow.Show(GameResultType.Win, _coinsCount);
        }
    }

    private void PlayerDie()
    {
        OnPauseClicked(true);
        _gameResultWindow.Show(GameResultType.Lost, _coinsCount + _level.AccumulatedCoins);
        SaveCoin();
    }

    private void SaveCoin()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            _coinsCount += PlayerPrefs.GetInt("Coins") + _level.AccumulatedCoins;
        }

        PlayerPrefs.SetInt("Coins", _coinsCount);
    }

    private void Restart()
    {
        _level.Recycle();
        _levelNumber = 0;
        _coinsCount = 0;
        _player.Revival();
        _level = _levelFactory.Get(_levelNumber);
        _level.Initialize(LevelCompleted, _enemyFactory, _player);
        OnPauseClicked(false);
    }

    public void Cleanup()
    {
        //_level.Recycle();
        _player.Recycle();
    }
}