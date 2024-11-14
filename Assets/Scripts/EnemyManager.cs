using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class EnemyManager : IFixedTickable
{
    private Border _border;
    //private Enemy.Factory _enemyFactory;
    private EnemyPool _enemyPool;
    private float timer;

    [Inject]
    private void Construct(Border border, EnemyPool enemyPool /*, Enemy.Factory enemyFactory*/)
    {
        this._border = border;
        this._enemyPool = enemyPool;
        //this._enemyFactory = enemyFactory;
    }

    private void SpawnEnemy()
    {
        /*Enemy enemy = _enemyFactory.Create();
        enemy.transform.position = _border.GetRandomPositionOnBorder();*/
        Enemy enemy = _enemyPool.Spawn(null);
        enemy.transform.position = _border.GetRandomPositionOnBorder();
        CountNumberInPool();
    }

    private void ClearAll()
    {
        _enemyPool.Clear();
    }

    public void CountNumberInPool()
    {
        Debug.Log("[Enemy Manager] Total In pool: " + _enemyPool.NumTotal);
        Debug.Log("[Enemy Manager] Total active: " + _enemyPool.NumActive);
        Debug.Log("[Enemy Manager] Total inactive: " + _enemyPool.NumInactive);
    }

    public void FixedTick()
    {
        timer += Time.fixedDeltaTime;
        if(timer >= 1)
        {
            SpawnEnemy();
            timer = 0;
        }
    }
}
