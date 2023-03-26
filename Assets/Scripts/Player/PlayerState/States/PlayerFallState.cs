using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStatesManager context, PlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        _context.PlayerAnimator.SetBool("onGround", false);
        _context.PlayerAnimator.SetFloat("velocityY", -0.01f);
    }

    public override void UpdateState()
    {
        _context.PlayerRigidbody.gravityScale = _context.FallGravityScale;

        // air control
        if (_context.LastMove != Vector2.zero && _context.LastMove.x < 0f)
            _context.FacingLeft();
        else if (_context.LastMove != Vector2.zero && _context.LastMove.x > 0f)
            _context.FacingRight();

        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {
        _context.PlayerRigidbody.velocity = new Vector2(_context.LastMove.x * _context.PlayerMoveSpeed, _context.PlayerRigidbody.velocity.y);
    }

    public override void ExitState()
    {
        _context.PlayerAnimator.SetFloat("velocityY", 0f);
        _context.PlayerAnimator.SetBool("onGround", true);
        _context.PlayerRigidbody.gravityScale = _context.NormalGravityScale;
    }

    public override void CheckSwitchState()
    {
        if (_context.CheckOnGround())
        {
            _context.JumpCountsLeft = _context.JumpCounts;
            _context.SwitchState(_factory.Idle());
        }
        else if (_context.CheckIsTouchingWall() && !_context.CheckOnGround() && _context.PlayerRigidbody.velocity.y < 0f)
        {
            _context.SwitchState(_factory.WallSlide());
        }
        else if (_context.IsJumpPress && _context.canJump())
        {
            _context.SwitchState(_factory.Jump());
        }
    }
}
