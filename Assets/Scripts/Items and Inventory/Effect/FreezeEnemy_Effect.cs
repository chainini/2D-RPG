using UnityEngine;

[CreateAssetMenu(fileName = "SwordSkillDamage Effect", menuName = "Data/Item Effect/SwordSkillDamage")]
public class FreezeEnemy_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _enemyPos)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats.currentHp > playerStats.GetMaxHealthValue() * .1f)
            return;


        if (!InventoryManager.Instance.CanUseArmor())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemyPos.position, 2);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
