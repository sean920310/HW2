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
        if(!_context.IsBlockingPress)
        {
            _context.SwitchState(_factory.Idle());
        }
    }
}
