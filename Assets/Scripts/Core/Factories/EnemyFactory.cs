using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Factory", menuName = "Factories/Enemy")]
public class EnemyFactory : GameObjectFactory
{
    [SerializeField] private Enemy _type1, _type2, _type3;

    public Enemy Get(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Type1:
                return Get(_type1);
            case EnemyType.Type2:
                return Get(_type2);
            case EnemyType.Type3:
                return Get(_type3);
        }

        return null;
    }

    private T Get<T>(T prefab) where T : Enemy
    {
        T instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }

    public void Reclaim(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
