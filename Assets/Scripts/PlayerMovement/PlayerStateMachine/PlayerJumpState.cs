using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }

    public override void EnterState()
    {
        ctx.animator.SetTrigger("Jump");
    }
    
    public override void UpdateState()
    {
        Vector3 direction = new Vector3(ctx.moveInput.x, 0f, ctx.moveInput.y).normalized;
        Vector3 moveDir = ctx.GetMoveDirection(direction);
        ctx.characterController.Move(moveDir * ctx.Speed * Time.deltaTime);

        if (ctx.isGrounded && ctx.velocity.y <= 0f)
        {
            if (ctx.moveInput.magnitude > 0.1f)
                ctx.SwitchState(factory.Run());
            else
                ctx.SwitchState(factory.Idle());
        }
    }

    public override void ExitState() { }
}
