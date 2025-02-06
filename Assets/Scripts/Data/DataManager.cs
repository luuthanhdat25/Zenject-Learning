using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private GameSettingsInstaller gameSettingsInstaller;

    public List<Weapon.Settings> GetWeaponDatas()
    {
        return gameSettingsInstaller.Weapons;
    }
}
