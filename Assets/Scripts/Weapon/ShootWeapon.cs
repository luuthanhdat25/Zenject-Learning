using UnityEngine;

public class ShootWeapon : Weapon
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Animator animator;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private AnimationClip shootClip;

    private float shootAnimationTime;

    private void Awake()
    {
        shootAnimationTime = shootClip.length;
    }

    private enum AnimationParam
    {
        AttackingMultiplier,
        AttackTrigger
    }

    protected override void HandleBehaviour()
    {
        var enemy = weaponDetect.TargetEnemy;

        attackTimer += Time.fixedDeltaTime;
        if (enemy != null)
        {
            RotateFollowTargetEnemy(enemy.transform);
            
            float currentAttackSpeed = GetCurrentAttackSpeed();
            if(attackTimer >= currentAttackSpeed)
            {
                attackTimer = 0;
                SpawnBullet(enemy);
                PlayShootAnimation(currentAttackSpeed);
            }
        } 
        else 
        {
            RotateFollowInputDirection();
        }

        transform.position = _player.WeaponManager.GetFollowPosition(this);
    }

    private void SpawnBullet(Transform enemy)
    {
        Bullet bullet = Instantiate(bulletPrefab);
        bullet.transform.position = shootingPoint.position;
        Vector2 targetPosition = (enemy.position - shootingPoint.position).normalized * GetCurrentRange() + shootingPoint.position;
        bullet.Init(targetPosition, DealDamageToEnemy);
    }

    private void PlayShootAnimation(float attackSpeed)
    {
        float multiplier = shootAnimationTime / attackSpeed;
        multiplier = multiplier > 1 ? multiplier : 1; // Shoot animation shouldn't slower default speed
        animator.SetFloat(AnimationParam.AttackingMultiplier.ToString(), multiplier);
        animator.SetTrigger(AnimationParam.AttackTrigger.ToString());
    }
}
