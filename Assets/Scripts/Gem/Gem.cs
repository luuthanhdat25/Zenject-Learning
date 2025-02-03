using System.Collections;
using UnityEngine;
using Zenject;

public class Gem : MonoBehaviour
{
    public enum GemType
    {
        Blue = 1,
        Green = 2,
        Pink = 5,
        Purple = 10,
        Red = 20
    }

    [SerializeField] private float sinOutDistance = 0.5f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private GemType Type;
    [Inject] private Player _player;
    private Vector3 targetPosition;
    private bool isMove = false;
    private bool isMoveToPlayer = false;

    public void Collect()
    {
        isMove = true;
        targetPosition = GetOutSinPosition(_player.transform.position);
    }

    private void FixedUpdate()
    {
        if (!isMove) return;
        if(isMoveToPlayer) targetPosition = _player.GetPosition(); 

        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.fixedDeltaTime;
        
        if(Vector3.Distance(targetPosition, transform.position) <= 0.1f)
        {
            if (!isMoveToPlayer)
            {
                isMoveToPlayer = true;
            }
        }
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }

    private Vector3 GetOutSinPosition(Vector3 targetPosition)
    {
        Vector3 direction = (transform.position - targetPosition).normalized;
        return transform.position + sinOutDistance * direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<PlayerExperience>(out PlayerExperience playerExperience))
            {
                if (!isMove) Collect();
            }else if(collision.TryGetComponent<Player>(out Player player))
            {
                isMoveToPlayer = false;
                isMove = false;
                _player.PlayerExperience.Add((int)Type);
                Despawn();
            }
        }
    }
}
