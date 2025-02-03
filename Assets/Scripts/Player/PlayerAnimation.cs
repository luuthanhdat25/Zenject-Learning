using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private enum AnimatorParammeters
    {
        Die,
        IsRunning,
        RunningSpeedMultiplier
    }

    [SerializeField] private Animator animator;

    private const float BASE_RUNNING_ANIMATION_SPEED = 2.5f;

    public void Die()
    {
        animator.SetTrigger(AnimatorParammeters.Die.ToString());
    }

    public void SetIsRunning(bool value, float moveSpeed)
    {
        animator.SetBool(AnimatorParammeters.IsRunning.ToString(), value);
        if (value)
        {
            animator.SetFloat(AnimatorParammeters.RunningSpeedMultiplier.ToString(), moveSpeed / BASE_RUNNING_ANIMATION_SPEED);
        }
    }
}
