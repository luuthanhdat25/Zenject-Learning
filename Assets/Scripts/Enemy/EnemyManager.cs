using System.Linq;
using UnityEngine;
using Zenject;

public class EnemyManager : IInitializable, IFixedTickable
{
    private BorderPoints _border;
    private Spawner _spawner;
    private Settings _settings;
    private SignalBus _signalBus;
    private float timer = 0;
    private bool isSpawn = true;
    private Enemy.Settings _enemySettings;

    [Inject]
    private void Construct(BorderPoints border, Enemy.Settings enemySettings, Spawner spawner, Settings settings, SignalBus signalBus)
    {
        this._border = border;
        this._enemySettings = enemySettings;
        this._spawner = spawner;
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
        var spawnPosition = _border.GetRandomPositionInBorder(_settings.MinDistanceToPlayer);
        Enemy enemy = _spawner.CreateEnemy(_enemySettings.EnemySettings[0].Id, spawnPosition);
        enemy.Init(_enemySettings.EnemySettings[0]);
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
        public float MinDistanceToPlayer;
    }
}
