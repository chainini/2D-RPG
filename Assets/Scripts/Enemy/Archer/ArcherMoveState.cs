public class ArcherMoveState : ArcherGroundState
{
    public ArcherMoveState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Archer enemy) : base(stateMachine, enemyBase, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

        if (enemy.isWallDetected() || !enemy.isGrounded())
        {
            enemy.Filp();
            stateMachine.ChangeState(enemy.idleState);
        }

    }
}
