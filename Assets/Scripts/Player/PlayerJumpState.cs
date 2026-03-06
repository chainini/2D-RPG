using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
        if (player.isWallDetected() && inputX != 0)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        if (inputX != 0)
            player.SetVelocity(player.moveSpeed * 0.8f * inputX, rb.velocity.y);
    }
}
