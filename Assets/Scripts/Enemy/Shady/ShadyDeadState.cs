using UnityEngine;

public class ShadyDeadState : EnemyState
{
    Enemy_Shady enemy;
    public ShadyDeadState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Shady enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        enemy.ZeroVelocity();
    }
}
