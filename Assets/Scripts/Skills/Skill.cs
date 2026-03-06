using UnityEngine;

public class Skill : MonoBehaviour
{
    public float coolDown;
    public float coolDownTimer;

    protected Player player;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;

        Invoke("CheckUnlock", .5f);
        //CheckUnlock();
    }

    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// 检查技能受否解锁  如果解锁则激活技能
    /// LoadSill 是在一个个遍历 不是一次完成 导致无法unlock技能
    /// 解决办法 延迟唤醒CheckUnlock
    /// </summary>
    public virtual void CheckUnlock()
    {

    }
    /// <summary>
    /// 是否能使用技能 能就使用 不能返回false
    /// </summary>
    /// <returns></returns>
    public virtual bool CanUseSkill()
    {
        if (coolDownTimer < 0)
        {
            UseSkill();
            coolDownTimer = coolDown;
            return true;
        }

        player.playerFX.CreatePopUpText("Cooldown");
        return false;
    }
    /// <summary>
    /// 使用技能
    /// </summary>
    public virtual void UseSkill()
    {

    }

    /// <summary>
    /// 以圆的范围找到最近的敌人
    /// </summary>
    /// <param name="_checkPos">查找点</param>
    /// <returns></returns>
    public virtual Transform FindCloseEnemy(Transform _checkPos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkPos.position, 25);
        float closeDistance = Mathf.Infinity;

        Transform closeEnemy = null;

        foreach (var hit in colliders)
        {
            if ((hit.GetComponent<Enemy>() != null))
            {
                float distanceFormEnemy = Vector2.Distance(_checkPos.position, hit.transform.position);

                if (distanceFormEnemy < closeDistance)
                {
                    closeDistance = distanceFormEnemy;
                    closeEnemy = hit.transform;
                }
            }
        }

        return closeEnemy;
    }
}
