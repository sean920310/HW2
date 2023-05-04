using UnityEngine;

public class PlayerHurtState : PlayerBaseState
{
    float hurtStiffTime;
    float HurtStiffCounter;

    public PlayerHurtState(PlayerStatesManager context, PlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        _context.PlayerAnimator.SetTrigger("isHurt");

        hurtStiffTime = _context.getAnimationLength("Hurt");
        HurtStiffCounter = 0;
    }

    public override void UpdateState()
    {
        HurtStiffCounter += Time.deltaTime;
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {
        if(HurtStiffCounter >= hurtStiffTime) 
        {
            _context.SwitchState(_factory.Idle());
        }
    }
}
