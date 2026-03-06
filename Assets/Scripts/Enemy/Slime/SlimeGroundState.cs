using UnityEngine;

public class SlimeGroundState : EnemyState
{
    protected Enemy_Slime enemy;
    protected Transform player;

    public SlimeGroundState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Slime enemy) : base(stateMachine, enemyBase, animBoolName)
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
        if (enemy.IsPlayerDetected() || Vector2.Distance(player.position, enemy.transform.position) < enemy.agroDistance)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
