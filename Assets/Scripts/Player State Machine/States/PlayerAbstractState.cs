public abstract class PlayerAbstractState
{
    protected bool isRootState = false;

    protected PlayerStateMachine _ctx;
    protected PlayerStateFactory _factory;
    public  PlayerAbstractState _subState;
    protected PlayerAbstractState _superState;
    public PlayerAbstractState(PlayerStateMachine currentContext, PlayerStateFactory factory)
    {
        _ctx = currentContext;
        _factory = factory;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();
    public abstract string getName();

    public void UpdateStates()
    {
        UpdateState();
        if (_subState != null)
        {
            _subState.UpdateStates();
        }
    }
    public void ExitStates()
    {
        ExitState();
        if (_subState != null)
        {
            _subState.ExitStates();
        }
    }
    protected void SwitchState(PlayerAbstractState newState)
    {
        ExitState();
        newState.EnterState();
        if (isRootState)
        {
            _ctx.CurrentState = newState;

        }
        else
        {
            if (_superState != null)
            {
                //if the new state is a sub state, set it as a child to the super state.
                _superState.SetSubState(newState);
            }
        }
    }
    protected void SetSuperState(PlayerAbstractState newSuperState)
    {
        _superState = newSuperState;
    }
    protected void SetSubState(PlayerAbstractState newSubState)
    {
        _subState = newSubState;
        _subState.SetSuperState(this);
    }
}