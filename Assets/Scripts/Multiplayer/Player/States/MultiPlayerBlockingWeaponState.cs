using Inventory.Model;
using UnityEngine;

public class MultiPlayerBlockingWeaponState : MultiPlayerBaseState
{
    public MultiPlayerBlockingWeaponState(MultiPlayerStatesManager context, MultiPlayerStateFactory factory)
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
        ItemSO item = _context.backpack.inventoryData.GetItemAt(_context.backpack.inventoryData.Size - 2).item;
        if (item != null)
        {
            string itemName = item.name;
            if (itemName == "BloodSword")
            {
                _context.IsBlockingPress = false;
            }
        }
        else
        {
            _context.IsBlockingPress = false;
        }
    }

    public override void CheckSwitchState()
    {
        ItemSO item = _context.backpack.inventoryData.GetItemAt(_context.backpack.inventoryData.Size - 2).item;
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
