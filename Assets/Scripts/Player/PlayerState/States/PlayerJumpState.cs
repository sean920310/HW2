using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStatesManager context, PlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        _context.JumpCountsLeft--;
        _context.IsJumpPress = false;
        _context.PlayerRigidbody.velocity = new Vector2(_context.PlayerRigidbody.velocity.x, 0);
        _context.JumpTimeCounter = 0;

        // set animation veriable
        _context.PlayerAnimator.SetBool("onGround", false);
        _context.PlayerAnimator.SetTrigger("isJump");

        // calculate jump force base on jump height
        float jumpForce = Mathf.Sqrt(_context.JumpHeight * -2 * (Physics2D.gravity.y * _context.PlayerRigidbody.gravityScale));
        _context.PlayerRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

    }

    public override void UpdateState()
    {
        _context.JumpTimeCounter += Time.deltaTime;

        if ((_context.IsJumpRelease || _context.JumpTimeCounter >= _context.JumpTime) && _context.PlayerRigidbody.velocity.y > 0)
            _context.PlayerRigidbody.AddForce(Vector2.down * _context.JumpCancelRate);

        _context.PlayerAnimator.SetFloat("velocityY", _context.PlayerRigidbody.velocity.y);

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
    }

    public override void CheckSwitchState()
    {
        if (_context.PlayerRigidbody.velocity.y < -0.001)
        {
            _context.SwitchState(_factory.Fall());
        }
        else if (_context.CheckIsTouchingWall() && !_context.CheckOnGround() && _context.PlayerRigidbody.velocity.y < 0f)
        {
            _context.SwitchState(_factory.WallSlide());
        }
        else if (_context.IsJumpPress && _context.canJump())
        {
            _context.SwitchState(_factory.Jump());
        }
        else if (_context.IsAttackPress)
        {
            _context.SwitchState(_factory.Attack());
        }
        //else if (_context.CheckOnGround())
        //{
        //    _context.JumpCountsLeft = _context.JumpCounts;
        //    _context.SwitchState(_factory.Idle());
        //}
    }
}
