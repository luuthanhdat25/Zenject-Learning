using UnityEngine;
using Utils;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;

    private Vector2 targetPosition;

    public void Init(Vector2 targetPosition, float range)
    {
        this.targetPosition = targetPosition;
        transform.RotateLootAt(targetPosition); 
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

    //private void OnCollisionEnter(Collision collision)
    //{
    //    //Instantiate(gunShoot.ImpactParticle, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
    //    //Instantiate(gunShoot.DecalParticle, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));

        
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet hit!");
        Destroy(gameObject);
    }
}
