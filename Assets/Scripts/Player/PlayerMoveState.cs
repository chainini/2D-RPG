public class PlayerMoveState : PlayerGroundState
{

    public PlayerMoveState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        player.SetVelocity(inputX * player.moveSpeed, rb.velocity.y);

        if (inputX == 0 || player.isWallDetected())
        {
            stateMachine.ChangeState(player.idolState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(14);
    }

}
