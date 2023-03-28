using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DemonBaseState
{
    protected PlayerStatesManager _context;
    protected PlayerStateFactory _factory;

    public DemonBaseState(PlayerStatesManager context, PlayerStateFactory factory)
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
