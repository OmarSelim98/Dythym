using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
public class PlayerDashingState : PlayerAbstractState
{
    public PlayerDashingState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }
    public override void EnterState()
    {
        _ctx.PlayerInput.Controller.Disable();
        _ctx.PlayerAnimator.SetBool(_ctx.IsDashingHash, true);
        Debug.Log("Starting Dash");
        _ctx.StartTimedFunction(_ctx.PLAYER_STATS.DASH_TIME);
        _ctx.DisableCharacterController();
        _ctx.PlayerTransform.DOMove(new Vector3(_ctx.PlayerTransform.position.x + (_ctx.CachedMovementVector.x * _ctx.PLAYER_STATS.DASH_DISTANCE), _ctx.PlayerTransformY, _ctx.PlayerTransform.position.z + (_ctx.CachedMovementVector.y * _ctx.PLAYER_STATS.DASH_DISTANCE)), _ctx.PLAYER_STATS.DASH_TIME);
        DOTween.To(x => _ctx.DashVolume.GetComponent<Volume>().weight = x, 0, 1, _ctx.PLAYER_STATS.DASH_TIME - 0.1f);
        //vol.weight.DOValue(1, _ctx.PLAYER_STATS.DASH_TIME);
    }

    public override void UpdateState()
    {
        if (_ctx.TimedFunctionFinished)
        {
            //if (_ctx.AudioStats.onQuarterBeat())
            //{
                
            //}
            Debug.Log(_ctx.TimedFunctionFinished);
            _ctx.IsDashPressed = false;
        }
        CheckSwitchStates();
    }



    public override void ExitState()
    {
        DOTween.To(x => _ctx.DashVolume.GetComponent<Volume>().weight = x, 1, 0, 0.1f);
        _ctx.EnableCharacterController();
        _ctx.PlayerInput.Controller.Enable();
        _ctx.PlayerAnimator.SetBool(_ctx.IsDashingHash, false);
        Debug.Log("Dash Ended");
    }

    public override void CheckSwitchStates()
    {
        if (!_ctx.IsDashPressed)
        {
            // if (_ctx.MainState.getName == "Grounded")
            // {
            SwitchState(_factory.Idle());
            //}
        }
    }

    public override string getName()
    {
        return "Dashing";
    }
    public override void InitializeSubState() { }


}
