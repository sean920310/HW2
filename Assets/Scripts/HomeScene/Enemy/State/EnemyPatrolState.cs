using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPatrolState : EnemyBaseState
{
    private bool turningWait ;
    public EnemyPatrolState(EnemyStateManager context, EnemyStateFactory factory)
     : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        turningWait = false;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        if ((_context.WallDetected || _context.GroundDetected) && !turningWait)
        {
            _context.startCorutine(TurnWait());
            turningWait = true;
            _context.TurnAtEdge();
        }
    }

    public override void FixedUpdateState()
    {
        if(!turningWait)
        {
            Vector2 newPos = Vector2.MoveTowards(_context.Rigidbody2D.position, _context.Rigidbody2D.position - (Vector2)_context.Rigidbody2D.transform.right, _context.MovingSpeed * Time.fixedDeltaTime);
            _context.Rigidbody2D.MovePosition(newPos);
        }
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {
        if (_context.AttackDetected)
        {
            _context.SwitchState(_factory.Attack());
        }
        else if (_context.PlayerDetected)
        {
            _context.SwitchState(_factory.Chase());
        }
    }

    IEnumerator TurnWait()
    {
        yield return new WaitForSeconds(0.1f);
        turningWait = false;
    }
}
