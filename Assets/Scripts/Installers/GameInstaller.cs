using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject] private Settings _settings;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        GameSignalInstaller.Install(Container);

        Container.BindInterfacesAndSelfTo<EnemyManager>().AsSingle();

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
