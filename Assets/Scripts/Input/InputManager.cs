using UnityEngine;

public class InputManager 
{
    private const string INPUT_HORIZONTAL = "Horizontal";
    private const string INPUT_VERTICAL = "Vertical";

    public Vector2 GetMoveInput()
    {
        float moveX = Input.GetAxisRaw(INPUT_HORIZONTAL);
        float moveY = Input.GetAxisRaw(INPUT_VERTICAL);

        return new Vector2(moveX, moveY);
    }

    public Vector2 GetMoveInputNormalized()
    {
        return GetMoveInput().normalized;
    }
}
