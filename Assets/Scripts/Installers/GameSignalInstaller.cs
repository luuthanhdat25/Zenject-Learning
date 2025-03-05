using Zenject;

public class GameSignalInstaller : Installer<GameSignalInstaller>
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<DealDamagePlayer>();
        Container.DeclareSignal<UpdatePlayerHP>();
        Container.DeclareSignal<UpdatePlayerExperience>();
        Container.DeclareSignal<UpdatePlayerLevel>();
        Container.DeclareSignal<PlayerGetHit>();
        Container.DeclareSignal<PlayerDie>();
        Container.DeclareSignal<UpdateHPRegeneration>();
        Container.DeclareSignal<EnemyGetHit>();
    }
}

