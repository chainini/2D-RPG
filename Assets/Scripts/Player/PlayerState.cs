using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;
    protected float inputX;
    protected float inputY;

    private string animBoolName;

    protected bool animTrigger;

    protected float stateTimer;

    public PlayerState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }

    /// <summary>
    /// 进入状态 调用一次
    /// </summary>
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;

        animTrigger = false;
    }
    /// <summary>
    /// 每帧更新  在Player里面的Update更新
    /// </summary>
    public virtual void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        player.anim.SetFloat("yVelocity", rb.velocity.y);

        stateTimer -= Time.deltaTime;
    }
    /// <summary>
    /// 退出状态时 调用一次
    /// </summary>
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    /// <summary>
    /// 可用于动画完成后的逻辑
    /// </summary>
    public virtual void AnimationFinish()
    {
        animTrigger = true;
    }
}
