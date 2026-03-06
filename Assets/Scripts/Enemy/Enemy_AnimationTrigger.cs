using UnityEngine;

/// <summary>
/// 动画事件
/// </summary>
public class Enemy_AnimationTrigger : MonoBehaviour
{
    private Enemy enemy => GetComponentInParent<Enemy>();

    /// <summary>
    /// 动画结束 动画事件
    /// </summary>
    private void AnimationTrigger()
    {
        enemy.AttackAnimationFinishTrigger();
    }
    /// <summary>
    /// 攻击判断 动画事件
    /// </summary>
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(enemy.attackCheck.position.x, enemy.attackCheck.position.y), enemy.attackRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }
    }

    /// <summary>
    /// 敌人特殊攻击 动画事件
    /// </summary>
    private void SpecialAttackTrigger()
    {
        enemy.AnimationSpecialAttackTrigger();
    }
    /// <summary>
    /// 敌人特殊攻击结束 动画事件
    /// </summary>
    private void SpecialAttackTriggerOver()
    {
        enemy.AnimationSpecialAttackTriggerOver();
    }

    /// <summary>
    /// 打开反击窗口 动画事件
    /// </summary>
    private void OpenCounterAttackWindow() => enemy.OpenConterAttackWindow();
    /// <summary>
    /// 关闭反击窗口 动画事件
    /// </summary>
    private void CloseCounterAttackWindow() => enemy.CloseConterAttackWindow();
}
