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

    public List<Transform> GetTargetsInConeAndTargetFarest(Vector2 coneDirection, float coneRadius, out Transform targetFarest)
    {
        List<Transform> targetList = new ();
        targetFarest = null;
        if (this.targetList.Count == 0) return targetList;

        float validAngle = coneRadius / 2;
        float farestDistance = 0;
        foreach (var target in this.targetList)
        {
            Vector2 directionToEnemy = target.position - transform.position;

            float angleGap = Vector2.Angle(directionToEnemy, coneDirection);
            if(angleGap <= validAngle)
            {
                targetList.Add(target);
                if(targetFarest == null)
                {
                    targetFarest = target;
                    farestDistance = Vector2.Distance(target.position, transform.position);
                }else
                {
                    float newDistance = directionToEnemy.magnitude;
                    if (newDistance > farestDistance)
                    {
                        targetFarest = target;
                        farestDistance = newDistance;
                    }
                }
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
}
