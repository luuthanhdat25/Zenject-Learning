using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponDetect weaponDetect;
    
    protected Settings _settings;
    protected InputManager _inputManager;
    protected int currentTier;
    protected bool isAttacking = false;
    protected Player _player;
    protected int damage;
    protected CritChange critChange;
    protected float attackTimer = 0;

    private void Awake()
    {
        if(weaponDetect == null)
        {
            weaponDetect = GetComponent<WeaponDetect>();
        }
    }

    public virtual void Init(Settings settings, Player player, InputManager inputManager, int tier = 0)
    {
        _settings = settings;
        _inputManager = inputManager;
        _player = player;

        if (tier == 0) 
        {
            currentTier = settings.StartTier;
        }
        else if (settings.IsValidTier(tier))
        {
            currentTier = tier;
        }
        else
        {
            Debug.LogError("Tier invalid!");
        }
        CalculateDamage();
        UpdateRange();
        attackTimer = GetCurrentAttackSpeed();
    }

    protected float GetCurrentAttackSpeed()
    {
        float attackSpeed = GetCurrentStats().AttackSpeed;
        if (attackSpeed > 0) return attackSpeed;
        Debug.LogError($"{nameof(Weapon.TierStat.AttackSpeed)} of weapon {_settings.Name} is not valid!");
        return -1;
    }

    public void CalculateDamage()
    {
        TierStat currentStats = GetCurrentStats();
        Player.Stats playerStats = _player.CurrentStats;
        int damageBonus =
            Mathf.RoundToInt((playerStats.MeleeDamage * currentStats.BonusDamage.MeleeDamage
            + playerStats.RangedDamage * currentStats.BonusDamage.RangeDamage
            + playerStats.Armor * currentStats.BonusDamage.Armor
            + playerStats.Range * currentStats.BonusDamage.Range
            ) / 100);
        damage = currentStats.BaseDamage + damageBonus;
        critChange = currentStats.Cris;
    }

    protected virtual void UpdateRange()
    {
        weaponDetect.SetRange(GetCurrentRange());
    }

    protected float GetCurrentRange()
    {
        return GetCurrentStats().Range + _player.CurrentStats.Range;
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
    protected virtual void FixedUpdate() => HandleBehaviour();

    protected abstract void HandleBehaviour();

    protected TierStat GetCurrentStats() => _settings.GetTierStatsByTier(currentTier);

    protected virtual void DealDamageToEnemy(Enemy enemy)
    {
        if (enemy == null) return;

        bool isCrit = IsCritDeal();
        int damageDealToEnemy = isCrit
            ? Mathf.RoundToInt(damage * critChange.DamageMultiplier)
            : damage;

        if (isCrit && damageDealToEnemy == damage) isCrit = false;
        //Debug.Log($"[{_settings.Name}] Damage Deal: {damageDealToEnemy}, isCrit: {isCrit}");
        enemy.DeductHP(damageDealToEnemy, isCrit, weaponDetect, GetCurrentStats().Knockback);
    }

    protected virtual bool IsCritDeal() => Random.Range(0, 100) < critChange.CritRate;

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
        public float AttackSpeed;
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
        public float Range;
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
