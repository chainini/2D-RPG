using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
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

        if (!player.isWallDetected() && player.isGrounded())
            stateMachine.ChangeState(player.idolState);
        if (!player.isWallDetected() && !player.isGrounded())
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }


        if (inputY < 0)
            player.SetVelocity(0, rb.velocity.y);
        else
            player.SetVelocity(0, rb.velocity.y * 0.7f);
        if (inputX != 0 && inputX != player.facingDir)
            stateMachine.ChangeState(player.idolState);
        if (player.isGrounded())
            stateMachine.ChangeState(player.idolState);
    }
}
