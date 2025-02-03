using UnityEngine;

public class Weapon : MonoBehaviour
{
    [System.Serializable]
    public class Settings 
    {
        [Header("Display Info")]
        public string Name;
        public Sprite Icon;

        [Header("Stats")]
        public int MaxLevel;
        public int BaseDamaage;
        public float Speed; 
        public float Duration;
        public float CoolDown;
        public float HitboxDelay;
        public int PoolLimit;
        public float CritMulti;
        public float Rarity;
        public int Amount;
        public float Area;
        public int Pierce;
        public float ProjectileInterval;
        public float Knockback;
        public float Chance;
        public bool BlockByWalls;
    }
}
