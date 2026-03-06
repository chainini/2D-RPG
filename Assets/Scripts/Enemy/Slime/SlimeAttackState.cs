using UnityEngine;

public class SlimeAttackState : EnemyState
{
    Enemy_Slime enemy;
    public SlimeAttackState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Slime enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.lastAttackTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.ZeroVelocity();

        if (animTrigger)
            stateMachine.ChangeState(enemy.battleState);
    }
}
