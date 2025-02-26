using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

public class Hammer : Weapon
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
                    isAttacking = true;
                    Vector2 swingDirection = enemy.position - transform.position;
                    targetsInSwing = weaponDetect.GetTargetsInConeAndTargetFarest(swingDirection, swingAngle, out Transform targetFarest);
                    
                    float startAngle = transform.eulerAngles.z + swingAngle/2;
                    centerPoint = transform.position;
                    startSwingPosition = transform.position + Quaternion.Euler(0, 0, startAngle) * Vector3.right * Vector2.Distance(transform.position, targetFarest.position);
                    RotateToTargetDirection(Quaternion.Euler(0, 0, startAngle) * Vector3.right);
                    
                    attackTimer = 0;
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
