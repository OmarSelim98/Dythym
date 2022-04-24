
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerIdleState : PlayerAbstractState
{
    public PlayerIdleState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }

    public override void EnterState()
    {
        _ctx.PlayerAnimator.SetBool(_ctx.IsWalkingHash, false);
        _ctx.PlayerMovementX = 0;
        _ctx.PlayerMovementY = 0;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        //Debug.Log("In Idle hash " + _ctx.PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash);

    }
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {
        if (_ctx.IsMovementPressed)
        {
            SwitchState(_factory.Moving());
        }
        else if (_ctx.IsAttackingPressed && !_ctx.PlayerAnimator.IsInTransition(0))
        {
            SwitchState(_factory.Attacking());
        }
        else if (_ctx.IsDashPressed){
            SwitchState(_factory.Dashing());
        }
    }
    public override void InitializeSubState() { }
    public override string getName()
    {
        return "Idle";
    }
}