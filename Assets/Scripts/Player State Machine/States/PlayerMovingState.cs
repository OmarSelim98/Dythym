
using UnityEngine;
public class PlayerMovingState : PlayerAbstractState
{
    public PlayerMovingState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }

    public override void EnterState()
    {
        _ctx.PlayerAnimator.SetBool(_ctx.IsWalkingHash, true);
        float initialSpeed = _ctx.AudioStats.AbsoluteAnimationSpeedByFrame(0.7f);
        _ctx.PlayerAnimator.speed = _ctx.AudioStats.RelativeAnimationSpeed(initialSpeed);

    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        _ctx.PlayerMovementX = _ctx.MovementVectorX * _ctx.PLAYER_STATS.MOVEMENT_SPEED;
        _ctx.PlayerMovementZ = _ctx.MovementVectorY * _ctx.PLAYER_STATS.MOVEMENT_SPEED;
        calculateRotation();
    }
    public override void ExitState()
    {
        _ctx.PlayerMovementX = 0;
        _ctx.PlayerMovementZ = 0;
        _ctx.PlayerAnimator.speed = 1.0f;
    }
    public override void CheckSwitchStates()
    {
        if (!_ctx.IsBeingDamaged)
        {
            if (_ctx.IsAttackingPressed && !_ctx.PlayerAnimator.IsInTransition(0))
            {
                SwitchState(_factory.Attacking());
            }
            else if (_ctx.IsDashPressed)
            {
                SwitchState(_factory.Dashing());

            }
            else if (!_ctx.IsMovementPressed)
            {
                SwitchState(_factory.Idle());
            }
        }
        else
        {
            SwitchState(_factory.Damaged());
        }
    }
    public override void InitializeSubState() { }
    public override string getName()
    {
        return "Moving";
    }
    void calculateRotation()
    {
        //calculate the new point of rotation
        if (_ctx.PlayerMovementX != 0 || _ctx.PlayerMovementZ != 0)
        {
            Vector3 positionToLookAt;
            positionToLookAt.x = _ctx.PlayerMovementX;
            positionToLookAt.y = 0;
            positionToLookAt.z = _ctx.PlayerMovementZ;
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

            //get the last point of rotation
            Quaternion currentRotation = _ctx.PlayerRotation;

            //rotate from the last rotation to the new rotation smoothly by a factor
            _ctx.PlayerRotation = Quaternion.Slerp(currentRotation, targetRotation, _ctx.PLAYER_STATS.ROTATION_FACTOR);
        }
    }
}