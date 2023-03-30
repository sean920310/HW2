using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/States/Idle")]
public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateManager context, EnemyStateFactory factory)
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
        if (!_context.PlayerDetected)
        {
            _context.SwitchState(_factory.Patrol());
        }
        else if (_context.PlayerDetected)
        {
            _context.SwitchState(_factory.Chase());
        }
    }
}
