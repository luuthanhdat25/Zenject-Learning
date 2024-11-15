using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    public GameInstaller.Settings GameInstaller;
    public Enemy.Settings Enemy;
    public EnemyManager.Settings EnemyManager;
    public Player.Settings Player;

    public override void InstallBindings()
    {
        Container.BindInstance(GameInstaller);
        Container.BindInstance(Enemy);
        Container.BindInstance(EnemyManager);
        Container.BindInstance(Player);
    }
}