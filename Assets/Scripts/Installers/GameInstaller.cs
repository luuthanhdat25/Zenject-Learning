using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private BorderPoints border;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private DataManager dataManager;

    [Inject] private Settings _settings;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        GameSignalInstaller.Install(Container);

        Container.Bind<InputManager>().FromComponentOn(inputManager.gameObject).AsSingle().NonLazy();
        Container.Bind<DataManager>().FromComponentOn(dataManager.gameObject).AsSingle().NonLazy();
        Container.Bind<BorderPoints>().FromComponentOn(border.gameObject).AsSingle().NonLazy();
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
