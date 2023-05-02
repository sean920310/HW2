using UnityEngine;
using Inventory.Model;

public class MultiPlayerMainWeaponState : MultiPlayerBaseState
{
    public MultiPlayerMainWeaponState(MultiPlayerStatesManager context, MultiPlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        //ItemSO item = _context.backpack.inventoryData.GetItemAt(_context.backpack.inventoryData.Size - 3).item;
        //if (item == null)
        //{
        //    _context.IsAttackPress = false;
        //    _context.SwitchState(_factory.Idle());
        //}
        //else
        //{
        //    string itemName = item.name;
        //    if (itemName == "BloodSword" && _context.CanAttack)
        //    {
        //        _context.IsAttackPress = false;
        //        _context.SwitchState(_factory.Attack());
        //    }
        //    else if (itemName == "Shield") 
        //    {
        //        _context.SwitchState(_factory.Blocking());
        //    }
        //    else if (itemName == "Potion")
        //    {
        //        _context.SwitchState(_factory.Idle());
        //    }
        //    else
        //    {
        //        _context.IsAttackPress = false;
        //        _context.SwitchState(_factory.Idle());
        //    }
        //}
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
        ItemSO item = _context.backpack.inventoryData.GetItemAt(_context.backpack.inventoryData.Size - 3).item;
        if (item != null)
        {
            string itemName = item.name;
            if (itemName == "BloodSword")
            {
                _context.IsAttackPress = false;
            }
        }
        else
        {
            
            // _context.IsAttackPress = false;
        }
    }

    public override void CheckSwitchState()
    {
        ItemSO item = _context.backpack.inventoryData.GetItemAt(_context.backpack.inventoryData.Size - 3).item;
        if (item == null)
        {
            _context.SwitchState(_factory.Idle());
        }
        else
        {
            string itemName = item.name;

            if (itemName == "BloodSword" && _context.CanAttack)
            {
                _context.SwitchState(_factory.Attack());
            }
            else if (itemName == "Shield")
            {
                _context.IsAttackPress = true;
                _context.SwitchState(_factory.Blocking());
            }
            else if (itemName == "Potion")
            {
                _context.SwitchState(_factory.Idle());
            }
            else
            {
                _context.SwitchState(_factory.Idle());
            }
        }
    }
}
