using UnityEngine;
public class PlayerJumpingState : PlayerAbstractState
{
    public PlayerJumpingState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        isRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        _ctx.PlayerAnimator.SetBool(_ctx.IsJumpingHash, true);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();

        applyMainGravity();
    }
    public override void ExitState()
    {
        _ctx.PlayerAnimator.SetBool(_ctx.IsJumpingHash, false);
    }
    public override void CheckSwitchStates()
    {
        if (_ctx.isPlayerGrounded())
        {
            SwitchState(_factory.Grounded());
        }
    }
    public override string getName()
    {
        return "Jumping";
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

    void applyMainGravity()
    {
        bool isFalling = _ctx.PlayerMovementY < 0.0f;
        if (isFalling)
        {
            _ctx.PlayerMovementY -= _ctx.PLAYER_STATS.GRAVITY * Time.deltaTime * _ctx.PLAYER_STATS.FALL_MULTIPLIER;

        }
        else
        {
            _ctx.PlayerMovementY -= _ctx.PLAYER_STATS.GRAVITY * Time.deltaTime;
        }
    }
    void applyZeroGravity()
    {
        _ctx.PlayerMovementY = _ctx.PlayerTransformY;
    }
}