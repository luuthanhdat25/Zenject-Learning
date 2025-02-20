using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] private float radius = .5f;

    private Dictionary<Weapon, Vector2> weaponDic = new ();
    private DataManager _dataManager;
    private InputManager _inputManager;
    private Player _player;

    [Inject]
    public void Construct(DataManager dataManager, InputManager inputManager, Player player)
    {
        _dataManager = dataManager;
        _inputManager = inputManager;
        _player = player;
    }

    private void Start()
    {
        Spawn(_dataManager.GetWeaponDatas()[1]);
        //Spawn(_dataManager.GetWeaponDatas().First());
        //Spawn(_dataManager.GetWeaponDatas().First());
        //Spawn(_dataManager.GetWeaponDatas().First());
        float gapRadius = (float)360 / weaponDic.Count;
        float startRotation = 90;

        var keys = weaponDic.Keys.ToList();

        foreach (var key in keys)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, startRotation);
            weaponDic[key] = GetPositionOnCircle(rotation);
            key.transform.localPosition = weaponDic[key];
            key.gameObject.SetActive(true);
            startRotation += gapRadius;
        }
    }

    private Weapon Spawn(Weapon.Settings data)
    {
        Weapon newWeapon = Instantiate(data.Prefab);
        newWeapon.Init(data, _player, _inputManager);
        weaponDic.Add(newWeapon, Vector2.zero);
        return newWeapon;
    }

    private Vector2 GetPositionOnCircle(Quaternion rotation)
    {
        float angle = rotation.eulerAngles.z * Mathf.Deg2Rad;

        float x = radius * Mathf.Cos(angle);
        float y = radius * Mathf.Sin(angle);

        return new Vector2(x, y);
    }

    public Vector2 GetFollowPosition(Weapon e) => weaponDic[e] + (Vector2)transform.position;
}
