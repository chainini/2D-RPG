using UnityEngine;

public class SlimeStunedState : EnemyState
{
    Enemy_Slime enemy;
    public SlimeStunedState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Slime enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.entityFX.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunedDuration;

        //这一行不起作用 Velocity的值被别的什么地方 覆盖了  值为20左右 为什么？ 
        //解决办法 ？
        //rb.velocity = new Vector2(-enemy.facingDir * 0, enemy.stunedDirection.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.ZeroVelocity();
        enemy.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();
        Debug.Log(rb.velocity.x);
        if (rb.velocity.y < .1f && enemy.isGrounded())
        {
            enemy.entityFX.Invoke("CancelColorChange", 0);
            enemy.anim.SetTrigger("StuneFold");
            enemy.stats.MakeInvincible(true);
        }

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
