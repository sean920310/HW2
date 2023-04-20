public class DemonStateFactory
{
    DemonStateManager _context;

    public DemonStateFactory(DemonStateManager context)
    {
        _context = context;
    }

    public DemonBaseState Idle()
    {
        return new DemonIdleState(_context, this);
    }

    public DemonBaseState Patrol()
    {
        return new DemonPatrolState(_context, this);
    }

    public DemonBaseState Chase()
    {
        return new DemonChaseState(_context, this);
    }

    public DemonBaseState Attack()
    {
        return new DemonAttackState(_context, this);
    }
}
