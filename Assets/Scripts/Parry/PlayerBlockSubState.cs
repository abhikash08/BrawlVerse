using UnityEngine;

public class PlayerBlockSubState : PlayerBaseState
{
    public PlayerBlockSubState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }

    public override void EnterState()
    {
        ctx.animator.SetBool("IsDefending", true);
        Debug.Log("Blocking.....");
    }
    public override void UpdateState()
    {
        if (!ctx.isBlockHeld) 
        {
            ctx.animator.SetBool("IsDefending", false);
            ctx.SwitchState(factory.Idle());

        }
    }

    public override void ExitState()
    {
        ctx.animator.SetBool("IsDefending", false);
    }
}
