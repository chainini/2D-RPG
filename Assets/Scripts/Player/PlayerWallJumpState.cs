public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = .4f;
        player.SetVelocity(5 * -player.facingDir, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (player.isGrounded())
        {
            stateMachine.ChangeState(player.idolState);
        }
        if (inputX != 0)
            player.SetVelocity(player.moveSpeed * 0.8f * inputX, rb.velocity.y);
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.airState);
        }

    }
}
