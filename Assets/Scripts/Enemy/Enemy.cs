using UnityEngine;
using Zenject;
using System;
using DG.Tweening;
using Zenject.SpaceFighter;
using System.Collections.Generic;

public class Enemy : MonoBehaviour, IPoolable<Vector2, IMemoryPool>, IDisposable
{
    private Player _player;
    private IMemoryPool _pool;
    private Setting _setting;
    private SignalBus _signalBus;
    private bool isMoveable = true;
    private float currentHP;
    private bool isDespawned = false;
    private const float KNOCKBACK_NORMALIZE = 50f;

    [SerializeField] private new Rigidbody2D rigidbody2D;

    [Inject]
    public void Construct(Player player, SignalBus signalBus)
    {
        _player = player;
        _signalBus = signalBus;
    }

    public virtual void Init(Setting setting)
    {
        _setting = setting;
        currentHP = _setting.BaseHP;
        isMoveable = true;
        isDespawned = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == _player.gameObject && !isDespawned)
        {
            _signalBus.Fire(new DealDamagePlayer() { Value = _setting.Damage });
            Despawn();
        }
    }

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        _signalBus.Subscribe<PlayerDie>(OnStopMoving);
    }

    private void OnStopMoving()
    {
        isMoveable = false;
        rigidbody2D.velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!isMoveable) return;
        Vector3 playerPosition = _player.transform.position;

        Vector3 direction = (playerPosition - transform.position).normalized;
        Vector3 currentScale = transform.localScale;
        if ((direction.x >= 0 && currentScale.x < 0) || (direction.x < 0 && currentScale.x > 0))
        {
            currentScale.x *= -1;
            transform.localScale = currentScale;
        }
        rigidbody2D.velocity = direction * _setting.Speed;
    }

    public void DeductHP(int value, bool isCrit, WeaponDetect weaponDetect, int knockBack)
    {
        if (currentHP < 0) return;

        if (currentHP > 0)
        {
            _signalBus.Fire(new EnemyGetHit
            {
                DamageHit = value,
                IsCrit = isCrit,
                Position = transform.position
            });

            currentHP -= value;
            if (currentHP < 0) currentHP = 0;

            // Got Damage Effect
            // Play Crit Effect if isCrit
        }

        if (currentHP == 0 && !isDespawned && weaponDetect.IsInDetectRange(transform))
        {
            Despawn();
        }
        else
        {
            HandleBeKnockback(knockBack);
        }
    }

    private void HandleBeKnockback(int knockBack)
    {
        if (knockBack > 0)
        {
            Vector2 knockBackDirection = (transform.position - _player.transform.position).normalized;
            transform.position = (knockBackDirection * knockBack / KNOCKBACK_NORMALIZE) + (Vector2)transform.position;
        }
    }

    private void Despawn()
    {
        if (isDespawned) return;
        isDespawned = true;
        Dispose();
    }

    public void Dispose()
    {
        if (_pool == null) return;
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
        _pool = null;
        gameObject.SetActive(false);
    }

    public void OnSpawned(Vector2 spawnPosition, IMemoryPool pool)
    {
        _pool = pool;
        transform.position = spawnPosition;
    }

    [Serializable]
    public class Settings
    {
        public DeadKnightEnemy DeadKnightPrefab;
        
        public List<Setting> EnemySettings;
    }

    [Serializable]
    public class Setting
    {
        public string Id;
        public int BaseHP = 1;
        public int HPIncreasePerWave = 2;
        public float Speed = 200;
        public int Damage = 1;
        public float DamageIncreasePerWave = 0.6f;
        public bool canBeKnockBack = true;
    }
}