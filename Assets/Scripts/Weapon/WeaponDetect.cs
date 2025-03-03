using System.Collections.Generic;
using UnityEngine;

public class WeaponDetect : MonoBehaviour
{
    public Transform TargetEnemy { get; private set; }
    [SerializeField] private CircleCollider2D circleCollider2D;
    
    private List<Transform> targetList = new List<Transform>();

    private const float NORMALIZE_DIVINE_RANGE = 100F;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetList.Contains(collision.transform)) return;
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

    public void RemoveTarget(Transform target)
    {
        if(!targetList.Contains(target)) return;
        targetList.Remove(target);
    }

    public List<Transform> GetTargetsInCone(Vector2 coneDirection, float coneAngle, float coneRadius)
    {
        List<Transform> targetList = new ();
        if (this.targetList.Count == 0) return targetList;

        float validAngle = coneAngle / 2;
        foreach (var target in this.targetList)
        {
            Vector2 directionToEnemy = target.position - transform.position;
            float angleGap = Vector2.Angle(directionToEnemy, coneDirection);

            if(angleGap <= validAngle)
            {
                targetList.Add(target);
            }
        }
        return targetList;
    }

    public void SetDetectable(bool value)
    {
        circleCollider2D.enabled = value;
        if (!value) targetList.Clear();
    }

    public void SetRange(float range)
    {
        float rangeNormalize = range / NORMALIZE_DIVINE_RANGE;
        circleCollider2D.radius = rangeNormalize > 0.25f ? rangeNormalize : 0.25f;
    }

    public bool IsInDetectRange(Transform target)
    {
        return Vector2.Distance(target.position, transform.position) <= circleCollider2D.radius;
    }
}
