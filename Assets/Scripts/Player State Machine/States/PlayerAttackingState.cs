using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerAttackingState : PlayerAbstractState
{
    // when the player tap the attack button, he enters the attack state for the first time
    // hit counter will be = 0
    // the state will exit to idle if
    // the animation of the attack has finished
    // it will repeat if
    // the player clicks the attack button again
    public PlayerAttackingState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }

    Coroutine _endAttackAnimationCoroutine;
    public override void EnterState()
    {
        // clamp the hit counter
        _ctx.HitCounter = Mathf.Clamp(_ctx.HitCounter, 0, 2);
        Debug.Log("Hits = " + _ctx.HitCounter);
        _ctx.PlayerAnimator.SetTrigger(_ctx.H_attack); // trigger the attack
        _ctx.PlayerAnimator.SetInteger(_ctx.H_hitCount, _ctx.HitCounter); // add the hit 
        //start the animtion and set an end to it
        _ctx.IsAttackingPressed = false;
        _endAttackAnimationCoroutine = _ctx.StartCoroutine(EndAttackAnimation());
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        _ctx.HitCounter++;
        if(_ctx.HitCounter > 2)
        {
            _ctx.HitCounter = 0;
        }
    }
    public override void CheckSwitchStates()
    {
        /**
            if the animation has finished exit the state to :
                idle : if player hasn't clicked the attack button (again) during the attack, and the animation has finished.
                attack : if the player clicks the attack button again during an attack.
         */
    }

    public override void InitializeSubState() { }
    public override string getName()
    {
        return "Attacking";
    }
    IEnumerator EndAttackAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        if (_ctx.IsAttackingPressed)
        {
            _ctx.PlayerAnimator.SetBool("canCombo", true);
            Debug.Log("Attacking Again");
            SwitchState(_factory.Attacking());
        }
        else
        {
            _ctx.PlayerAnimator.SetBool("canCombo", false);
            Debug.Log("Exiting combo bye");
            SwitchState(_factory.Idle());
        }
    }
}
