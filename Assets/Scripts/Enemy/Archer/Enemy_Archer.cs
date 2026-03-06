using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Enemy_Archer : Enemy
{
    [Header("Archer specific info")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float arrowDamage;

    public Vector2 JumpVelocity;
    public float jumpCooldown;
    public float saveDistance;//how close Archer should jump on battleState
    [HideInInspector]public float lastTimeJumped;

    [Header("Additional collision check")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;

    #region State
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    public ArcherStunedState stunedState { get; private set; }
    public ArcherDeadState dieState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new ArcherIdleState(stateMachine, this, "Idle", this);
        moveState = new ArcherMoveState(stateMachine, this, "Move", this);
        battleState = new ArcherBattleState(stateMachine, this, "Idle", this);
        attackState = new ArcherAttackState(stateMachine, this, "Attack", this);
        stunedState = new ArcherStunedState(stateMachine, this, "Stuned", this);
        dieState = new ArcherDeadState(stateMachine, this, "Idle", this);
        jumpState = new ArcherJumpState(stateMachine, this, "Jump", this);
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


    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(arrowPrefab, attackCheck.position, Quaternion.identity);

        newArrow.GetComponent<Arrow_Controller>()?.SetupArrow(arrowSpeed*facingDir, stats);
    }

    public bool GroundBehindCheck() => Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, 0, whatIsGround);
    public bool WallBehindCheck() => Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDir, wallCheckDistance + 2, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindCheckSize);
    }
}
