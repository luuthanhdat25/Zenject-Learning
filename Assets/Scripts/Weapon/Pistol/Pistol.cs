using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Animator animator;
    [SerializeField] private Bullet bulletPrefab;

    protected override void HandleBehaviour()
    {
        var enemy = weaponDetect.TargetEnemy;

        if (!isAttacking)
        {
            attackTimer += Time.fixedDeltaTime;
            if (enemy != null && attackTimer >= GetCurrentAttackSpeed())
            {
                isAttacking = true;
                RotateFollowTargetEnemy(enemy.transform);
                Bullet bullet = Instantiate(bulletPrefab);
                bullet.transform.position = shootingPoint.position;
                bullet.Init(enemy.position, GetCurrentStats().Range);
                attackTimer = 0;
            }
        }
        else
        {
            isAttacking = false;
        }

        if (!isAttacking)
        {
            //RotateFollowInputDirection();
        }
        transform.position = _player.WeaponManager.GetFollowPosition(this);

        //weaponDetect.SetDetectable(!isAttacking);
    }

    public Vector3 ShootingPosition() => shootingPoint.position;
}
