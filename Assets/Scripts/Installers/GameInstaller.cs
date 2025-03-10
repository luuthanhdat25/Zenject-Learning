using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Transform worldSpaceCanvas;
    
    [Inject] private Settings _settings;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        GameSignalInstaller.Install(Container);
        EnemyInstaller.Install(Container);

        Container.BindInterfacesAndSelfTo<EnemyManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<Spawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<UIDamageFloatTextManager>().AsSingle();

        Container.BindMemoryPool<UIDamageFloatingText, DamageFloatingTextPool>()
            .WithInitialSize(10)
            .FromComponentInNewPrefab(_settings.DamageFloatingTextPrefab)
            .UnderTransform(worldSpaceCanvas);
    }

    [System.Serializable]
    public class Settings
    {
        public UIDamageFloatingText DamageFloatingTextPrefab;
    }
}
