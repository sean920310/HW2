using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/States/Chase")]
public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateManager context, EnemyStateFactory factory)
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
        _context.LookAtPlayer();
    }

    public override void FixedUpdateState()
    {
        if(!(_context.WallDetected || _context.GroundDetected))
        {
            Vector2 target = new Vector2(_context.Target.x, _context.Rigidbody2D.position.y);
            Vector2 newPos = Vector2.MoveTowards(_context.Rigidbody2D.position, target, _context.MovingSpeed * Time.fixedDeltaTime);
            _context.Rigidbody2D.MovePosition(newPos);
        }
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
        else if(_context.AttackDetected)
        {
            _context.SwitchState(_factory.Attack());
        }
    }
}
