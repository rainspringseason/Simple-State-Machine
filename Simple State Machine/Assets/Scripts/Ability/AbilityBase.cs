using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    [Header("Ability References")]
    [SerializeField] protected AbilityDataSO abilityData;

    protected PlayerStateMachine playerStateMachine;
    protected PlayerInput playerInput;
    protected PlayerVisual playerVisual;

    protected float lastUseTime = -999f;
    protected bool isExecuting = false;

    protected virtual void Awake()
    {
        if (playerStateMachine == null)
            playerStateMachine = GetComponentInParent<PlayerStateMachine>();

        if (playerInput == null)
            playerInput = GetComponentInParent<PlayerInput>();

        if (playerVisual == null)
            playerVisual = GetComponentInParent<PlayerVisual>();
    }

    public virtual bool CanExecute()
    {
        if (isExecuting || abilityData == null)
            return false;

        if (Time.time - lastUseTime < abilityData.cooldown)
            return false;

        return true;
    }

    public float GetCooldownRemaining()
    {
        float remaining = abilityData.cooldown - (Time.time - lastUseTime);
        return Mathf.Max(0, remaining);
    }

    public bool IsOnCooldown()
    {
        return GetCooldownRemaining() > 0;
    }

    public void Execute()
    {
        if (!CanExecute())
            return;

        lastUseTime = Time.time;
        StartCoroutine(ExecuteAbilityRoutine());
    }

    protected virtual IEnumerator ExecuteAbilityRoutine()
    {
        isExecuting = true;

        Direction abilityDirection = playerStateMachine.CurrentDirection;

        yield return StartCoroutine(PlayAbilityAnimation(abilityDirection));
        yield return StartCoroutine(ExecuteAbility(abilityDirection));
        yield return StartCoroutine(WaitForAnimationEnd(abilityDirection));

        isExecuting = false;
    }

    protected virtual IEnumerator PlayAbilityAnimation(Direction direction)
    {
        if (playerVisual != null)
        {
            playerVisual.PlayFirstAbility(direction);
        }

        yield return new WaitForSeconds(abilityData.castTime);
    }

    protected abstract IEnumerator ExecuteAbility(Direction direction);

    protected virtual IEnumerator WaitForAnimationEnd(Direction direction)
    {
        if (playerVisual != null)
        {
            string animName = playerVisual.GetAnimationName("AbilityFirst", direction);
            float animLength = playerVisual.GetAnimationLength(animName);
            float waitTime = Mathf.Max(0, animLength - abilityData.castTime);
            yield return new WaitForSeconds(waitTime);
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
        }
    }

    public bool IsExecuting() => isExecuting;
}
