public class Enemy_Skeleton : Enemy
{
    #region State
    public SkeletionIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunedState stunedState { get; private set; }
    public SkeletonDieState dieState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletionIdleState(stateMachine, this, "Idle", this);
        moveState = new SkeletonMoveState(stateMachine, this, "Move", this);
        battleState = new SkeletonBattleState(stateMachine, this, "Move", this);
        attackState = new SkeletonAttackState(stateMachine, this, "Attack", this);
        stunedState = new SkeletonStunedState(stateMachine, this, "Stuned", this);
        dieState = new SkeletonDieState(stateMachine, this, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStuned()
    {
        if (base.CanBeStuned())
        {
            stateMachine.ChangeState(stunedState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(dieState);
    }
}
