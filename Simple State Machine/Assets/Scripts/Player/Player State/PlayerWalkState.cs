using UnityEngine;

public class PlayerWalkState : BaseState<PlayerStateMachine>
{
    public override void OnUpdate()
    {
        if (owner.PlayerVisual != null)
        {
            owner.PlayerVisual.PlayWalk(owner.CurrentDirection);
        }

        Vector2 moveInput = owner.PlayerInput.Move;
        if (moveInput.magnitude > 0.01f)
        {
            owner.MovementDirection = moveInput;
            owner.CurrentDirection = DirectionHelper.Vector2ToDirection(moveInput);
        }
    }

    public override void OnFixedUpdate()
    {
        owner.Rb.velocity = owner.MovementDirection.normalized * owner.MoveSpeed * 50f * Time.deltaTime;
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
        if (moveInput.magnitude <= 0.01f)
        {
            stateMachine.ChangeState<PlayerIdleState>();
            return;
        }
    }

    public override void OnExit()
    {
        owner.Rb.velocity = Vector2.zero;
    }
}
