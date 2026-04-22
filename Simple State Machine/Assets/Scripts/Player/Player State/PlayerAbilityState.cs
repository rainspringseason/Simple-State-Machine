using UnityEngine;

public class PlayerAbilityState : BaseState<PlayerStateMachine>
{
    private bool abilityExecuted = false;

    public override void OnEnter()
    {
        owner.Rb.velocity = Vector2.zero;
        abilityExecuted = false;

        if (owner.ProjectileAbility != null && owner.ProjectileAbility.CanExecute())
        {
            owner.ProjectileAbility.Execute();
            abilityExecuted = true;
        }
    }

    public override void CheckTransitions()
    {
        if (abilityExecuted && owner.ProjectileAbility != null)
        {
            if (!owner.ProjectileAbility.IsExecuting())
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
        else if (!abilityExecuted)
        {
            stateMachine.ChangeState<PlayerIdleState>();
        }
    }

    public override void OnExit()
    {
        abilityExecuted = false;
    }
}
