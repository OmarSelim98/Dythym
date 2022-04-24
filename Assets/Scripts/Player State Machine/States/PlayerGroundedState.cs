using UnityEngine;
public class PlayerGroundedState : PlayerAbstractState
{
    public PlayerGroundedState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        isRootState = true;
        InitializeSubState();
    }
    public override void EnterState()
    {
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        applyGroundedGravity();
        resetJump();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {
        if (!_ctx.isPlayerGrounded())
        {
            SwitchState(_factory.Jumping());
        }
        checkAndApplyJump();
    }
    public override string getName()
    {
        return "Grounded";
    }
    public override void InitializeSubState()
    {
        if (!_ctx.IsMovementPressed)
        {
            SetSubState(_factory.Idle());
        }
        else
        {
            SetSubState(_factory.Moving());
        }
    }
    void applyGroundedGravity()
    {
        _ctx.PlayerMovementY -= _ctx.PLAYER_STATS.GROUNDED_GRAVITY * Time.deltaTime;
    }
    void resetJump()
    {
        if (!_ctx.CanJump && !_ctx.IsJumpPressed)
        {
            _ctx.CanJump = true;
        }
    }
    void checkAndApplyJump()
    {
        if (_ctx.IsJumpPressed && _ctx.CanJump)
        {
            SwitchState(_factory.Jumping());
            _ctx.PlayerMovementY = _ctx.PLAYER_STATS.INITIAL_JUMP_SPEED;
            _ctx.CanJump = false;
        }
    }
}