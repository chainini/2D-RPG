using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canClone;
    public PlayerCounterAttackState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        canClone = true;
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.ZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(player.attackCheck.position.x, player.attackCheck.position.y), player.attackRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Arrow_Controller>() != null)
            {
                hit.GetComponent<Arrow_Controller>().FilpArrow();
                SuccessfulCounterAttack();
            }


            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStuned())
                    {
                        SuccessfulCounterAttack();

                        player.skill.parry_Skill.UseSkill();//goint to use to restore health on parry

                        player.stats.DoDamage(hit.GetComponent<CharacterStats>());
                        if (canClone)
                        {
                            canClone = false;
                            //player.skill.clone_Skill.CreateCloneWithDelay(hit.transform);
                            player.skill.parry_Skill.MakeMirageOnParry(hit.transform);
                        }
                    }
                }
        }

        if (stateTimer < 0 || animTrigger)
        {
            stateMachine.ChangeState(player.idolState);
        }

    }

    private void SuccessfulCounterAttack()
    {
        stateTimer = 10;//any valye is bigger then 1
        player.anim.SetBool("SuccessfulCounterAttack", true);
    }
}
