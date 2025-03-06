using UnityEngine;

public class DealDamagePlayer
{
    public int Value;
}

public class UpdatePlayerHP
{
    public float CurrentHP;
    public float MaxHP;
}

public class PlayerGetHit { }
public class PlayerDie { }

public class UpdatePlayerExperience
{
    public int CurrentExperience;
}

public class UpdatePlayerLevel
{
    public int TargetExperience;
    public int PreLevel;
    public int NewLevel;
}

public class UpdateHPRegeneration { }

public class EnemyGetHit 
{
    public int DamageHit;
    public bool IsCrit;
    public Vector2 Position;
}