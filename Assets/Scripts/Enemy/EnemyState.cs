using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine { get; private set; }
    protected Enemy enemyBase { get; private set; }
    protected Rigidbody2D rb;

    private string animBoolName;

    protected float stateTimer;
    protected bool animTrigger;
    public EnemyState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.enemyBase = enemyBase;
        this.animBoolName = animBoolName;
    }

    /// <summary>
    /// 进入状态 调用一次
    /// </summary>
    public virtual void Enter()
    {
        animTrigger = false;

        rb = enemyBase.rb;

        enemyBase.anim.SetBool(animBoolName, true);

    }
    /// <summary>
    /// 退出状态 调用一次
    /// </summary>
    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignlastAnimBoolName(animBoolName);
    }
    /// <summary>
    /// 每帧执行  在Enemy的Update调用
    /// </summary>
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    /// <summary>
    /// 可用于动画完成后的逻辑
    /// </summary>
    public virtual void AnimationFinishTrigger()
    {
        animTrigger = true;
    }

}
