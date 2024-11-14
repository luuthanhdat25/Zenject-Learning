using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Border borderPrefab;
    [SerializeField] private Player playerPrefab;

    [Inject]
    private Settings _settings;

    public override void InstallBindings()
    {
        Container.Bind<Border>().FromComponentInNewPrefab(borderPrefab).AsSingle().NonLazy();
        Container.Bind<Player>().FromComponentInNewPrefab(playerPrefab).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<EnemyManager>().AsSingle();

        //Container.BindFactory<Enemy, Enemy.Factory>().FromMonoPoolableMemoryPool(
        //    x => x.WithInitialSize(3).FromComponentInNewPrefab(_settings.EnemyPrefab).UnderTransformGroup("EnemyPooll"));

        Container.BindMemoryPool<Enemy, EnemyPool>().WithInitialSize(1)
            .FromComponentInNewPrefab(_settings.EnemyPrefab)
            .UnderTransformGroup("EnemyPool");
    }

    [System.Serializable]
    public class Settings
    {
        public Enemy EnemyPrefab;
    }
}
