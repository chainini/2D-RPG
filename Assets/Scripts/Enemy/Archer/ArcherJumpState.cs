using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherJumpState : EnemyState
{
    Enemy_Archer enemy;
    public ArcherJumpState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Archer enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(enemy.JumpVelocity.x * -enemy.facingDir, enemy.JumpVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.anim.SetFloat("yVelocity",enemy.rb.velocity.y);

        if (rb.velocity.y < 0 && enemy.isGrounded())
            stateMachine.ChangeState(enemy.battleState);
    }
}
