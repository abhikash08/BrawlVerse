using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }

    public override void EnterState() => ctx.animator.SetFloat("Speed", ctx.Speed);

    public override void UpdateState()
    {
        if (ctx.moveInput.magnitude <= 0.1f)
        {
            ctx.SwitchState(factory.Idle());
            return;
        }

        //Vector3 moveDir = ctx.GetMoveDirection().normalized;
        //ctx.RotateTowardsMovementDirection(moveDir);
        //ctx.characterController.Move(moveDir * ctx.Speed * Time.deltaTime);
    }

    public override void ExitState() { }

    public override void HandleJumpInput()
    {
        if (ctx.isGrounded)
            ctx.SwitchState(factory.Jump());
    }
}
