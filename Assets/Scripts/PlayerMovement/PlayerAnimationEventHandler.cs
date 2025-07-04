using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    public PlayerStateMachine _playerStateMachine;
    public void Jump()
    {
        _playerStateMachine.ApplyJumpVelocity();
    }

    public void ApplyAttackDamageEvent()
    {
        _playerStateMachine.ApplyAttackDamage();
    }
    public void EndAttackAnimationEvent()
    {
        _playerStateMachine.EndAttack();
    }
}
