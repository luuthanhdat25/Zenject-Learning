using UnityEngine;

public class BorderPoints : MonoBehaviour
{
    [SerializeField] private Transform topPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform botPoint;

    private float yMax, yMin, xMax, xMin;

    private void Awake()
    {
        xMax = rightPoint.position.x;
        xMin = leftPoint.position.x;
        yMax = topPoint.position.y;
        yMin = botPoint.position.y;
    }

    public bool IsCrossOverBorder(Vector2 position)
    {
        return position.y >= yMax 
            || position.y <= yMin 
            || position.x >= xMax 
            || position.x <= xMin;   
    }

    public Vector2 GetRandomPositionInBorder()
    {
        float randomX = Random.Range(xMax, xMin);
        float randomY = Random.Range(yMax, yMin);
        return new Vector2(randomX, randomY);
    }
}
