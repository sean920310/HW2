using UnityEngine;

// This is the base of the finite state machine
public abstract class MultiPlayerBaseState
{

    protected MultiPlayerStatesManager _context;
    protected MultiPlayerStateFactory _factory;

    public MultiPlayerBaseState(MultiPlayerStatesManager context, MultiPlayerStateFactory factory)
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
