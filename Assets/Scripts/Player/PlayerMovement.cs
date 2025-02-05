using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private Transform graphicTransform;
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private float baseModeSpeed = 2;
    private Player _player;
    private BorderPoints _border;
    private SignalBus _signalBus;
    private InputManager _inputManager;

    [Inject]
    public void Construct(Player player, BorderPoints border, SignalBus signalBus, InputManager inputManager)
    {
        _player = player;
        _border = border;
        _signalBus = signalBus;
        _inputManager = inputManager;
    }



    private float currentMoveSpeed;
    private bool isMoveable = true;

    private void Awake()
    {
        UpdateCurrentSpeed();
        _signalBus.Subscribe<PlayerDie>(OnDie);
    }

    private void OnDie()
    {
        isMoveable = false;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    public void UpdateCurrentSpeed()
    {
        currentMoveSpeed = baseModeSpeed * (1 + (_player.CurrentStats.Speed/100));
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!isMoveable) return;
        float moveDistance = currentMoveSpeed * Time.fixedDeltaTime;
        Vector2 moveVector = GetMoveVector(moveDistance);

        if (moveVector != Vector2.zero) HandleFlipGraphic(moveVector.x);

        rigidbody2D.velocity = moveVector * currentMoveSpeed;
        playerAnimation.SetIsRunning(moveVector != Vector2.zero, currentMoveSpeed);
    }

    private Vector2 GetMoveVector(float moveDistance)
    {
        Vector2 moveInput = _inputManager.GetMoveInput();
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

    private bool IsNextMoveCrossBorder(Vector2 moveDirectionNormalized, float moveDistance)
    {
        Vector2 nextPosition = (Vector2)transform.position + moveDirectionNormalized * moveDistance;
        return _border.IsCrossOverBorder(nextPosition);
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
 