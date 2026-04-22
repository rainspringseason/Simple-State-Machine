using UnityEngine;
using System.Collections;

public class PlayerAttackState : BaseState<PlayerStateMachine>
{
    private Direction attackDirection;
    private bool isAttackComplete = false;

    public override void OnEnter()
    {
        owner.Rb.velocity = Vector2.zero;
        isAttackComplete = false;

        attackDirection = owner.GetDirectionToMouse();
        owner.CurrentDirection = attackDirection;

        owner.StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        if (owner.PlayerVisual != null)
        {
            owner.PlayerVisual.PlayAttack(attackDirection);
        }

        yield return new WaitForSeconds(0.2f);

        if (owner.PlayerVisual != null)
        {
            string attackAnimName = owner.PlayerVisual.GetAnimationName("Attack", attackDirection);
            float animLength = owner.PlayerVisual.GetAnimationLength(attackAnimName);
            yield return new WaitForSeconds(animLength - 0.2f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }

        isAttackComplete = true;
    }

    public override void CheckTransitions()
    {
        if (isAttackComplete)
        {
            Vector2 moveInput = owner.PlayerInput.Move;
            if (moveInput.magnitude > 0.01f)
            {
                owner.MovementDirection = moveInput;
                owner.CurrentDirection = DirectionHelper.Vector2ToDirection(moveInput);
                stateMachine.ChangeState<PlayerWalkState>();
            }
            else
            {
                stateMachine.ChangeState<PlayerIdleState>();
            }
        }
    }

    public override void OnExit()
    {
        isAttackComplete = false;
    }
}
