using System.Collections.Generic;
using UnityEngine;

public class WeaponDetect : MonoBehaviour
{
    public Transform NearestTarget { get; private set; }
    
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
        NearestTarget = null;
        if (targetList.Count == 0) return;
        float minDistance = float.MaxValue;

        foreach (var target in targetList)
        {
            float distance = Vector2.Distance(transform.position, target.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                NearestTarget = target;
            }
        }
    }
}
