using UnityEngine;
using Zenject;
using System;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    private Player _player;
    private Settings _settings;
    private EnemyPool _pool;
    private SignalBus _signalBus;
    private bool isMoveable = true;
    private float currentHP;
    private bool isDespawned = false;
    private const float KNOCKBACK_NORMALIZE = 50f;

    [SerializeField] private new Rigidbody2D rigidbody2D;

    [Inject]
    public void Construct(Settings settings, Player player, EnemyPool enemyPool, SignalBus signalBus)
    {
        _settings = settings;
        _player = player;
        _pool = enemyPool;
        _signalBus = signalBus;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == _player.gameObject && !isDespawned)
        {
            _signalBus.Fire(new DealDamagePlayer() { Value = _settings.Damage });
            Despawn();
        }
    }

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        _signalBus.Subscribe<PlayerDie>(OnStopMoving);
        ResetSetting();
    }

    public void ResetSetting()
    {
        currentHP = _settings.BaseHP;
        isMoveable = true;
        isDespawned = false;
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
        rigidbody2D.velocity = direction * _settings.Speed;
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
        _pool.Despawn(this);
    }

    [System.Serializable]
    public class Settings
    {
        public int BaseHP = 1;
        public int HPIncreasePerWave = 2;
        public float Speed = 200;
        public int Damage = 1;
        public float DamageIncreasePerWave = 0.6f;
        public bool canBeKnockBack = true;
    }
}

public class EnemyPool : MonoMemoryPool<Enemy.Settings, Enemy>
{
    protected override void OnSpawned(Enemy item)
    {
        base.OnSpawned(item);
        item.ResetSetting();
    }
}
