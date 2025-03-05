using TMPro;
using UnityEngine;
using Utils;
using Zenject;

public class UIDamageFloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float moveToTargetPositionSpeed = 5f;

    private DamageFloatingTextPool _damageFloatingTextPool;
    private Vector2 _targetMovePosition;
    private Transform _playerTransform;

    [Inject]
    public void Construct(DamageFloatingTextPool damageFloatingTextPool, Player player)
    {
        _damageFloatingTextPool = damageFloatingTextPool;
        _playerTransform = player.transform;
    }

    public void UpdateUI(string text, Color color, Vector2 spawnPosition)
    {
        Vector2 velocity = spawnPosition - (Vector2)_playerTransform.position;
        _targetMovePosition = velocity.normalized * velocity.magnitude + spawnPosition;
        damageText.text = text;
        damageText.color = color;
    }

    private void FixedUpdate()
    {
        transform.MoveToPosition(_targetMovePosition, moveToTargetPositionSpeed);
    }

    public void Despawn()
    {
        _damageFloatingTextPool.Despawn(this);
    }
}

public class DamageFloatingTextPool : MonoMemoryPool<UIDamageFloatingText> {}