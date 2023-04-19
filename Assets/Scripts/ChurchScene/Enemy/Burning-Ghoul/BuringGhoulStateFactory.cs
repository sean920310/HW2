public class BurningGhoulStateFactory
{
    BurningGhoulStateManager _context;

    public BurningGhoulStateFactory(BurningGhoulStateManager context)
    {
        _context = context;
    }

    public BurningGhoulBaseState Patrol()
    {
        return new BurningGhoulPatrolState(_context, this);
    }

    public BurningGhoulBaseState Chase()
    {
        return new BurningGhoulChaseState(_context, this);
    }

    public BurningGhoulBaseState Attack()
    {
        return new BurningGhoulAttackState(_context, this);
    }
}
