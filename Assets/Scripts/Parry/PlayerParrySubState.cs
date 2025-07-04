using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerParrySubState : PlayerBaseState
{
    public PlayerParrySubState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }

    private float parryWindow = 0.5f;
    private float startTime;

    public override void EnterState()
    {
        startTime = Time.time;
        ctx.animator.SetBool("IsDefending", true);
        Debug.Log("Parry window opened");
    }

    public override void UpdateState()
    {
        if (Time.time - startTime > parryWindow) 
        {
            if (ctx.isBlockHeld)
            {
                superState.SetSubState(factory.BlockSub());
            }
            else
            {
                ctx.SwitchState(factory.Idle());
            }
        }
    }

    public override void ExitState()
    {
        Debug.Log("Parry Ended");
    }
}
