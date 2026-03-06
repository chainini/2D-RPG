using UnityEngine;

public class ShadyBattleState : EnemyState
{
    Enemy_Shady enemy;
    Transform player;
    int moveDir;

    private float defaultSpeed;
    public ShadyBattleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Shady enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        defaultSpeed = enemy.moveSpeed;
        enemy.moveSpeed = enemy.battleMoveSpeed;


        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.moveSpeed = defaultSpeed;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                stateMachine.ChangeState(enemy.dieState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.position, enemy.transform.position) > 5)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (enemy.transform.position.x > player.position.x)
            moveDir = -1;
        else if (enemy.transform.position.x < player.position.x)
            moveDir = 1;

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
