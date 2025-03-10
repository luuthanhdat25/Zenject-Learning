using UnityEngine;
using Zenject;

public class Spawner
{
    private DeadKnightEnemy.Factory _deadKnightFactory;
    
    [Inject]
    public void Construct(DeadKnightEnemy.Factory deadKnightFactory)
    {
        _deadKnightFactory = deadKnightFactory;
    }

    public Enemy CreateEnemy(string id, Vector2 position)
    {
        switch (id)
        {
            case GameDefine.EnemyEntity.DEAD_KNIGHT:
                return _deadKnightFactory.Create(position);

            default:
                Debug.LogWarning("Enemy has Id: " + id + " isn't define");
                break;
        }
        return null;
    }
}
