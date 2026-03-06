using UnityEngine;

public class DeathBrinerSpellCastState : EnemyState
{
    Enemy_DeathBriner_Boss enemy;

    private int amountOfSpell; 
    private float spellTimer;
    public DeathBrinerSpellCastState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_DeathBriner_Boss enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        amountOfSpell = enemy.amountOfSpell;
        spellTimer = .5f;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lasttimeCast = Time.time;
    }

    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;

        if (CanCast())
            enemy.CastSpell();
        if(amountOfSpell<=0)
             stateMachine.ChangeState(enemy.teleportState);

    }

    private bool CanCast()
    {
        if (amountOfSpell > 0 && spellTimer<0)
        {
            amountOfSpell--;
            spellTimer = enemy.spellCoolDown;
            return true;
        }

        return false;
    }
}
