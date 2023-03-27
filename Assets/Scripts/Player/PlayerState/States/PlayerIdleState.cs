using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStatesManager context, PlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        _context.PlayerRigidbody.velocity = new Vector2(0f, _context.PlayerRigidbody.velocity.y);

        _context.AttackCount = 0;

        if (_context.CheckOnGround())
        {
            _context.PlayerRigidbody.isKinematic = true;
            _context.PlayerAnimator.SetBool("onGround", true);
            _context.PlayerRigidbody.velocity = Vector2.zero;
        }
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
        _context.PlayerRigidbody.isKinematic = false;
    }

    public override void CheckSwitchState()
    {
        if (_context.IsMovePress)
        {
            _context.SwitchState(_factory.Walk());
        }
        else if (_context.IsJumpPress && _context.canJump())
        {
            _context.SwitchState(_factory.Jump());
        }
        else if (_context.IsCrouchPress)
        {
            _context.SwitchState(_factory.Crouch());
        }
        else if (_context.PlayerRigidbody.velocity.y < -0.001)
        {
            _context.SwitchState(_factory.Fall());
        }
        else if (_context.IsJumpDownPress && _context.CheckOnOneWayPlatform())
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
