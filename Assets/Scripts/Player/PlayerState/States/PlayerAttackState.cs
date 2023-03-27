using UnityEngine;
using System.Collections;

public class PlayerAttackState : PlayerBaseState
{
    Animation animate;
    float attackCounter;
    public PlayerAttackState(PlayerStatesManager context, PlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        _context.PlayerAnimator.SetTrigger("onAttack");

        if (_context.CheckOnGround())
        {
            _context.PlayerRigidbody.isKinematic = true;
        }
        _context.PlayerRigidbody.velocity = new Vector2(0f, 0f);
        _context.startCorutine(AttackTime());
    }

    public override void UpdateState()
    {
        if (_context.CheckOnGround())
        {
            _context.PlayerRigidbody.velocity = new Vector2(0f, 0f);
        }

        attackCounter += Time.deltaTime;
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {
        _context.PlayerRigidbody.isKinematic = false;
        _context.IsAttackPress = false;
        _context.AttackRange.SetActive(false);
    }

    public override void CheckSwitchState()
    {
        if(attackCounter > _context.AttackAnimation.length)
            _context.SwitchState(_factory.Idle());
    }

    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(0.3f);
        _context.AttackRange.SetActive(true);
    }

}