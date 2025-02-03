using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private Transform graphicTransform;
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private BorderPoints border;
    [Inject] private Player.Settings _settings;

    private const string INPUT_HORIZONTAL = "Horizontal";
    private const string INPUT_VERTICAL = "Vertical";

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float moveDistance = _settings.MoveSpeed * Time.fixedDeltaTime;
        Vector2 moveVector = GetMoveVector(moveDistance);

        if (moveVector != Vector2.zero) HandleFlipGraphic(moveVector.x);

        rigidbody2D.velocity = moveVector * _settings.MoveSpeed;
        playerAnimation.SetIsRunning(moveVector != Vector2.zero, _settings.MoveSpeed);
    }

    private Vector2 GetMoveVector(float moveDistance)
    {
        Vector2 moveInput = GetMoveInput();
        if (moveInput == Vector2.zero) return Vector2.zero;

        if (!IsNextMoveCrossBorder(moveInput, moveDistance))
            return moveInput.normalized;

        float moveInputY = moveInput.y;

        moveInput.y = 0; 
        if (!IsNextMoveCrossBorder(moveInput, moveDistance))
            return moveInput;

        moveInput.x = 0;
        moveInput.y = moveInputY;

        if (!IsNextMoveCrossBorder(moveInput, moveDistance))
            return moveInput;

        return Vector3.zero;
    }

    private Vector2 GetMoveInput()
    {
        float moveX = Input.GetAxisRaw(INPUT_HORIZONTAL);
        float moveY = Input.GetAxisRaw(INPUT_VERTICAL);

        return new Vector2(moveX, moveY);
    }

    private bool IsNextMoveCrossBorder(Vector2 moveDirectionNormalized, float moveDistance)
    {
        Vector2 nextPosition = (Vector2)transform.position + moveDirectionNormalized * moveDistance;
        return border.IsCrossOverBorder(nextPosition);
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
