using UnityEngine;
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

public class DealDamagePlayer
{
    public int Value;
}

public class UpdatePlayerHP
{
    public float CurrentHP;
    public float MaxHP;
}

public class PlayerGetHit { }
public class PlayerDie { }

public class UpdatePlayerExperience
{
    public int CurrentExperience;
}

public class UpdatePlayerLevel
{
    public int TargetExperience;
    public int PreLevel;
    public int NewLevel;
}

public class UpdateHPRegeneration { }

public class EnemyGetHit
{
    public int DamageHit;
    public bool IsCrit;
    public Vector2 Position;
}

