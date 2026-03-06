using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{
    public string lastAnimBoolName;


    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stuned info")]
    public float stunedDuration = 1;
    public Vector2 stunedDirection = new Vector2(10,12);
    protected bool canStuned;
    [SerializeField] private GameObject counterAttackWindow;

    [Header("Move info")]
    public float idleTime = 2;
    public float moveSpeed = 1.5f;
    public float battleTime = 2;
    public float defaultMoveSpeed;

    [Header("Attack info")]
    public float agroDistance=2;
    public float attackDistance = 2;
    public float attackCoolDown;
    public float maxAttackCoolDown = 1;
    public float minAttackCoolDown = 2;
    [HideInInspector] public float lastAttackTime;
    public EnemyStateMachine stateMachine { get; private set; }
    public EntityFX entityFX { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }
    protected override void Start()
    {
        base.Start();
        entityFX = GetComponentInChildren<EntityFX>();
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public override void SlowEntityBy(float _percentage, float _slowDration)
    {
        moveSpeed = moveSpeed * (1 - _percentage);
        anim.speed = anim.speed * (1 - _percentage);

        Invoke("ReturnDufaultSpeed", _slowDration);
    }
    protected override void ReturnDufaultSpeed()
    {
        base.ReturnDufaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }

    public virtual void AssignlastAnimBoolName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    /// <summary>
    /// 冻结敌人
    /// </summary>
    /// <param name="_isFronzen">是否冻结</param>
    public virtual void FreezeTime(bool _isFronzen)
    {
        if (_isFronzen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }
    /// <summary>
    /// 调用了 Enemy里的协程FrozenFor
    /// </summary>
    /// <param name="_duration">持续时间</param>
    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FrozenFor(_duration));

    /// <summary>
    /// 用协程调用 冻结方法
    /// </summary>
    /// <param name="_seconds">持续时间</param>
    /// <returns></returns>
    public virtual IEnumerator FrozenFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    #region Counter Attack Window
    /// <summary>
    /// 打开反击窗口
    /// </summary>
    public virtual void OpenConterAttackWindow()
    {
        canStuned = true;
        counterAttackWindow.SetActive(true);
    }
    /// <summary>
    /// 关闭反击窗口
    /// </summary>
    public virtual void CloseConterAttackWindow()
    {
        canStuned = false;
        counterAttackWindow.SetActive(false);
    }
    #endregion

    /// <summary>
    /// 是否能被反击
    /// </summary>
    /// <returns></returns>
    public virtual bool CanBeStuned()
    {
        if (canStuned)
        {
            CloseConterAttackWindow();
            return true;
        }
        return false;
    }
    public override void Die()
    {
        base.Die();
        entityFX.igniteFx.Stop();
        entityFX.chillFx.Stop();
        entityFX.shockFx.Stop();
    }
    /// <summary>
    /// 动画结束
    /// </summary>
    public virtual void AttackAnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    /// <summary>
    /// 特殊敌人攻击
    /// </summary>
    public virtual void AnimationSpecialAttackTrigger()
    {

    }
    /// <summary>
    /// 特殊敌人攻击后的逻辑
    /// </summary>
    public virtual void AnimationSpecialAttackTriggerOver()
    {

    }

    /// <summary>
    /// 玩家检测
    /// </summary>
    /// <returns></returns>
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }


}
