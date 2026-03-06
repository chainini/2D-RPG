using UnityEngine;

public class DeathBrinerTeleportState : EnemyState
{
    Enemy_DeathBriner_Boss enemy;
    public DeathBrinerTeleportState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_DeathBriner_Boss enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.stats.MakeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if (animTrigger)
        {
            if (enemy.canDoSpellCast())
                stateMachine.ChangeState(enemy.spellCastState);
            else
                stateMachine.ChangeState(enemy.battleState);
        }
    }
}
