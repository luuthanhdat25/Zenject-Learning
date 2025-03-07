using UnityEngine;
using Zenject;

public class BorderPoints : MonoBehaviour
{
    [SerializeField] private Transform topPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform botPoint;

    private float yMax, yMin, xMax, xMin;
    private Transform _playerTransform;

    [Inject]
    public void Construct(Player player)
    {
        _playerTransform = player.transform;
    }

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

    public Vector2 GetRandomPositionInBorder(float minDistanceToPlayer)
    {
        Vector2 playerPosition = _playerTransform.position;

        bool canMoveUp = topPoint.position.y - playerPosition.y > minDistanceToPlayer;
        bool canMoveDown = playerPosition.y - botPoint.position.y > minDistanceToPlayer;
        bool canMoveLeft = playerPosition.x - leftPoint.position.x > minDistanceToPlayer;
        bool canMoveRight = rightPoint.position.x - playerPosition.x > minDistanceToPlayer;

        float randomY;
        if (canMoveUp && canMoveDown)
        {
            randomY = (Random.Range(0, 2) == 0)
                ? Random.Range(playerPosition.y + minDistanceToPlayer, yMax)
                : Random.Range(yMin, playerPosition.y - minDistanceToPlayer);
        }
        else
        {
            randomY = canMoveUp
                ? Random.Range(playerPosition.y + minDistanceToPlayer, yMax)
                : Random.Range(yMin, playerPosition.y - minDistanceToPlayer);
        }

        float randomX;
        if (canMoveLeft && canMoveRight)
        {
            randomX = (Random.Range(0, 2) == 0)
                ? Random.Range(xMin, playerPosition.x - minDistanceToPlayer)
                : Random.Range(playerPosition.x + minDistanceToPlayer, xMax);
        }
        else
        {
            randomX = canMoveLeft
                ? Random.Range(xMin, playerPosition.x - minDistanceToPlayer)
                : Random.Range(playerPosition.x + minDistanceToPlayer, xMax);
        }

        return new Vector2(randomX, randomY);
    }
}
