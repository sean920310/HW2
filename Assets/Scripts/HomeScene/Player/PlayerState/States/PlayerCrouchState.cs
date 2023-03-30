using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    public PlayerCrouchState(PlayerStatesManager context, PlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        _context.PlayerAnimator.SetBool("isCrouch", true);
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
        _context.PlayerRigidbody.velocity = new Vector2(_context.LastMove.x * _context.PlayerCrouchMoveSpeed, _context.PlayerRigidbody.velocity.y);
    }

    public override void ExitState()
    {
        _context.PlayerAnimator.SetBool("isCrouch", false);
    }

    public override void CheckSwitchState()
    {
        if (!_context.IsCrouchPress)
        {
            _context.SwitchState(_factory.Idle());
        }
    }
}