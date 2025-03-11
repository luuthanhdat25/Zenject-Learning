using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "EnemySettingInstaller", menuName = "Installers/EnemySettingInstaller")]
public class EnemySettingInstaller : ScriptableObjectInstaller<EnemySettingInstaller>
{
    public EnemyManager.Settings EnemyManager;
    public Enemy.Settings Enemies;

    public override void InstallBindings()
    {
        Container.BindInstance(EnemyManager);
        Container.BindInstance(Enemies);
    }
}