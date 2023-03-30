using UnityEngine;

public class PlayerWallSlideState : PlayerBaseState
{
    public PlayerWallSlideState(PlayerStatesManager context, PlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        _context.PlayerAnimator.SetBool("isWallSliding", true);
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
        _context.PlayerRigidbody.velocity = new Vector2(_context.LastMove.x * _context.PlayerMoveSpeed, _context.PlayerRigidbody.velocity.y);

        if (_context.PlayerRigidbody.velocity.y < -_context.WallSlideSpeed)
        {
            _context.PlayerRigidbody.velocity = new Vector2(_context.PlayerRigidbody.velocity.x, -_context.WallSlideSpeed);
        }
    }

    public override void ExitState()
    {
        _context.PlayerAnimator.SetBool("isWallSliding", false);
    }

    public override void CheckSwitchState()
    {
        if (_context.CheckOnGround() || !_context.CheckIsTouchingWall())
        {
            _context.SwitchState(_factory.Idle());
        }
        else if (_context.IsJumpPress) // wall jump
        {
            _context.SwitchState(_factory.WallJump());
        }
    }
}
