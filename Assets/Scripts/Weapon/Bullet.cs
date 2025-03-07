using System;
using UnityEngine;
using Utils;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;

    private Vector2 targetPosition;
    private Action<Enemy> onDealDamage;
    public void Init(Vector2 targetPosition, float range, Action<Enemy> onDealDamage)
    {
        this.targetPosition = targetPosition;
        transform.RotateLootAt(targetPosition);
        this.onDealDamage = onDealDamage;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if(transform.MoveToPosition(targetPosition, moveSpeed))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy e))
        {
            onDealDamage(e);
            //Instantiate(gunShoot.ImpactParticle, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
            //Play hit vfx
            //Play sfx 
            Destroy(gameObject);
        }
    }
}
