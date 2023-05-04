public class BanditStateFactory
{
    BanditStateManager _context;

    public BanditStateFactory(BanditStateManager context)
    {
        _context = context;
    }

    public BanditBaseState Patrol()
    {
        return new BanditPatrolState(_context, this);
    }

    public BanditBaseState Chase()
    {
        return new BanditChaseState(_context, this);
    }

    public BanditBaseState Attack()
    {
        return new BanditAttackState(_context, this);
    }

    public BanditBaseState Hurt()
    {
        return new BanditHurtState(_context, this);
    }
}
