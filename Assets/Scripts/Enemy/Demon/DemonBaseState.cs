using UnityEngine;

// This is the base of the finite state machine
public abstract class DemonBaseState
{

    protected DemonStateManager _context;
    protected DemonStateFactory _factory;

    public DemonBaseState(DemonStateManager context, DemonStateFactory factory)
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