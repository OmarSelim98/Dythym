using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
public class PlayerDamagedState : PlayerAbstractState
{
    public PlayerDamagedState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }
    public override void EnterState()
    {
        _ctx.PlayerInput.Controller.Disable();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }



    public override void ExitState()
    {
        _ctx.PlayerInput.Controller.Enable();
    }

    public override void CheckSwitchStates()
    {
        if (!_ctx.IsBeingDamaged)
        {
            SwitchState(_factory.Idle());
        }
    }

    public override string getName()
    {
        return "Damaged";
    }
    public override void InitializeSubState() { }


}
