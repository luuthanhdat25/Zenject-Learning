using System.Collections.Generic;
using UnityEngine;
using Zenject;

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
            _signalBus.Fire(new DealDamagePlayer() { Value = _settings.MaxHP });
            //Apply Damage To player
            isDespawned = true;
            _pool.Despawn(this);
        }
    }

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        _signalBus.Subscribe<PlayerDie>(OnStopMoving);
        currentHP = _settings.MaxHP;
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
        if((direction.x >= 0 && currentScale.x < 0) || (direction.x < 0 && currentScale.x > 0))
        {
            currentScale.x *= -1; 
            transform.localScale = currentScale;
        }
        rigidbody2D.velocity = direction * _settings.MoveSpeed;
    }

    /// <summary>
    /// Deducts HP by a given value, applying damage effects and handling critical hits.
    /// Despawns the object and removes it from the weapon's target list if HP reaches zero.
    /// </summary>
    /// <param name="value">The amount of HP to deduct.</param>
    /// <param name="isCrit">Indicates if the hit is critical.</param>
    /// <param name="weaponDetect">The weapon detection system to update target status.</param>
    public void DeductHP(int value, bool isCrit, WeaponDetect weaponDetect)
    {
        if (currentHP < 0) return;
        if (currentHP > 0)
        {
            currentHP -= value;
            if (currentHP < 0) currentHP = 0;

            //Got Damage Effect
            //Play Crit Effect if isCrit
        }

        if (currentHP == 0 && !isDespawned)
        {
            weaponDetect.RemoveTarget(this.transform);
            isDespawned = true;
            _pool.Despawn(this);
        }
    }

    [System.Serializable]
    public class Settings
    {
        public float MoveSpeed;
        public int MaxHP = 1;
    }

    public void ResetSetting(Settings settings)
    {
        if (settings == null) return;
        _settings = settings;
        currentHP = settings.MaxHP;
        isDespawned = false;
    }
}

public class EnemyPool : MonoMemoryPool<Enemy.Settings, Enemy>
{
    protected override void Reinitialize(Enemy.Settings settings, Enemy enemy)
    {
        enemy.ResetSetting(settings);
    }
}