using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBrinerAttackState : EnemyState
{
    Enemy_DeathBriner_Boss enemy;
    public DeathBrinerAttackState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_DeathBriner_Boss enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.lastAttackTime = Time.time;
        enemy.chanceToTeleport += 5;
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
        {
            if(enemy.canTeleport())
                stateMachine.ChangeState(enemy.teleportState);
            else
                stateMachine.ChangeState(enemy.battleState);
        }
    }
}
