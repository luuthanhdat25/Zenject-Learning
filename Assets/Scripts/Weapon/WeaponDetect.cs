using System.Collections.Generic;
using UnityEngine;

public class WeaponDetect : MonoBehaviour
{
    public Transform TargetEnemy { get; private set; }
    [SerializeField] private CircleCollider2D circleCollider2D;
    
    private Dictionary<Transform, Enemy> enemyDic = new ();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyDic.ContainsKey(collision.transform)) return;
        if(collision.TryGetComponent(out Enemy e))
        {
            enemyDic.Add(collision.transform, e);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemyDic.Remove(collision.transform);
    }

    private void Update() => UpdateNearestTarget();

    private void UpdateNearestTarget()
    {
        TargetEnemy = null;
        if (enemyDic.Count == 0) return;
        float minDistance = float.MaxValue;

        foreach (var target in enemyDic)
        {
            float distance = Vector2.Distance(transform.position, target.Key.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                TargetEnemy = target.Key;
            }
        }
    }

    public void RemoveTarget(Transform target)
    {
        if(!enemyDic.ContainsKey(target)) return;
        enemyDic.Remove(target);
    }

    public List<Enemy> GetEnemysInCone(Vector2 coneDirection, float coneAngle, float coneRadius)
    {
        List<Enemy> enemyList = new ();
        if (this.enemyDic.Count == 0) return enemyList;

        float validAngle = coneAngle / 2;
        foreach (var enemy in this.enemyDic)
        {
            Vector2 directionToEnemy = enemy.Key.position - transform.position;
            float angleGap = Vector2.Angle(directionToEnemy, coneDirection);

            if(angleGap <= validAngle)
            {
                enemyList.Add(enemy.Value);
            }
        }
        return enemyList;
    }

    public Enemy GetEnemyByTransform(Transform transform)
    {
        return enemyDic[transform];
    }

    public void SetDetectable(bool value)
    {
        circleCollider2D.enabled = value;
        if (!value) enemyDic.Clear();
    }

    public void SetRange(float range)
    {
        circleCollider2D.radius = range > 0.25f ? range : 0.25f;
    }

    public bool IsInDetectRange(Transform target)
    {
        return Vector2.Distance(target.position, transform.position) <= circleCollider2D.radius;
    }
}
