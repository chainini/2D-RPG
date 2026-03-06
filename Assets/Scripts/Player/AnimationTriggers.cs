using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    /// <summary>
    /// 动画结束
    /// </summary>
    private void AnimationFinish()
    {
        player.AnimationTrigger();
    }
    /// <summary>
    /// 攻击判定  动画事件
    /// </summary>
    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(2, null);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(player.attackCheck.position.x, player.attackCheck.position.y), player.attackRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats targetStats = hit.GetComponent<EnemyStats>();

                if (targetStats != null)
                {
                    player.stats.DoDamage(targetStats);
                }

                ItemData_Equipment weaponData = InventoryManager.Instance.GetEquipment(EquipemntType.Weapon);
                if (weaponData != null)
                    weaponData.Effect(targetStats.transform);
            }
        }
    }
    /// <summary>
    /// 投掷剑 动画事件
    /// </summary>
    private void ThrowSword()
    {
        SkillManager.instance.Sword_Skill.CreateSword();
    }

    /// <summary>
    /// 脚步声  动画事件
    /// </summary>
    private void MoveStepSFX()
    {
        AudioManager.instance.PlaySFX(14, null);
    }
}
