using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningGhoulAttackState : BurningGhoulBaseState
{
    public BurningGhoulAttackState(BurningGhoulStateManager context, BurningGhoulStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        if (_context.CanAttack)
            _context.startCorutine(AttackDelay());
        _context.Anim.SetFloat("Speed", 0);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        _context.LookAtPlayer(); 
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
        _context.attackEnd();
    }

    public override void CheckSwitchState()
    {
        if (!_context.AttackDetected && !_context.Attacking)
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
    IEnumerator AttackDelay()
    {
        _context.CanAttack = false;
        yield return new WaitForSeconds(_context.AttackDelay);
        _context.CanAttack = true;
    }

    IEnumerator AttackCD()
    {
        _context.CanAttack = false;
        yield return new WaitForSeconds(_context.AttackCDTime);
        _context.CanAttack = true;
    }
}
