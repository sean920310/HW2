using UnityEngine;

public class MultiPlayerHurtState : MultiPlayerBaseState
{
    float hurtStiffTime;
    float HurtStiffCounter;

    public MultiPlayerHurtState(MultiPlayerStatesManager context, MultiPlayerStateFactory factory)
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
