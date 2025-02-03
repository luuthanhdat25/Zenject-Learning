using Zenject;

public class GameSignalInstaller : Installer<GameSignalInstaller>
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<DealDamagePlayer>();
        Container.DeclareSignal<UpdatePlayerHealth>();
        Container.DeclareSignal<UpdatePlayerExperience>();
        Container.DeclareSignal<UpdatePlayerLevel>();
    }
}

