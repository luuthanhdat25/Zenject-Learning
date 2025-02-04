using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour
{
    private Player _player;
    private Settings _settings;
    private EnemyPool _pool;
    private SignalBus _signalBus;
    private bool hasCollided = false;
    private bool isMoveable = true;

    [SerializeField] private new Rigidbody2D rigidbody2D;

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
        if((direction.x >= 0 && currentScale.x < 0) || (direction.x < 0 && currentScale.x > 0))
        {
            currentScale.x *= -1; 
            transform.localScale = currentScale;
        }
        rigidbody2D.velocity = direction * _settings.MoveSpeed;
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