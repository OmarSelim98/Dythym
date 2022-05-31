using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerAttackingStateOld : PlayerAbstractState
{
    public PlayerAttackingStateOld(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }
    bool isFinished = false;
    int prevHitCounter = 0;
    Coroutine endAttackAnimationCoroutine;
    public override void EnterState()
    {
        prevHitCounter = _ctx.HitCounter;
        isFinished = false;
        //_ctx.PlayerAudio.PlayOneShot(_ctx.AttacksAudio[_ctx.HitCounter % _ctx.AttacksAudio.Count].file);
        _ctx.PlayerAnimator.speed = _ctx.AudioStats.RelativeAnimationSpeed(1.0f);
        _ctx.PlayerAnimator.SetInteger(_ctx.H_hitCount, _ctx.HitCounter);
        _ctx.PlayerAnimator.SetTrigger(_ctx.H_attack);
        Debug.Log("Entering attack state at: " + _ctx.HitCounter);
        _ctx.AnimationSlash[_ctx.HitCounter].SetActive(true);

        endAttackAnimationCoroutine =  _ctx.StartCoroutine(endAttackAnimation()); // start
        _ctx.IsAttackingPressed = false;
        //_ctx.KatanaTrail.SetActive(true);
    }
    public override void UpdateState()
    {
        //Debug.Log("In Attack hash " + _ctx.PlayerAnimator.GetCurrentAnimatorStateInfo(0).ToString());
        //if (_ctx.IsAttackingPressed) {
        //    _ctx.KatanaCombo += 1;
        //}
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        _ctx.PlayerAnimator.speed = 1;
        _ctx.AnimationSlash[_ctx.HitCounter].SetActive(false);
        _ctx.HitCounter++;
        Debug.Log("Exit state at hit count = "+_ctx.HitCounter);
    }
    public override void CheckSwitchStates()
    {
        // if i attacked an another attack
        if (_ctx.IsAttackingPressed && prevHitCounter != _ctx.HitCounter)
        {
            if (endAttackAnimationCoroutine != null) _ctx.StopCoroutine(endAttackAnimationCoroutine);
            //Exit state and attack again
            SwitchState(_factory.Attacking());
        }
        if(isFinished)
        {
            SwitchState(_factory.Idle());
        }
    }

    public override void InitializeSubState() { }
    public override string getName()
    {
        return "Attacking";
    }

    
    IEnumerator endAttackAnimation()
    {
        yield return new WaitForSeconds(0.75f);
        isFinished = true;
    }
    
}
