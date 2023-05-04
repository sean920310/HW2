using UnityEngine;

public class MultiPlayerWallJump : MultiPlayerBaseState
{
    public MultiPlayerWallJump(MultiPlayerStatesManager context, MultiPlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        _context.JumpCountsLeft = 1;
        _context.IsJumpPress = false;
        _context.JumpTimeCounter = 0;
        _context.WallJumpStiffTimeCounter = 0;

        // set animation veriable
        _context.PlayerAnimator.SetBool("onGround", false);
        _context.PlayerAnimator.SetBool("isJump", true);

        // calculate jump force base on jump height
        Vector2 jumpForce = new Vector2(-_context.facingDirection() * _context.WallJumpForce * _context.WallJumpDirection.x, _context.WallJumpForce * _context.WallJumpDirection.y);
        _context.PlayerRigidbody.AddForce(jumpForce, ForceMode2D.Impulse);

        if (_context.facingDirection() > 0)
            _context.FacingLeft();
        else
            _context.FacingRight();
    }

    public override void UpdateState()
    {
        _context.JumpTimeCounter += Time.deltaTime;
        _context.WallJumpStiffTimeCounter += Time.deltaTime;

        if ((_context.IsJumpRelease || _context.JumpTimeCounter >= _context.JumpTime) && _context.PlayerRigidbody.velocity.y > 0)
            _context.PlayerRigidbody.AddForce(Vector2.down * _context.JumpCancelRate);

        if (_context.PlayerRigidbody.velocity.y < 0)
            _context.PlayerRigidbody.gravityScale = _context.FallGravityScale;

        _context.PlayerAnimator.SetFloat("velocityY", _context.PlayerRigidbody.velocity.y);

        // air control
        if (_context.WallJumpStiffTimeCounter >= _context.WallJumpStiffTime)
        {
            if (_context.LastMove != Vector2.zero && _context.LastMove.x < 0f)
                _context.FacingLeft();
            else if (_context.LastMove != Vector2.zero && _context.LastMove.x > 0f)
                _context.FacingRight();

        }

        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {
        if(_context.WallJumpStiffTimeCounter >= _context.WallJumpStiffTime && Mathf.Abs(_context.LastMove.x) > 0f)
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
            _context.JumpCountsLeft = _context.JumpCounts;
            _context.SwitchState(_factory.Idle());
        }
        else if (_context.CheckIsTouchingWall() && !_context.CheckOnFloor() && _context.PlayerRigidbody.velocity.y < 0f)
        {
            _context.SwitchState(_factory.WallSlide());
        }
        else if (_context.IsJumpPress && _context.canJump())
        {
            _context.SwitchState(_factory.Jump());
        }
        else if (_context.IsAttackPress)
        {
            _context.SwitchState(_factory.MainWeapon());
        }
        else if (_context.IsBlockingPress)
        {
            _context.SwitchState(_factory.BlockingWeapon());
        }
    }
}
