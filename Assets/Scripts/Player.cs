using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [field: SerializeField] public PlayerExperience PlayerExperience;
    
    public Stats CurrentStats { get; private set; }
    private SignalBus signalBus;

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

    public void UpdateDodge(int value)
    {
        CurrentStats.Dodge += value;
        if (CurrentStats.Dodge > 60) CurrentStats.Dodge = 60;
    }

    [System.Serializable]
    public class Stats
    {
        public int MaxHP = 5;
        public int HPRegeneration;
        public int Lifesteal;
        public float Damage;
        public int MeleeDamage;
        public int RangedDamage;
        public int ElementalDamage;
        public int AttackSpeed;
        public int CritChance;
        public int Range = 2;
        public int Armor;
        public int Dodge;
        public int Speed;
        public int Luck;
        public int Harvesting;
    }
}
