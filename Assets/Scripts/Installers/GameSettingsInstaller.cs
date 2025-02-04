using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    public GameInstaller.Settings GameInstaller;
    public Enemy.Settings Enemy;
    public EnemyManager.Settings EnemyManager;
    public Player.Stats PlayerStats;
    public List<Weapon.Settings> Weapons;

    public override void InstallBindings()
    {
        Container.BindInstance(GameInstaller);
        Container.BindInstance(Enemy);
        Container.BindInstance(EnemyManager);
        Container.BindInstance(PlayerStats);
        Container.BindInstance(Weapons);
    }
}