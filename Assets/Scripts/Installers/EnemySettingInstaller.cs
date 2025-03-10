using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "EnemySettingInstaller", menuName = "Installers/EnemySettingInstaller")]
public class EnemySettingInstaller : ScriptableObjectInstaller<EnemySettingInstaller>
{
    public override void InstallBindings()
    {
    }
}