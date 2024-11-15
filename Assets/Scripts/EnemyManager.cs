using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class EnemyManager : IFixedTickable
{
    private Border _border;
    private EnemyPool _enemyPool;
    private Settings _settings;
    private float timer = 0;

    [Inject]
    private void Construct(Border border, EnemyPool enemyPool, Settings settings)
    {
        this._border = border;
        this._enemyPool = enemyPool;
        this._settings = settings;


        
    }

    private void Start()
    {
        if (_settings.SpawnOnStart)
        {
            timer = _settings.TimeSpawn;
        }
    }

    private void SpawnEnemy()
    {
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
        if(timer >= _settings.TimeSpawn)
        {
            SpawnEnemy();
            timer = 0;
        }
    }

    [System.Serializable]
    public class Settings
    {
        public float TimeSpawn;
        public bool SpawnOnStart;
    }
}
