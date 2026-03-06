using UnityEngine;

public class SkeletonDieState : EnemyState
{
    Enemy_Skeleton enemy;
    public SkeletonDieState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Skeleton enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);

        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .2f;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 10);
        }
    }
}
