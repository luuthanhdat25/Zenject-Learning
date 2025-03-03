using UnityEngine;
using Utils;

public class StabWeapon : Weapon
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] protected LayerMask enemyLayerMask;
    [SerializeField] protected Transform damagePoint;
    [SerializeField] protected float damageRadius = 0.5f;

    private Vector2 targetPosition;
    private State attackState = State.MoveToTargetPosition;

    private enum State
    {
        MoveToTargetPosition,
        MoveBackToDefaultPosition
    }

    protected override void HandleBehaviour()
    {
        var enemy = weaponDetect.TargetEnemy;

        if (!isAttacking)
        {
            attackTimer += Time.fixedDeltaTime;
            if(enemy != null)
            {
                RotateFollowTargetEnemy(enemy);
                if(attackTimer >= GetCurrentAttackSpeed())
                {
                    isAttacking = true;
                    targetPosition = enemy.position;
                    attackTimer = 0;
                }
            } 
        }
        else 
        {
            switch (attackState)
            {
                case State.MoveToTargetPosition:
                    if (transform.MoveToPosition(targetPosition, moveSpeed))
                    {
                        DealDamage();
                        attackState = State.MoveBackToDefaultPosition;
                    }
                    break;

                case State.MoveBackToDefaultPosition:
                    if (transform.MoveToPosition(_player.WeaponManager.GetFollowPosition(this), moveSpeed))
                    {
                        isAttacking = false;
                        attackState = State.MoveToTargetPosition;
                    }
                    break;
            }
        }

        if (!isAttacking)
        {
            if(enemy == null) RotateFollowInputDirection();
            transform.position = _player.WeaponManager.GetFollowPosition(this);
        }

        weaponDetect.SetDetectable(!isAttacking);
    }

    private void DealDamage()
    {
        Vector2 damagePosition = (Vector2)damagePoint.position;
        Collider2D hitEnemies = Physics2D.OverlapCircle(damagePosition, damageRadius, enemyLayerMask);

        if(hitEnemies != null)
        {
            if (hitEnemies.TryGetComponent(out Enemy e))
            {
                DealDamageToEnemy(e);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (damagePoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(damagePoint.position, damageRadius);
    }
}
