public class ArcherStunedState : EnemyState
{
    Enemy_Archer enemy;
    public ArcherStunedState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Archer enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.entityFX.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunedDuration;

        //与slime 一样这一行被覆盖  Velocity的值别的什么地方 覆盖了  值为20左右 为什么？
        //rb.velocity = new Vector2(-enemy.facingDir * enemy.stunedDirection.x, enemy.stunedDirection.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.entityFX.Invoke("CancelColorChange", 0);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
