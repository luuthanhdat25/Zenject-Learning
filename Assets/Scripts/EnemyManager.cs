using UnityEngine;
using Zenject;

public class EnemyManager : IInitializable, IFixedTickable
{
    private BorderPoints _border;
    private EnemyPool _enemyPool;
    private Settings _settings;
    private SignalBus _signalBus;
    private float timer = 0;
    private bool isSpawn = true;

    [Inject]
    private void Construct(BorderPoints border, EnemyPool enemyPool, Settings settings, SignalBus signalBus)
    {
        this._border = border;
        this._enemyPool = enemyPool;
        this._settings = settings;
        this._signalBus = signalBus;
    }

    public void Initialize()
    {
        if (_settings.SpawnOnStart)
        {
            timer = _settings.TimeSpawn;
        }
        _signalBus.Subscribe<PlayerDie>(() => isSpawn = false);
    }

    private void SpawnEnemy()
    {
        Enemy enemy = _enemyPool.Spawn(null);
        enemy.transform.position = _border.GetRandomPositionInBorder();
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
        if (!isSpawn) return;
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
