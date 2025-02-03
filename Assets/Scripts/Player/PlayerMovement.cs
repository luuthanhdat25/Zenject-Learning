using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private Transform graphicTransform;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [Inject] private Player.Settings _settings;

    private const string INPUT_HORIZONTAL = "Horizontal";
    private const string INPUT_VERTICAL = "Vertical";

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw(INPUT_HORIZONTAL);
        float moveY = Input.GetAxisRaw(INPUT_VERTICAL);

        Vector2 movement = new Vector2(moveX, moveY).normalized;

        if(moveX != 0) HandleFlipGraphic(moveX);
        rigidbody2D.velocity = movement * _settings.MoveSpeed;
        playerAnimation.SetIsRunning(movement != Vector2.zero);
    }

    private void HandleFlipGraphic(float moveX)
    {
        Vector3 currentScale = graphicTransform.localScale;
        if ((moveX > 0 && currentScale.x < 0) || (moveX < 0 && currentScale.x > 0))
        {
            currentScale.x *= -1;
            graphicTransform.localScale = currentScale;
        }
    }
}
