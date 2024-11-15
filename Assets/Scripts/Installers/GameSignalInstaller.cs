using UnityEngine;
using Zenject;

public class GameSignalInstaller : Installer<GameSignalInstaller>
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<DealDamagePlayer>();
        Container.DeclareSignal<UpdatePlayerHealth>();
    }
}

