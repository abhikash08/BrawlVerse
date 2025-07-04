using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }

    public override void EnterState()
    {
        ctx.animator.SetFloat("Speed", ctx.Speed * 2f);
    }

    public override void UpdateState()
    {
        if (ctx.isDashing)
            return; // Skip movement

        Vector3 direction = new Vector3(ctx.moveInput.x, 0f, ctx.moveInput.y).normalized;
        Vector3 moveDir = ctx.GetMoveDirection(direction);
        //ctx.RotateTowardsMovementDirection(moveDir);
        ctx.characterController.Move(moveDir * ctx.Speed * Time.deltaTime);

        if (ctx.moveInput.magnitude <= 0.1f)
            ctx.SwitchState(factory.Idle());
    }

    public override void ExitState() { }

    public override void HandleJumpInput()
    {
        if (ctx.isGrounded)
            ctx.SwitchState(factory.Jump());
    }
}
