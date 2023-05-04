using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditHurtState : BanditBaseState
{
    private Color hurtColor;
    public BanditHurtState(BanditStateManager context, BanditStateFactory factory)
           : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        hurtColor = _context.HurtColor;
        _context.Material.shader = _context.ColorTintShader;
        _context.Material.SetColor("_TintColor", hurtColor);
        _context.Anim.SetTrigger("Hurt");
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {
        if(hurtColor.a > 0)
        {
            hurtColor.a = Mathf.Clamp01(hurtColor.a - _context.HurtFadeSpeed * Time.deltaTime);
            _context.Material.SetColor("_TintColor", hurtColor);
        }
    }

    public override void ExitState()
    {
        _context.Material.shader = _context.AttackingShader;
        _context.Anim.ResetTrigger("Hurt");
    }

    public override void CheckSwitchState()
    {
        if(hurtColor.a<=0)
        {
            _context.SwitchState(_factory.Patrol());
        }
    }
}
