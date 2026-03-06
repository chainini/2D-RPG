using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    public int comboCounter;

    private float lastAttackTime;
    private float comboWindows = 2;

    public PlayerPrimaryAttack(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //AudioManager.instance.PlaySFX(2);// attack sound effect

        inputX = 0;//we need this to fix bug on attack direction

        if (comboCounter > 2 || Time.time >= lastAttackTime + comboWindows)
            comboCounter = 0;

        player.anim.speed = .9f;

        float attckDir = player.facingDir;
        if (inputX != 0)
            attckDir = inputX;

        player.anim.SetInteger("ComboCounter", comboCounter);
        player.SetVelocity(player.attackMovement[comboCounter].x * attckDir, player.attackMovement[comboCounter].y);

        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.anim.speed = 1;

        player.StartCoroutine("BusyFor", 0.15f);

        comboCounter++;
        lastAttackTime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            player.ZeroVelocity();
        if (animTrigger)
            stateMachine.ChangeState(player.idolState);
    }
}
