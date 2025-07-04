using System;
using UnityEngine;

public class PlayerDefenseState : PlayerBaseState
{
    public PlayerDefenseState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        SetIsRootState(true);
    }
    public override void EnterState()
    {
        if (ctx.isParryWindowOpen && ctx.isBlockJustPressed)
        {
            ctx.isParryWindowOpen = false;
            SetSubState(factory.ParrySub());
        }
        else //if (ctx.isBlockHeld || ctx.isBlockJustPressed)
        {
            SetSubState(factory.BlockSub());
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        currentSubState?.ExitState();
    }
}

