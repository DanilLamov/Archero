using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Factory", menuName = "Factories/Level")]
public class LevelFactory : GameObjectFactory
{
    [SerializeField] private Level[] _levelsPrefabs;

    public int LevelsCount => _levelsPrefabs.Length;

    public Level Get(int levelNumber)
    {
        Level instance = CreateGameObjectInstance(_levelsPrefabs[levelNumber]);
        instance.OriginFactory = this;
        return instance;
    }

    public void Reclaim(Level level)
    {
        Destroy(level.gameObject);
    }
}