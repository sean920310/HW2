using Inventory.Model;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    public PlayerBlockingState(PlayerStatesManager context, PlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        _context.PlayerAnimator.SetBool("onShield", true);

        _context.PlayerBoxCollider.sharedMaterial = _context.BlockingPhysics;
    }

    public override void UpdateState()
    {
        if (_context.LastMove != Vector2.zero && _context.LastMove.x < 0f)
            _context.FacingLeft();
        else if (_context.LastMove != Vector2.zero && _context.LastMove.x > 0f)
            _context.FacingRight();

        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {
        _context.PlayerAnimator.SetBool("onShield", false);
        _context.PlayerBoxCollider.sharedMaterial = _context.NormalPhysics;
    }

    public override void CheckSwitchState()
    {
        ItemSO item = _context.backpack.inventoryData.GetItemAt(_context.backpack.inventoryData.Size - 2).item;
        bool blockingPress = false;
        if (item != null)
        {
            string itemName = item.name;
            if (itemName == "Shield")
            {
                blockingPress = _context.IsBlockingPress;
                if (!blockingPress)
                {
                    _context.SwitchState(_factory.Idle());
                }
            }
            else
            {
                blockingPress = _context.IsAttackPress;
                if (!blockingPress)
                {
                    _context.SwitchState(_factory.Idle());
                }
            }
        }
        else
        {
            blockingPress = _context.IsAttackPress;
            if (!blockingPress)
            {
                _context.SwitchState(_factory.Idle());
            }
        }

    }
}
