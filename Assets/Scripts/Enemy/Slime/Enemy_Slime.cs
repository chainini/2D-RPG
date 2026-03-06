using UnityEngine;

public enum SlimeType { big, medium, small }

public class Enemy_Slime : Enemy
{
    [Header("Slime spesific")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int slimeToCreate;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minCreationVelocity;
    [SerializeField] private Vector2 maxCreationVelocity;

    #region State
    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunedState stunedState { get; private set; }
    public SlimeDeadState dieState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        idleState = new SlimeIdleState(stateMachine, this, "Idle", this);
        moveState = new SlimeMoveState(stateMachine, this, "Move", this);
        battleState = new SlimeBattleState(stateMachine, this, "Move", this);
        attackState = new SlimeAttackState(stateMachine, this, "Attack", this);
        stunedState = new SlimeStunedState(stateMachine, this, "Stune", this);
        dieState = new SlimeDeadState(stateMachine, this, "Idle", this);
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

        if (slimeType == SlimeType.small)
            return;

        CreateSlime(slimeToCreate, slimePrefab);
    }

    private void CreateSlime(int _amountOfSlime,GameObject _slimePrefab)
    {
        for(int i = 0; i < _amountOfSlime; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab,transform.position, Quaternion.identity);

            newSlime.GetComponent<Enemy_Slime>().SetupSlime(facingDir);
        }
    }

    public void SetupSlime(int _facingDir)
    {
        if (_facingDir != facingDir)
            Filp();

        float xVelocity = Random.Range(minCreationVelocity.x, maxCreationVelocity.x);
        float yVelocity = Random.Range(minCreationVelocity.y, maxCreationVelocity.y);

        isKnock = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, yVelocity);

        Invoke("CancelKnockBack", 1.5f);
    }

    private void CancelKnockBack()=>isKnock = false;
}
