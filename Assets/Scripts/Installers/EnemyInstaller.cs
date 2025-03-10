using UnityEngine;
using Zenject;

public class EnemyInstaller : Installer<EnemyInstaller>
{
    [Inject] private Enemy.Settings _settings;

    public override void InstallBindings()
    {
        Container.BindFactory<Vector2, DeadKnightEnemy, DeadKnightEnemy.Factory>()
            .FromMonoPoolableMemoryPool(x => x.WithInitialSize(10)
                .FromComponentInNewPrefab(_settings.DeadKnightPrefab)
                .WithGameObjectName(_settings.DeadKnightPrefab.name));
    }
}
