public class Enemy_DeathBrinerTrigger : Enemy_AnimationTrigger
{
    private Enemy_DeathBriner_Boss enemyDeathBriner => GetComponentInParent<Enemy_DeathBriner_Boss>();

    private void Relocate() => enemyDeathBriner.FindPosition();

    private void MakeInvisible() => enemyDeathBriner.entityFX.MakeTransprent(true);
    private void MakeVisible() => enemyDeathBriner.entityFX.MakeTransprent(false);
}
