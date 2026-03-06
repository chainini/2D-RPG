using UnityEngine;

public class ArcherBattleState : EnemyState
{
    Enemy_Archer enemy;
    Transform player;
    int moveDir;
    public ArcherBattleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Archer enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
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
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.saveDistance)
            {
                if (CanJump())
                    stateMachine.ChangeState(enemy.jumpState);
            }


            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.position, enemy.transform.position) > 5)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        ArcherBattleStateFilpLogic();
    }

    private void ArcherBattleStateFilpLogic()
    {
        if (enemy.transform.position.x > player.position.x && enemy.facingDir == 1)
            enemy.Filp();
        else if (enemy.transform.position.x < player.position.x && enemy.facingDir == -1)
            enemy.Filp();
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

    private bool CanJump()
    {
        if(!enemy.GroundBehindCheck() || enemy.WallBehindCheck())
            return false;
        

        if(Time.time > enemy.lastTimeJumped + enemy.jumpCooldown)
        {
            enemy.lastTimeJumped = Time.time;
            return true;
        }
        return false;
    }
}
