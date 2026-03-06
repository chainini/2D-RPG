using UnityEngine;

public class DeathBrinerIdleState : EnemyState
{
    Enemy_DeathBriner_Boss enemy;
    Transform player;
    public DeathBrinerIdleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_DeathBriner_Boss enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(Vector2.Distance(player.position, enemy.transform.position) < 7)
            enemy.bossFight = true;
        
        if(Input.GetKeyDown(KeyCode.P))
        {
            stateMachine.ChangeState(enemy.teleportState);
        }

        if(stateTimer<0 && enemy.bossFight)
            stateMachine.ChangeState(enemy.battleState);
    }
}
