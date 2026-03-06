using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
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

        if (Input.GetKeyUp(KeyCode.R) && player.skill.blockHole_Skill.unlockBlackHole)
        {
            if (player.skill.blockHole_Skill.coolDownTimer > 0)
            {
                player.playerFX.CreatePopUpText("Cooldonw");
                return;
            }

            stateMachine.ChangeState(player.blackHoleState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !HasNoSword() && player.skill.Sword_Skill.unlockSword)
        {
            stateMachine.ChangeState(player.aimSwordState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.F) && SkillManager.instance.parry_Skill.parryUnlocked)
        {
            stateMachine.ChangeState(player.counterAttackState);
        }

        if (!player.isGrounded())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.primaryAttack);
        }
    }

    private bool HasNoSword()
    {
        if (!player.sword)
            return false;

        player.sword.GetComponent<SwordSkillControl>().ReturnSword();
        return true;
    }
}
