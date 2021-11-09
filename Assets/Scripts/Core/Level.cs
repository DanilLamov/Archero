using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private Door _door;
    [SerializeField] private Transform _startPlayerPosition;

    [Header("Start Enemies Positions")]
    [SerializeField] public Transform[] _startEnemyType1Positions;
    [SerializeField] public Transform[] _startEnemyType2Positions;
    [SerializeField] public Transform[] _startEnemyType3Positions;

    public LevelFactory OriginFactory { get; set; }

    public int AccumulatedCoins => _coinsCount;

    private List<Enemy> _enemies;
    private List<Coin> _coins;
    private int _coinsCount;

    public void Initialize(Action levelCompleted, EnemyFactory enemyFactory, Player player)
    {
        _door.Initialize(levelCompleted);

        _enemies = new List<Enemy>();
        _coins = new List<Coin>();
        _coinsCount = 0;
        for (int j = 0; j < _startEnemyType1Positions.Length; ++j)
        {
            Enemy enemy = enemyFactory.Get(EnemyType.Type1);
            enemy.Initialize(player, EnemyDie, AfterEnemyDie);
            enemy.transform.position = _startEnemyType1Positions[j].position;
            enemy.transform.rotation = _startEnemyType1Positions[j].rotation;

            _enemies.Add(enemy);
        }
        for (int j = 0; j < _startEnemyType2Positions.Length; ++j)
        {
            Enemy enemy = enemyFactory.Get(EnemyType.Type2);
            enemy.Initialize(player, EnemyDie, AfterEnemyDie);
            enemy.transform.position = _startEnemyType2Positions[j].position;
            enemy.transform.rotation = _startEnemyType2Positions[j].rotation;

            _enemies.Add(enemy);
        }
        for (int j = 0; j < _startEnemyType3Positions.Length; ++j)
        {
            Enemy enemy = enemyFactory.Get(EnemyType.Type3);
            enemy.Initialize(player, EnemyDie, AfterEnemyDie);
            enemy.transform.position = _startEnemyType3Positions[j].position;
            enemy.transform.rotation = _startEnemyType3Positions[j].rotation;

            _enemies.Add(enemy);
        }

        if (_enemies.Count == 0)
        {
            _door.Open();
        }

        player.SetTargets(_enemies);
        player.transform.position = _startPlayerPosition.position;
        player.transform.rotation = _startPlayerPosition.rotation;
    }

    private void EnemyDie(Enemy enemy)
    {
        _enemies.Remove(enemy);
        if (_enemies.Count == 0)
        {
            _door.Open();
        }
    }

    private void AfterEnemyDie(Enemy enemy)
    {
        if (enemy.Type == EnemyType.Type2)
        {
            SpawnCoin(enemy.transform.position - new Vector3(.25f, 0f, .25f));
            SpawnCoin(enemy.transform.position + new Vector3(.25f, 0f, .25f));
            return;
        }

        SpawnCoin(enemy.transform.position);
    }

    private void SpawnCoin(Vector3 position)
    {
        Coin coin = Instantiate(_coinPrefab, position, Quaternion.identity, transform);
        coin.Initialize(CoinCollected);
        _coins.Add(coin);
    }

    private void CoinCollected(Coin coin)
    {
        _coinsCount += 10;
        _coins.Remove(coin);
    }

    public void GameUpdate()
    {
        foreach(Enemy enemy in _enemies)
        {
            enemy.GameUpdate();
        }

        foreach(Coin coin in _coins)
        {
            coin.GameUpdate();
        }
    }

    public void Recycle()
    {
        foreach (Enemy enemy in _enemies)
        {
            enemy.Recycle();
        }
        _enemies.Clear();

        foreach (Coin coin in _coins)
        {
            coin.Recycle();
        }
        _coins.Clear();

        OriginFactory.Reclaim(this);
    }
}