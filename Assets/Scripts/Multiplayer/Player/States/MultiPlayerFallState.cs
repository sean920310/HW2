using UnityEngine;

public class MultiPlayerFallState : MultiPlayerBaseState
{
    public MultiPlayerFallState(MultiPlayerStatesManager context, MultiPlayerStateFactory factory)
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
        _context.MoveWithLimit();
    }

    public override void ExitState()
    {
        _context.PlayerAnimator.SetFloat("velocityY", 0f);
        _context.PlayerAnimator.SetBool("onGround", true);
        _context.PlayerRigidbody.gravityScale = _context.NormalGravityScale;
    }

    public override void CheckSwitchState()
    {
        if (_context.CheckOnFloor())
        {
            _context.JumpCountsLeft = _context.JumpCounts; // Jump Counts Reloading
            _context.OnGroundParticle.Play();
            _context.SwitchState(_factory.Idle());
        }
        else if (_context.CheckIsTouchingWall() && !_context.CheckOnFloor() && _context.PlayerRigidbody.velocity.y < 0f)
        {
            _context.JumpCountsLeft = _context.JumpCounts;
            _context.SwitchState(_factory.WallSlide());
        }
        else if (_context.IsJumpDownPress && _context.CheckOnOneWayPlatform())
        {
            _context.SwitchState(_factory.JumpDown());
        }
        else if (_context.IsAttackPress)
        {
            _context.SwitchState(_factory.MainWeapon());
        }
        else if (_context.IsBlockingPress)
        {
            _context.JumpCountsLeft = _context.JumpCounts;
            _context.SwitchState(_factory.BlockingWeapon());
        }
        else if (_context.IsJumpPress && _context.canJump())
        {
            _context.SwitchState(_factory.Jump()); // Jump could not reload Jump Counts
        }
        else if (_context.IsCrouchPress && _context.CanSlide && _context.IsAirSlideEnable)
        {
            _context.SwitchState(_factory.Slide());
        }
    }
}
