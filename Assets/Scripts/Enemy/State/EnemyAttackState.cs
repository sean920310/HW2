using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/States/Attack")]
public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateManager context, EnemyStateFactory factory)
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
        if (_context.CanAttack)
        {
            _context.Anim.SetTrigger("Attack");
            _context.startCorutine(AttackCD());
        }
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {
        if(!_context.AttackDetected)
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
    

    IEnumerator AttackCD()
    {
        _context.CanAttack = false;
        yield return new WaitForSeconds(_context.AttackCDTime);
        _context.CanAttack = true;
    }
}
