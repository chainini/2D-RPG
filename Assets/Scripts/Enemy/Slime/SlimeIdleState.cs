public class SlimeIdleState : SlimeGroundState
{
    public SlimeIdleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Slime enemy) : base(stateMachine, enemyBase, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }
}
