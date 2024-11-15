using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour
{
    private Player _player;
    private Settings _settings;
    private EnemyPool _pool;
    private SignalBus _signalBus;
    private bool hasCollided = false;

    [Inject]
    public void Construct(Settings settings, Player player, EnemyPool enemyPool, SignalBus signalBus)
    {
        _settings = settings;
        _player = player;
        _pool = enemyPool;
        _signalBus = signalBus;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasCollided) return;
        if (collision.GetComponent<Player>() != null)
        {
            hasCollided = true;
            _signalBus.Fire(new DealDamagePlayer() { Value = _settings.Damage });
            _pool.Despawn(this);
        }
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector3 playerPosition = _player.GetPosition();

        float distance = Vector2.Distance(transform.position, playerPosition);

        Vector3 direction = (playerPosition - transform.position).normalized;
        transform.position += direction * _settings.MoveSpeed * Time.deltaTime;
    }

    [System.Serializable]
    public class Settings
    {
        public float MoveSpeed;
        public int Damage;
    }

    public void ResetSetting(Settings settings)
    {
        hasCollided = false;
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