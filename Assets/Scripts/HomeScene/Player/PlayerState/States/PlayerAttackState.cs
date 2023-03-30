using UnityEngine;
using System.Collections;

public class PlayerAttackState : PlayerBaseState
{
    bool normalExit = false;

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
            _context.PlayerRigidbody.isKinematic = false;
        }

        // Set Weapon and Player animation
        _context.Weapon.SetActive(true);
        _context.Weapon.GetComponent<Animator>().SetInteger("AttackCount", _context.AttackCount);
        _context.PlayerAnimator.SetInteger("AttackCount", _context.AttackCount);

        _context.Weapon.GetComponent<Animator>().SetBool("isAttack", true);
        _context.PlayerAnimator.SetBool("isAttack", true);

        _context.AttackCount++;
        _context.IsAttackPress = false;
        attackTime = _context.getAnimationLength("PlayerAttack" + _context.AttackCount.ToString()); // set attack duration by getAnimationLength()
        attackCounter = 0f;
        //_context.applyAttackForce();
        _context.PlayerBoxCollider.sharedMaterial = _context.AttackingPhysics;
    }

    public override void UpdateState()
    {
        if (_context.CheckOnGround())
        {
            _context.PlayerRigidbody.velocity = new Vector2(0f, 0f);
        }

        if (_context.LastMove != Vector2.zero && _context.LastMove.x < 0f)
            _context.FacingLeft();
        else if (_context.LastMove != Vector2.zero && _context.LastMove.x > 0f)
            _context.FacingRight();

        attackCounter += Time.deltaTime;
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {
        _context.PlayerRigidbody.isKinematic = false;
        _context.Weapon.SetActive(false);
        _context.IsAttackPress = false;
        _context.PlayerBoxCollider.sharedMaterial = _context.NormalPhysics;

        _context.PlayerAnimator.SetBool("isAttack", false);
        _context.Weapon.GetComponent<Animator>().SetBool("isAttack", false);



        if (normalExit == false) // unusual exit like Hurt() state will need this to reset state. 
        {
            _context.AttackCount = 0;
            _context.startCorutine(AttackCoolDown());
        }
    }

    public override void CheckSwitchState()
    {

        if (attackTime < attackCounter)
        {
            if (_context.IsAttackPress && _context.AttackCount < 3)
            {
                normalExit = true;
                _context.SwitchState(_factory.Attack());
            }
            else if (_context.IsBlockingPress)
            {
                normalExit = true;
                if (_context.AttackCount == 1 || _context.AttackCount == 3) // Critical Hit Reservation by not reset AttackCount
                    _context.AttackCount = 0;
                else
                    _context.startCorutine(CritReserve());
                _context.startCorutine(AttackCoolDown());
                _context.SwitchState(_factory.Blocking());
            }
            else
            {
                normalExit = true;
                if (_context.AttackCount == 1 || _context.AttackCount == 3) // Critical Hit Reservation by not reset AttackCount
                    _context.AttackCount = 0;
                else
                    _context.startCorutine(CritReserve());
                _context.startCorutine(AttackCoolDown());
                _context.SwitchState(_factory.Idle());
            }
        }
    }

    IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(_context.AttackCoolDownTime);

        _context.CanAttack = true;
    }
    IEnumerator CritReserve() // reset AttackCount 
    {
        yield return new WaitForSeconds(_context.CritReserveTime);

        if(_context.AttackCount == 2)
            _context.AttackCount = 0;
    }
}
