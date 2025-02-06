using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Knife : Weapon
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] protected LayerMask enemyLayerMask;
    [SerializeField] protected Transform damagePoint;
    [SerializeField] protected float damageRadius = 0.5f;

    private Vector2 targetPosition;
    private bool isMoveToTarget = false;

    protected override void HandleBehaviour()
    {
        var enemy = weaponDetect.TargetEnemy;

        if (enemy != null && !isAttacking)
        {
            isAttacking = true;
            RotateFollowTargetEnemy(enemy.transform);
            targetPosition = enemy.position;
        }
        else if (isAttacking)
        {
            if (!isMoveToTarget)
            {
                if (MoveToPosition(targetPosition, moveSpeed))
                {
                    isMoveToTarget = true;
                    DealDamage();
                }
            }
            else
            {
                if (MoveToPosition(_weaponManager.GetFollowPosition(this), moveSpeed))
                {
                    isAttacking = false;
                    isMoveToTarget = false;
                }
            }
        }

        if (!isAttacking)
        {
            RotateFollowInputDirection();
            transform.position = _weaponManager.GetFollowPosition(this);
        }

        weaponDetect.SetDetectable(!isAttacking);
    }

    private bool MoveToPosition(Vector2 position, float speed)
    {
        Vector2 currentPos = transform.position;
        float distance = Vector2.Distance(currentPos, position);
        float step = speed * Time.fixedDeltaTime;

        if (distance <= step)
        {
            transform.position = position;
            return true;
        }

        transform.position = Vector2.MoveTowards(currentPos, position, step);
        return false;
    }

    private void DealDamage()
    {
        Vector2 damagePosition = (Vector2)damagePoint.position;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(damagePosition, damageRadius, enemyLayerMask);

        Collider2D firstEnemyHit = hitEnemies.FirstOrDefault();
        if(firstEnemyHit != null)
        {
            if (firstEnemyHit.TryGetComponent(out Enemy e))
            {
                e.DeductHP(_settings.GetTierStatsByTier(currentLevel).BaseDamage);
            }
        }
    }

    
    private void OnDrawGizmos()
    {
        if (damagePoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(damagePoint.position, damageRadius);
    }
}
