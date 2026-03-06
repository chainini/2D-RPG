public class PlayerAirState : PlayerState
{
    public PlayerAirState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
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
        if (player.isGrounded())
        {
            stateMachine.ChangeState(player.idolState);
        }

        if (player.isWallDetected() && inputX != 0)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if (inputX != 0)
            player.SetVelocity(player.moveSpeed * 0.8f * inputX, rb.velocity.y);
    }
}
