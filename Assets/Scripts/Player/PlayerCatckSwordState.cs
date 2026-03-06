using UnityEngine;

public class PlayerCatckSwordState : PlayerState
{
    public PlayerCatckSwordState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    private Transform sword;

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;

        player.playerFX.PlayDustFX();
        player.playerFX.ScreenShake(new Vector3(player.playerFX.shakeSwordImpact.x,player.playerFX.shakeSwordImpact.y));

        if (player.transform.position.x > sword.position.x && player.facingDir == 1)
            player.Filp();
        else if (player.transform.position.x < sword.position.x && player.facingDir == -1)
            player.Filp();

        rb.velocity = new Vector2(player.catchSwordForce * -player.facingDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();
        if (animTrigger)
            stateMachine.ChangeState(player.idolState);
    }
}
