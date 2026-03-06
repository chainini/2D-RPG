using UnityEngine;

public class DeathBrinerBattleState : EnemyState
{
    Enemy_DeathBriner_Boss enemy;
    Transform player;
    int moveDir;
    public DeathBrinerBattleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_DeathBriner_Boss enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            //stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
                else
                    stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (enemy.transform.position.x > player.position.x)
            moveDir = -1;
        else if (enemy.transform.position.x < player.position.x)
            moveDir = 1;

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance <= enemy.attackDistance)
            return;

        enemy.SetVelocity(moveDir * enemy.moveSpeed, rb.velocity.y);


    }

    private bool CanAttack()
    {
        if (Time.time > enemy.lastAttackTime + enemy.attackCoolDown)
        {
            enemy.attackCoolDown = Random.Range(enemy.minAttackCoolDown, enemy.maxAttackCoolDown);
            enemy.lastAttackTime = Time.time;
            return true;
        }
        return false;
    }
}
