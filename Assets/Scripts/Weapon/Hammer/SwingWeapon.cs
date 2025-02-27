using System.Collections.Generic;
using UnityEngine;
using Utils;

public class SwingWeapon : Weapon
{
    [SerializeField] private float swingSpeed = 200f;
    [SerializeField] private float moveToSwing = 10f;
    [SerializeField] private float swingAngle = 90;

    private List<Transform> targetsInSwing;
    private float rotatedAngle;
    private Vector2 centerPoint;
    private Vector2 startSwingPosition;
    private State attackState = State.MoveToSwingPosition;

    private enum State
    {
        MoveToSwingPosition,
        Swing,
        MoveBackToDefaultPosition
    }

    protected override void HandleBehaviour()
    {
        var enemy = weaponDetect.TargetEnemy;

        if (!isAttacking)
        {
            attackTimer += Time.fixedDeltaTime;
            if (enemy != null)
            {
                RotateFollowTargetEnemy(enemy);
                if (attackTimer >= GetCurrentAttackSpeed())
                {
                    Vector2 swingDirection = enemy.position - transform.position;
                    targetsInSwing = weaponDetect.GetTargetsInCone(swingDirection, swingAngle);
                    float longestDistance = LongestTargetDistance(targetsInSwing);
                    
                    if(longestDistance > 0)
                    {
                        isAttacking = true;
                        attackTimer = 0;
                        
                        float startAngle = transform.eulerAngles.z + swingAngle/2;
                        centerPoint = transform.position;
                        startSwingPosition = transform.position + Quaternion.Euler(0, 0, startAngle) * Vector3.right * longestDistance;
                        RotateToTargetDirection(Quaternion.Euler(0, 0, startAngle) * Vector3.right);
                    }
                }
            }
        }
        else
        {
            switch (attackState)
            {
                case State.MoveToSwingPosition:
                    if (transform.MoveToPosition(startSwingPosition, moveToSwing))
                    {
                        attackState = State.Swing;
                    }
                    break;

                case State.Swing:
                    float rotateAngle = swingSpeed * Time.fixedDeltaTime;
                    rotatedAngle += rotateAngle;
                    transform.RotateAround(centerPoint, Vector3.forward, -rotateAngle);
                    if (rotatedAngle >= swingAngle / 2) DealDamage();
                    if (rotatedAngle >= swingAngle)
                    {
                        rotatedAngle = 0;
                        attackState = State.MoveBackToDefaultPosition;
                    }
                    break;

                case State.MoveBackToDefaultPosition:
                    if (transform.MoveToPosition(_player.WeaponManager.GetFollowPosition(this), moveToSwing))
                    {
                        attackState = State.MoveToSwingPosition;
                        isAttacking = false;
                    }
                    break;
            }
        }

        if (!isAttacking)
        {
            if (enemy == null) RotateFollowInputDirection();
            transform.position = _player.WeaponManager.GetFollowPosition(this);
        }

        weaponDetect.SetDetectable(!isAttacking);
    }

    private float LongestTargetDistance(List<Transform> targets)
    {
        if(targets == null || targets.Count == 0) return -1;
        
        float longestDistance = float.MinValue;
        foreach (var item in targets)
        {
            longestDistance = Mathf.Max(longestDistance, Vector2.Distance(transform.position, item.position)); 
        }
        return longestDistance;
    }

    private void DealDamage()
    {
        if (targetsInSwing == null) return;
        
        foreach (var item in targetsInSwing)
        {
            if (item.TryGetComponent(out Enemy e))
            {
                DealDamageToEnemy(e);
            }
        }
        targetsInSwing = null;
    }
}
