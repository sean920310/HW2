using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStatesManager context, PlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;

    }

    public override void EnterState()
    {
        _context.PlayerAnimator.SetBool("isMoving", true);
    }

    public override void UpdateState()
    {
        // inertia cancel
        if (_context.LastMove != Vector2.zero && _context.LastMove.x < 0f)
            _context.FacingLeft();
        else if (_context.LastMove != Vector2.zero && _context.LastMove.x > 0f)
            _context.FacingRight();

        CheckSwitchState();
    }
    public override void FixedUpdateState()
    {
        if (_context.PlayerRigidbody.velocity.x > 0 && _context.LastMove.x < 0 ||
            _context.PlayerRigidbody.velocity.x < 0 && _context.LastMove.x > 0)
            _context.PlayerRigidbody.velocity = new Vector2(0, _context.PlayerRigidbody.velocity.y);

        _context.MoveWithLimit();


        if(!_context.FootStepSound.isPlaying)
            _context.FootStepSound.Play();

        //_context.PlayerRigidbody.velocity = new Vector2(_context.LastMove.x * _context.PlayerMoveSpeed, _context.PlayerRigidbody.velocity.y);
    }

    public override void ExitState()
    {
        _context.PlayerAnimator.SetBool("isMoving", false);

        if (_context.FootStepSound.isPlaying)
            _context.FootStepSound.Pause();
    }

    public override void CheckSwitchState()
    {
        if (_context.LastMove == Vector2.zero)
        {
            _context.SwitchState(_factory.Idle());
        }
        else if (_context.IsJumpPress && _context.canJump())
        {
            _context.SwitchState(_factory.Jump());
        }
        else if (_context.IsCrouchPress && _context.CanSlide)
        {
            _context.SwitchState(_factory.Slide());
        }
        else if (_context.PlayerRigidbody.velocity.y < -0.001)
        {
            _context.SwitchState(_factory.Fall());
        }
        else if (_context.IsJumpDownPress && _context.CheckOnOneWayPlatform() && !_context.CheckOnGround())
        {
            _context.SwitchState(_factory.JumpDown());
        }
        else if (_context.IsAttackPress && _context.CanAttack)
        {
            _context.SwitchState(_factory.Attack());
        }
        else if (_context.IsBlockingPress)
        {
            _context.SwitchState(_factory.Blocking());
        }
    }
}
