using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningGhoulHurtState : BurningGhoulBaseState
{
    public BurningGhoulHurtState(BurningGhoulStateManager context, BurningGhoulStateFactory factory)
           : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {

    }
}
