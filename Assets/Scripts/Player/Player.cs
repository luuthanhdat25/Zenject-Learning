using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [field: SerializeField] public PlayerExperience PlayerExperience;
    [field: SerializeField] public PlayerWeaponManager WeaponManager;
    
    public Stats CurrentStats { get; private set; }
    private SignalBus signalBus;
    public int Level { get; private set; }

    [Inject]
    private void Construct(SignalBus signalBus, Stats stats)
    {
        this.signalBus = signalBus;
        CurrentStats = stats;
    }

    public void UpdateMaxHP(int hp)
    {
        CurrentStats.MaxHP += hp;
        if (CurrentStats.MaxHP < 1) CurrentStats.MaxHP = 1;
    }

    [System.Serializable]
    public class Stats
    {
        public float MaxHP = 5;
        public int HPRegeneration;
        public float Damage;
        public int MeleeDamage;
        public int RangedDamage;
        public int AttackSpeed;
        public int CritChance;
        public int Range = 2;
        public int Armor;
        public int Speed;
    }
}
