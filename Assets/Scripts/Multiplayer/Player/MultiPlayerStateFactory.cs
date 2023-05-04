public class MultiPlayerStateFactory
{
    MultiPlayerStatesManager _context;

    public MultiPlayerStateFactory(MultiPlayerStatesManager context)
    {
        _context = context;
    }

    public MultiPlayerBaseState Idle()
    {
        return new MultiPlayerIdleState(_context, this);
    }
    public MultiPlayerBaseState Jump()
    {
        return new MultiPlayerJumpState(_context, this);
    }
    public MultiPlayerBaseState Walk()
    {
        return new MultiPlayerWalkState(_context, this);
    }
    public MultiPlayerBaseState WallSlide()
    {
        return new MultiPlayerWallSlideState(_context, this);
    }
    public MultiPlayerBaseState WallJump()
    {
        return new MultiPlayerWallJump(_context, this);
    }
    public MultiPlayerBaseState Crouch()
    {
        return new MultiPlayerCrouchState(_context, this);
    }
    public MultiPlayerBaseState Slide()
    {
        return new MultiPlayerSlideState(_context, this);
    }
    public MultiPlayerBaseState Fall()
    {
        return new MultiPlayerFallState(_context, this);
    }
    public MultiPlayerBaseState JumpDown()
    {
        return new MultiPlayerJumpDownState(_context, this);
    }
    public MultiPlayerBaseState Attack()
    {
        return new MultiPlayerAttackState(_context, this);
    }
    public MultiPlayerBaseState Blocking()
    {
        return new MultiPlayerBlockingState(_context, this);
    }
    public MultiPlayerBaseState Hurt()
    {
        return new MultiPlayerHurtState(_context, this);
    }
    public MultiPlayerBaseState MainWeapon()
    {
        return new MultiPlayerMainWeaponState(_context, this);
    }
    public MultiPlayerBaseState BlockingWeapon()
    {
        return new MultiPlayerBlockingWeaponState(_context, this);
    }
}
