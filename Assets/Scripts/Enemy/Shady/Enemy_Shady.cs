using UnityEngine;

public class Enemy_Shady : Enemy
{
    [Header("Shady special info")]
    public float battleMoveSpeed;
    [SerializeField] private GameObject shadyExploedPrefab;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxSize;


    #region State
    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyBattleState battleState { get; private set; }
    public ShadyStunedState stunedState { get; private set; }
    public ShadyDeadState dieState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new ShadyIdleState(stateMachine, this, "Idle", this);
        moveState = new ShadyMoveState(stateMachine, this, "Move", this);
        battleState = new ShadyBattleState(stateMachine, this, "MoveFast", this);
        stunedState = new ShadyStunedState(stateMachine, this, "Stun", this);
        dieState = new ShadyDeadState(stateMachine, this, "Dead", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
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

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newExploed = Instantiate(shadyExploedPrefab,attackCheck.position,Quaternion.identity);
        newExploed.GetComponent<ShadyExplosion_Controller>().SetupExploed(stats, growSpeed, maxSize, attackRadius);

        cd.enabled = false;
        rb.gravityScale = 0;
    }
    public override void AnimationSpecialAttackTriggerOver()
    {
        SelfDestory();
    }

    public void SelfDestory()
    {
        Destroy(gameObject);
        stats.killEnetity();
    }
}
