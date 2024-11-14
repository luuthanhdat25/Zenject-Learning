using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour/*, IPoolable<IMemoryPool>, IDisposable*/
{
    private Player _player;
    private Settings _settings;
    private EnemyPool _pool;

    [Inject]
    public void Construct(Settings settings, Player player, EnemyPool enemyPool)
    {
        _settings = settings;
        _player = player;
        _pool = enemyPool;
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector3 playerPosition = _player.GetPosition();

        float distance = Vector2.Distance(transform.position, playerPosition);

        if (distance <= 0.1f)
        {
            _pool.Despawn(this);
        }

        Vector3 direction = (playerPosition - transform.position).normalized;
        transform.position += direction * _settings.moveSpeed * Time.deltaTime;
    }

    /*public void OnDespawned()
    {
        _pool = null;
    }

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }


    public void Dispose()
    {
        _pool.Despawn(this);
    }*/

    [System.Serializable]
    public class Settings
    {
        public float moveSpeed;
    }

    //public class Factory : PlaceholderFactory<Enemy>
    //{
    //}

    public void ResetSetting(Settings settings)
    {
        if (settings == null) return;
        _settings = settings;
    }
}


public class EnemyPool : MonoMemoryPool<Enemy.Settings, Enemy>
{
    protected override void Reinitialize(Enemy.Settings settings, Enemy enemy)
    {
        enemy.ResetSetting(settings);
    }
}