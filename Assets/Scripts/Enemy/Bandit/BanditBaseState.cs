using UnityEngine;

// This is the base of the finite state machine
public abstract class BanditBaseState
{

    protected BanditStateManager _context;
    protected BanditStateFactory _factory;

    public BanditBaseState(BanditStateManager context, BanditStateFactory factory)
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