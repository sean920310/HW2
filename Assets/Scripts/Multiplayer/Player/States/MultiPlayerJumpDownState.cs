using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerJumpDownState : MultiPlayerBaseState
{
    public MultiPlayerJumpDownState(MultiPlayerStatesManager context, MultiPlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        _context.gameObject.layer = 0;   //set layer to default that don't collide with one way platform
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
        _context.gameObject.layer = 8;   //set layer to player
    }

    public override void CheckSwitchState()
    {
        if(!_context.CheckOnOneWayPlatform())
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
        }
    }
}
