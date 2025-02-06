using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponDetect weaponDetect;
    
    protected Settings _settings;
    protected InputManager _inputManager;
    protected int currentLevel;
    protected PlayerWeaponManager _weaponManager;
    protected bool isAttacking = false;

    public virtual void Init(Settings settings, PlayerWeaponManager weaponManager, InputManager inputManager)
    {
        _settings = settings;
        _inputManager = inputManager;
        currentLevel = settings.StartTier; 
        _weaponManager = weaponManager;
    }

    public virtual void Init(Settings settings, PlayerWeaponManager weaponManager, InputManager inputManager, int tier)
    {
        _settings = settings;
        _inputManager = inputManager;
        _weaponManager = weaponManager;

        if (settings.IsValidTier(tier))
        {
            currentLevel = tier;
        }
        else
        {
            Debug.LogError("Tier invalid!");
        }
    }

    protected virtual void FixedUpdate()
    {
        HandleBehaviour();
    }
    
    protected virtual void RotateFollowInputDirection()
    {
        RotateToTargetDirection(_inputManager.GetMoveInput());
    }

    protected virtual void RotateFollowTargetEnemy(Transform target)
    {
        if (target == null) return;
        Vector2 targetDirection = target.position - transform.position;
        RotateToTargetDirection(targetDirection);
    }

    protected virtual void RotateToTargetDirection(Vector2 direciton)
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direciton.y, direciton.x) * Mathf.Rad2Deg);
    }

    protected abstract void HandleBehaviour();

    #region Data Config
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
        

        [Header("Stats")]
        public TierStat[] TierStats;

        private const int MAX_NUMBER_LEVEL = 4;

        public int StartTier => MAX_NUMBER_LEVEL - TierStats.Length + 1;

        public bool IsValidTier(int tier) => tier >= StartTier && tier <= MAX_NUMBER_LEVEL; 

        public TierStat GetTierStatsByTier(int tier)
        {
            if (tier > MAX_NUMBER_LEVEL) return default;
            int index = MAX_NUMBER_LEVEL - tier;
            return index >= TierStats.Length
                ? default
                : TierStats[index];
        }
    }

    [System.Serializable]
    public struct TierStat
    {
        public int BaseDamage;
        public BonusDamage BonusDamage;
        public float AttackSpeeds;
        public CritChange Cris;
        public float Range;
        public int Knockback;
        public float Lifesteal;
    }

    [System.Serializable]
    public struct BonusDamage
    {
        public float MeleeDamage;
        public float RangeDamage;
        public float ElementalDamage;
        public float AttackSpeed;
        public float MaxHP;
        public float Range;
        public float Speed;
        public float Luck;
        public float Level;
        public float Lifesteal;
        public float Armor;
    }

    [System.Serializable]
    public struct CritChange
    {
        public float CritRate;
        public float DamageMultiplier;
    }
    #endregion
}
