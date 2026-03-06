using UnityEngine;

public class ArcherAttackState : EnemyState
{
    Enemy_Archer enemy;
    public ArcherAttackState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Archer enemy) : base(stateMachine, enemyBase, animBoolName)
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
