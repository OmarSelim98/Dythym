using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerAttackingState : PlayerAbstractState
{
    public PlayerAttackingState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }
    bool isFinished = false;
    public override void EnterState()
    {
        isFinished = false;
        _ctx.PlayerInput.Controller.Disable();
        //_ctx.PlayerAudio.PlayOneShot(_ctx.AttacksAudio[_ctx.HitCounter % _ctx.AttacksAudio.Count].file);
        if (_ctx.HitCounter > 2)
        {
            _ctx.HitCounter = 0;
        }
        _ctx.PlayerAnimator.speed = _ctx.AudioStats.RelativeAnimationSpeed(1.0f);
        _ctx.PlayerAnimator.SetInteger(_ctx.H_hitCount, _ctx.HitCounter);
        _ctx.PlayerAnimator.SetTrigger(_ctx.H_attack);
        DOTween.To(x => _ctx.KatanaDissolve[0].SetFloat("_dissolve", x), 1, 0, 0.4f);
        DOTween.To(x => _ctx.KatanaDissolve[1].SetFloat("_dissolve", x), 1, 0, 0.4f);
        DOTween.To(x => _ctx.KatanaDissolve[2].SetFloat("_dissolve", x), 1, 0, 0.4f);
        DOTween.To(x => _ctx.KatanaDissolve[3].SetFloat("_dissolve", x), 1, 0, 0.4f);
        _ctx.AnimationSlash[_ctx.HitCounter].SetActive(true);
        _ctx.StartTimedFunction(0.5f);
        //_ctx.KatanaTrail.SetActive(true);
    }
    public override void UpdateState()
    {
        Debug.Log("In Attack hash " + _ctx.PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash);

        if (_ctx.TimedFunctionFinished)
        {
            isFinished = true;
            _ctx.TimedFunctionFinished = false;
            //Debug.Log(_ctx.KatanaDissolve[0].GetFloat("_dissolve"));
        }
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        _ctx.PlayerAnimator.speed = 1;
        _ctx.IsAttackingPressed = false;
        _ctx.AnimationSlash[_ctx.HitCounter].SetActive(false);
        _ctx.HitCounter++;
        
        _ctx.PlayerInput.Controller.Enable();

    }
    public override void CheckSwitchStates()
    {
        //if ((_ctx.H_attackAnimationList.Contains(_ctx.PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash)
        //|| _ctx.H_attackAnimationList.Contains(_ctx.PlayerAnimator.GetNextAnimatorStateInfo(0).shortNameHash))
        //|| _ctx.PlayerAnimator.IsInTransition(0))
        //{

        //}
        if(isFinished)
        {
            DOTween.To(x => _ctx.KatanaDissolve[0].SetFloat("_dissolve", x), 0, 1, 0.5f);
            DOTween.To(x => _ctx.KatanaDissolve[1].SetFloat("_dissolve", x), 0, 1, 0.5f);
            DOTween.To(x => _ctx.KatanaDissolve[2].SetFloat("_dissolve", x), 0, 1, 0.5f);
            DOTween.To(x => _ctx.KatanaDissolve[3].SetFloat("_dissolve", x), 0, 1, 0.5f);
            SwitchState(_factory.Idle());
            //_ctx.PlayerInput.Controller.Enable();
            // if (_ctx.IsMovementPressed)
            // {
            //     SwitchState(_factory.Moving());
            // }
            // else
            // {
            //     SwitchState(_factory.Idle());
            // }
        }
    }

    public override void InitializeSubState() { }
    public override string getName()
    {
        return "Attacking";
    }
}
