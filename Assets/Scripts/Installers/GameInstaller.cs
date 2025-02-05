using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private BorderPoints border;
    [SerializeField] private Player playerPrefab;

    [Inject] private Settings _settings;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        GameSignalInstaller.Install(Container);

        Container.Bind<InputManager>().AsSingle();
        Container.Bind<BorderPoints>().FromComponentOn(border.gameObject).AsSingle().NonLazy();
        Container.Bind<Player>().FromComponentOn(playerPrefab.gameObject).AsSingle().NonLazy();
        //Container.BindInterfacesAndSelfTo<EnemyManager>().AsSingle();

        Container.BindMemoryPool<Enemy, EnemyPool>().WithInitialSize(5)
            .FromComponentInNewPrefab(_settings.EnemyPrefab)
            .UnderTransformGroup("EnemyPool");
    }

    [System.Serializable]
    public class Settings
    {
        public Enemy EnemyPrefab;
    }
}
