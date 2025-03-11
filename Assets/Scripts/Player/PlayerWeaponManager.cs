using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] private float radius = .5f;
    [SerializeField] private string[] weaponsStart;
    
    private Dictionary<Weapon, Vector2> weaponDic = new ();
    private InputManager _inputManager;
    private Player _player;
    private SignalBus _signalBus;
    private List<Weapon.Settings> _weapons;

    [Inject]
    public void Construct
        (InputManager inputManager, 
        Player player,
        List<Weapon.Settings> weapon,
        SignalBus signalBus)
    {
        _weapons = weapon;
        _inputManager = inputManager;
        _player = player;
        _signalBus = signalBus;
    }

    private void Start()
    {
        radius += 0.1f * weaponsStart.Count();
        foreach (var weaponName in weaponsStart)
        {
            Weapon.Settings weaponData = _weapons.FirstOrDefault(w => w.Name == weaponName);
            if(weaponData != null)
            {
                Spawn(weaponData);
            }
        }
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

        _signalBus.Subscribe<PlayerDie>(DestroyAllWeapons);
    }

    private Weapon Spawn(Weapon.Settings data)
    {
        Weapon newWeapon = Instantiate(data.Prefab);
        newWeapon.Init(data, _player, _inputManager);
        newWeapon.name = $"{data.Name}_{weaponDic.Count}";
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

    private void DestroyAllWeapons()
    {
        foreach (var item in weaponDic)
        {
            Destroy(item.Key.gameObject);
        }
        weaponDic.Clear();
    }

    public Vector2 GetFollowPosition(Weapon e) => weaponDic[e] + (Vector2)transform.position;
}
