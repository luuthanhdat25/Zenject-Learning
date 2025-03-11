using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PlayerSettingInstaller", menuName = "Installers/PlayerSettingInstaller")]
public class PlayerSettingInstaller : ScriptableObjectInstaller<PlayerSettingInstaller>
{
    public Player.Stats PlayerStats;

    public override void InstallBindings()
    {
        Container.BindInstance(PlayerStats);
    }
}