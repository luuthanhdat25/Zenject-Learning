using System.Collections.Generic;
using UnityEngine;

public class WeaponDetect : MonoBehaviour
{
    public Transform TargetEnemy { get; private set; }
    [SerializeField] private CircleCollider2D circleCollider2D;
    
    private List<Transform> targetList = new List<Transform>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        targetList.Add(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        targetList.Remove(collision.transform);
    }

    private void Update() => UpdateNearestTarget();

    private void UpdateNearestTarget()
    {
        TargetEnemy = null;
        if (targetList.Count == 0) return;
        float minDistance = float.MaxValue;

        foreach (var target in targetList)
        {
            float distance = Vector2.Distance(transform.position, target.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                TargetEnemy = target;
            }
        }
    }

    public void SetDetectable(bool value)
    {
        circleCollider2D.enabled = value;
        if (!value) targetList.Clear();
    }
}
