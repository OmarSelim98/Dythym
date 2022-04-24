public class PlayerStateFactory
{
    PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerAbstractState Idle()
    {
        return new PlayerIdleState(_context, this);
    }
    public PlayerAbstractState Moving()
    {
        return new PlayerMovingState(_context, this);
    }
    public PlayerAbstractState Attacking()
    {
        return new PlayerAttackingState(_context, this);
    }
    public PlayerAbstractState Jumping()
    {
        return new PlayerJumpingState(_context, this);
    }
    public PlayerAbstractState Dashing ()
    {
        return new PlayerDashingState(_context, this);
    }
    public PlayerAbstractState Grounded()
    {
        return new PlayerGroundedState(_context, this);
    }
}