using UnityEngine;

// This is the base of the finite state machine
public abstract class BurningGhoulBaseState
{

    protected BurningGhoulStateManager _context;
    protected BurningGhoulStateFactory _factory;

    public BurningGhoulBaseState(BurningGhoulStateManager context, BurningGhoulStateFactory factory)
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