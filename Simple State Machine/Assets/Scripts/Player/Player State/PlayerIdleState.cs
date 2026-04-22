using UnityEngine;

public class PlayerIdleState : BaseState<PlayerStateMachine>
{
    public override void OnEnter()
    {
        owner.Rb.velocity = Vector2.zero;
    }

    public override void OnUpdate()
    {
        if (owner.PlayerVisual != null)
        {
            owner.PlayerVisual.PlayIdle(owner.CurrentDirection);
        }
    }

    public override void CheckTransitions()
    {
        if (owner.PlayerInput.NormalAttack)
        {
            stateMachine.ChangeState<PlayerAttackState>();
            return;
        }

        if (owner.PlayerInput.FirstAbility)
        {
            if (owner.ProjectileAbility != null && owner.ProjectileAbility.CanExecute())
            {
                stateMachine.ChangeState<PlayerAbilityState>();
                return;
            }
        }

        Vector2 moveInput = owner.PlayerInput.Move;
        if (moveInput.magnitude > 0.01f)
        {
            owner.MovementDirection = moveInput;
            owner.CurrentDirection = DirectionHelper.Vector2ToDirection(moveInput);
            stateMachine.ChangeState<PlayerWalkState>();
            return;
        }
    }
}
