using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerAnimation : MonoBehaviour
{
    private enum AnimatorParammeters
    {
        Die,
        IsRunning,
        RunningSpeedMultiplier
    }

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float getHitDuration = 0.2f;
    [Inject] private SignalBus signalBus;

    private const float BASE_RUNNING_ANIMATION_SPEED = 2.5f;
    private Coroutine hitCoroutine;

    private void Awake()
    {
        signalBus.Subscribe<PlayerGetHit>(OnGetHit);
        signalBus.Subscribe<PlayerDie>(OnDie);
    }

    public void OnGetHit()
    {
        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
        }
        hitCoroutine = StartCoroutine(HitEffectCoroutine());
    }

    private IEnumerator HitEffectCoroutine()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(getHitDuration);
        spriteRenderer.color = Color.white;
        hitCoroutine = null; 
    }

    public void OnDie()
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
