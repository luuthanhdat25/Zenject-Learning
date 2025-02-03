using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private enum AnimatorParammeters
    {
        Die,
        IsRunning
    }

    [SerializeField] private Animator animator;

    public void Die()
    {
        animator.SetTrigger(AnimatorParammeters.Die.ToString());
    }

    public void SetIsRunning(bool value)
    {
        animator.SetBool(AnimatorParammeters.IsRunning.ToString(), value);
    }
}
