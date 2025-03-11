using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "WeaponSettingInstaller", menuName = "Installers/WeaponSettingInstaller")]
public class WeaponSettingInstaller : ScriptableObjectInstaller<WeaponSettingInstaller>
{
    public List<Weapon.Settings> Weapons;

    public override void InstallBindings()
    {
        Container.BindInstance(Weapons);
    }
}