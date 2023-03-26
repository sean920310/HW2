using UnityEngine;

// This is the base of the finite state machine
public abstract class PlayerBaseState
{

    protected PlayerStatesManager _context;
    protected PlayerStateFactory _factory;

    public PlayerBaseState(PlayerStatesManager context, PlayerStateFactory factory)
    {
        _context = context;
        _factory = factory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();

}
