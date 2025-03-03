using UnityEngine;
using Zenject;
using System;

public class Enemy : MonoBehaviour
{
    private Player _player;
    private Settings _settings;
    private EnemyPool _pool;
    private SignalBus _signalBus;
    private bool isMoveable = true;
    private int currentHP;
    private bool isDespawned = false;

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
            _signalBus.Fire(new DealDamagePlayer() { Value = currentHP });
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
        currentHP = _settings.MaxHP;
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
        rigidbody2D.velocity = direction * _settings.MoveSpeed;
    }

    public void DeductHP(int value, bool isCrit, WeaponDetect weaponDetect)
    {
        if (currentHP < 0) return;

        if (currentHP > 0)
        {
            currentHP -= value;
            if (currentHP < 0) currentHP = 0;

            // Got Damage Effect
            // Play Crit Effect if isCrit
        }

        if (currentHP == 0 && !isDespawned && weaponDetect.IsInDetectRange(transform))
        {
            Despawn();
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
        public float MoveSpeed;
        public int MaxHP = 1;
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
