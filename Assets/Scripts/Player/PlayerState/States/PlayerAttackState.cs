using UnityEngine;
using System.Collections;

public class PlayerAttackState : PlayerBaseState
{
    Animation animate;
    float attackCounter;
    float attackTime;
    public PlayerAttackState(PlayerStatesManager context, PlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        _context.CanAttack = false;
        if (_context.CheckOnGround())
        {
            _context.PlayerRigidbody.isKinematic = true;
        }
        //_context.PlayerRigidbody.velocity = new Vector2(0f, 0f);
        _context.startCorutine(AttackTime());

        _context.Weapon.SetActive(true);
        _context.Weapon.GetComponent<Animator>().SetInteger("AttackCount", _context.AttackCount);
        _context.PlayerAnimator.SetInteger("AttackCount", _context.AttackCount);

        _context.Weapon.GetComponent<Animator>().SetBool("isAttack", true);
        _context.PlayerAnimator.SetBool("isAttack", true);

        _context.AttackCount++;
        _context.IsAttackPress = false;
        attackCounter = 0f;
        attackTime = _context.getAnimationLength("PlayerAttack" + _context.AttackCount.ToString());
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
        _context.AttackRange.SetActive(false);
        _context.Weapon.SetActive(false);
        _context.IsAttackPress = false;
    }

    public override void CheckSwitchState()
    {

        if (attackTime < attackCounter)
        {
            _context.Weapon.GetComponent<Animator>().SetBool("isAttack", false);
            _context.PlayerAnimator.SetBool("isAttack", false);
            if (_context.IsAttackPress && _context.AttackCount < 3)
            {
                _context.SwitchState(_factory.Attack());
            }
            else
            {
                _context.Weapon.GetComponent<Animator>().SetBool("isAttack", false);
                _context.PlayerAnimator.SetBool("isAttack", false);
                _context.AttackCount = 0;
                _context.startCorutine(AttackCoolDown());
                _context.SwitchState(_factory.Idle());
            }
        }
    }

    IEnumerator AttackTime()
    {
        if(_context.AttackCount == 1)
            yield return new WaitForSeconds(attackTime * 0.5f);
        else if (_context.AttackCount == 2)
            yield return new WaitForSeconds(attackTime * 0.2f);
        else
            yield return new WaitForSeconds(attackTime * 0.5f);

        _context.AttackRange.SetActive(true);
    }

    IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(_context.AttackCoolDownTime);

        _context.CanAttack = true;
    }
}
