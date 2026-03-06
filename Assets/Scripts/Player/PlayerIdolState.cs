public class PlayerIdolState : PlayerGroundState
{
    public PlayerIdolState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.ZeroVelocity();
    }
    public override void Update()
    {
        base.Update();
        if (inputX == player.facingDir && player.isWallDetected())
        {
            return;
        }

        if (inputX != 0 && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();

    }

}
