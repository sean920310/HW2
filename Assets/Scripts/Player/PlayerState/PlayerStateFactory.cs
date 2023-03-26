public class PlayerStateFactory
{
    PlayerStatesManager _context;

    public PlayerStateFactory(PlayerStatesManager context)
    {
        _context = context;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_context, this);
    }
    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(_context, this);
    }
    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(_context, this);
    }
    public PlayerBaseState WallSlide()
    {
        return new PlayerWallSlideState(_context, this);
    }
    public PlayerBaseState WallJump()
    {
        return new PlayerWallJump(_context, this);
    }
    public PlayerBaseState Crouch()
    {
        return new PlayerCrouchState(_context, this);
    }
    public PlayerBaseState Slide()
    {
        return new PlayerSlideState(_context, this);
    }
    public PlayerBaseState Fall()
    {
        return new PlayerFallState(_context, this);
    }
    public PlayerBaseState JumpDown()
    {
        return new PlayerJumpDownState(_context, this);
    }
}
