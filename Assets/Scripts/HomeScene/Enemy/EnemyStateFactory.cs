public class EnemyStateFactory
{
    EnemyStateManager _context;

    public EnemyStateFactory(EnemyStateManager context)
    {
        _context = context;
    }

    public EnemyBaseState Idle()
    {
        return new EnemyIdleState(_context, this);
    }

    public EnemyBaseState Patrol()
    {
        return new EnemyPatrolState(_context, this);
    }

    public EnemyBaseState Chase()
    {
        return new EnemyChaseState(_context, this);
    }

    public EnemyBaseState Attack()
    {
        return new EnemyAttackState(_context, this);
    }
}
