using UnityEngine;
using System.Collections;
using Inventory.Model;

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
        ItemSO item = _context.backpack.inventoryData.GetItemAt(_context.backpack.inventoryData.Size - 3).item;
        if (item == null)
        {
            _context.IsBlockingPress = false;
        }
        else
        {
            string itemName = item.name;
            if (itemName == "BloodSword")
            {
                _context.IsAttackPress = false;
            }
            else
            {
                _context.IsBlockingPress = false;
            }
        }

        _context.CanAttack = false;
        //if (_context.CheckOnFloor())
        //{
        //    _context.PlayerRigidbody.isKinematic = false;
        //    return;
        //}

        // Set Weapon and Player animation
        _context.Weapon.SetActive(true);
        _context.IsAttacking = true;
        _context.Weapon.GetComponent<Animator>().SetInteger("AttackCount", _context.AttackCount);
        _context.PlayerAnimator.SetInteger("AttackCount", _context.AttackCount);

        _context.Weapon.GetComponent<Animator>().SetBool("isAttack", true);
        _context.PlayerAnimator.SetBool("isAttack", true);

        _context.AttackCount++;

        attackTime = _context.getAnimationLength("PlayerAttack" + _context.AttackCount.ToString()); // set attack duration by getAnimationLength()
        attackCounter = 0f;
        //_context.applyAttackForce();
        _context.PlayerBoxCollider.sharedMaterial = _context.AttackingPhysics;
    }

    public override void UpdateState()
    {
        if (_context.CheckOnFloor())
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
        Debug.Log("WTF?????????????");
        _context.PlayerRigidbody.isKinematic = false;
        _context.Weapon.SetActive(false);
        _context.IsAttacking = false;

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
        ItemSO item = _context.backpack.inventoryData.GetItemAt(_context.backpack.inventoryData.Size - 3).item;
        bool attackPress;
        bool blockingPress;
        if (item == null)
        {
            attackPress = _context.IsBlockingPress;
            blockingPress = _context.IsAttackPress;
        }
        else
        {
            string itemName = item.name;
            if (itemName == "BloodSword")
            {
                attackPress = _context.IsAttackPress;
                blockingPress = _context.IsBlockingPress;
            }
            else
            {
                attackPress = _context.IsBlockingPress;
                blockingPress = _context.IsAttackPress;
            }
        }


        if (attackTime < attackCounter)
        {
            if (attackPress && _context.AttackCount < 3)
            {
                normalExit = true;
                _context.SwitchState(_factory.Attack());

            }
            else if (blockingPress)
            {
                normalExit = true;
                if (_context.AttackCount == 1 || _context.AttackCount == 3) // Critical Hit Reservation by not reset AttackCount
                    _context.AttackCount = 0;
                else
                    _context.startCorutine(CritReserve());
                _context.startCorutine(AttackCoolDown());
                _context.SwitchState(_factory.BlockingWeapon());
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
