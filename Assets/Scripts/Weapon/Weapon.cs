using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type
    {
        Blade,
        Elemental,
        Explosive,
        Gun,
        Precise
    }

    [System.Serializable]
    public class Settings 
    {
        [Header("Display Info")]
        public string Name;
        public Sprite Icon;
        public Weapon Prefab;
        public Type[] types;
        public int startTier = 1;

        [Header("Stats")]
        public LevelStat[] LevelStats;
    }

    [System.Serializable]
    public struct LevelStat
    {
        public int BaseDamage;
        public float MeleeDamageBonus;
        public float RangeDamageBonus;
        public float ElementalDamageBonus;
        public float AttackSpeedBonus;
        public float MaxHPBonus;
        public float RangeBonus;
        public float SpeedBonus;
        public float LuckBonus;
        public float LevelBonus;
        public float LifestealBonus;
        public float ArmorBonus;

        public float AttackSpeeds;
        public CritChange Cris;
        public float Range;
        public int Knockback;
        public float Lifesteal;
    }

    public struct CritChange
    {
        public float CritRate;
        public float DamageMultiplier;
    }
}
