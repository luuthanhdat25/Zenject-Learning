using UnityEngine;

public class Border : MonoBehaviour
{
    [SerializeField] public Transform topLeftPoint;
    [SerializeField] public Transform botLeftPoint;
    [SerializeField] public Transform topRightPoint;
    [SerializeField] public Transform botRightPoint;

    public Vector2 GetRandomPositionOnBorder()
    {
        int edgeRandom = Random.Range(0, 4);
        if(edgeRandom == 0) return GetRandomPointBetween(topLeftPoint, botLeftPoint);
        if(edgeRandom == 1) return GetRandomPointBetween(topLeftPoint, topRightPoint);
        if(edgeRandom == 2) return GetRandomPointBetween(botRightPoint, topRightPoint);
        return GetRandomPointBetween(botLeftPoint, botRightPoint);
    }

    public Vector2 GetRandomPointBetween(Vector2 pointA, Vector2 pointB)
    {
        float t = Random.Range(0f, 1f);
        Vector2 randomPoint = Vector2.Lerp(pointA, pointB, t);
        return randomPoint;
    }

    public Vector2 GetRandomPointBetween(Transform pointA, Transform pointB)
    {
        float t = Random.Range(0f, 1f);
        Vector2 randomPoint = Vector2.Lerp(pointA.position, pointB.position, t);
        return randomPoint;
    }
}
