using System.Collections;
using UnityEngine;

public class PlayerSlideState : PlayerBaseState
{
    public PlayerSlideState(PlayerStatesManager context, PlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        _context.CanSlide = false;

        _context.PlayerAnimator.SetBool("isSlide", true);
        _context.PlayerAnimator.SetFloat("velocityX", 1f);

        _context.PlayerRigidbody.AddForce(new Vector2(_context.facingDirection() * _context.PlayerSlideForce, 0), ForceMode2D.Impulse);

        _context.PlayerBoxCollider.size = _context.SlideColliderSize;
        _context.PlayerBoxCollider.offset = _context.SlideColliderShift;

        _context.PlayerBoxCollider.sharedMaterial = _context.SlidePhysics;
    }

    public override void UpdateState()
    {
        if (_context.LastMove != Vector2.zero && _context.LastMove.x < 0f)
            _context.FacingLeft();
        else if (_context.LastMove != Vector2.zero && _context.LastMove.x > 0f)
            _context.FacingRight();

        _context.PlayerAnimator.SetFloat("velocityX", Mathf.Abs(_context.PlayerRigidbody.velocity.x));

        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {
        _context.PlayerRigidbody.velocity = new Vector2(_context.facingDirection() * Mathf.Abs(_context.PlayerRigidbody.velocity.x), _context.PlayerRigidbody.velocity.y);
    }

    public override void ExitState()
    {
        _context.startCorutine(SlideCoolDown());
        _context.PlayerAnimator.SetBool("isSlide", false);
        _context.PlayerAnimator.SetFloat("velocityX", 0f);
        _context.PlayerBoxCollider.size = _context.NormalColliderSize;
        _context.PlayerBoxCollider.offset = _context.NormalColliderShift;

        _context.PlayerBoxCollider.sharedMaterial = _context.NormalPhysics;
    }

    public override void CheckSwitchState()
    {
        if (_context.IsCrouchRelease)
        {
            _context.SwitchState(_factory.Idle());
        }
        else if (_context.PlayerRigidbody.velocity.y >= -0.1)
        {
            //_context.SwitchState(_factory.Fall());
        }
    }

    IEnumerator SlideCoolDown()
    {
        yield return new WaitForSeconds(_context.PlayerSlideCoolDownTime);
        _context.CanSlide = true;
    }
}
