using UnityEngine;

public class ArcherDeadState : EnemyState
{
    Enemy_Archer enemy;
    public ArcherDeadState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Archer enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);

        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .2f;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 10);
        }
    }
}
