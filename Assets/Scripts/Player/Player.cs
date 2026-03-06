using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration;
    public bool isBusy { get; private set; }

    [Header("Move info")]
    public float moveSpeed;
    public float jumpForce;
    public float catchSwordForce;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash info")]
    public float dashDuration;
    public float dashSpeed;
    private float defaultDashSpeed;

    public float dashDir { get; private set; }

    public SkillManager skill { get; private set; }
    public GameObject sword;
    public PlayerFX playerFX;

    #region State
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerIdolState idolState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }

    public PlayerPrimaryAttack primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }

    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatckSwordState catchSwordState { get; private set; }
    public PlayerBlackHoleState blackHoleState { get; private set; }

    public PlayerDieState dieState { get; private set; }
    #endregion


    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();
        idolState = new PlayerIdolState(stateMachine, this, "Idle");
        moveState = new PlayerMoveState(stateMachine, this, "Move");
        jumpState = new PlayerJumpState(stateMachine, this, "Jump");
        airState = new PlayerAirState(stateMachine, this, "Jump");
        dashState = new PlayerDashState(stateMachine, this, "Dash");
        wallSlideState = new PlayerWallSlideState(stateMachine, this, "WallSlide");
        wallJumpState = new PlayerWallJumpState(stateMachine, this, "Jump");

        primaryAttack = new PlayerPrimaryAttack(stateMachine, this, "Attack");
        counterAttackState = new PlayerCounterAttackState(stateMachine, this, "CounterAttack");

        aimSwordState = new PlayerAimSwordState(stateMachine, this, "AimSword");
        catchSwordState = new PlayerCatckSwordState(stateMachine, this, "CatchSword");
        blackHoleState = new PlayerBlackHoleState(stateMachine, this, "Jump");

        dieState = new PlayerDieState(stateMachine, this, "Die");

    }




    protected override void Start()
    {
        base.Start();
        skill = SkillManager.instance;

        stateMachine.Initialize(idolState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        if (Time.timeScale == 0)
            return;

        base.Update();

        stateMachine.currentState.Update();

        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.C) && skill.crystal_Skill.crystalUnlocked)
        {
            SkillManager.instance.crystal_Skill.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InventoryManager.Instance.UseFlask();
        }

    }

    public override void SlowEntityBy(float _percentage, float _slowDration)
    {
        moveSpeed = moveSpeed * (1 - _percentage);
        jumpForce = jumpForce * (1 - _percentage);
        dashSpeed = dashSpeed * (1 - _percentage);
        anim.speed = anim.speed * (1 - _percentage);

        Invoke("ReturnDufaultSpeed", _slowDration);
    }

    protected override void ReturnDufaultSpeed()
    {
        base.ReturnDufaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public void SginSword(GameObject _newSword)
    {
        sword = _newSword;
    }
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    /// <summary>
    /// 调用了  PlayerState里的AnimationFinish
    /// </summary>
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinish();

    /// <summary>
    /// 检查Dash的输入
    /// </summary>
    private void CheckForDashInput()
    {
        if (isWallDetected())
            return;

        if (skill.dash_Skill.dashUnlocked == false)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash_Skill.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();
        playerFX.igniteFx.Stop();
        playerFX.chillFx.Stop();
        playerFX.shockFx.Stop();
        stateMachine.ChangeState(dieState);
    }

    protected override void SetupZeroKnockBackPower()
    {
        knockBackPower = new Vector2(0, 0);
    }

}
