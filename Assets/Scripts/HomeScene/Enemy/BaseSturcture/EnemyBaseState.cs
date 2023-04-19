using UnityEngine;

// This is the base of the finite state machine
public abstract class EnemyBaseState
{

    protected EnemyStateManager _context;
    protected EnemyStateFactory _factory;
    
    public EnemyBaseState(EnemyStateManager context, EnemyStateFactory factory)
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