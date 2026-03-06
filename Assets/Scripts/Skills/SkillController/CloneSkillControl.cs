using UnityEngine;

public class CloneSkillControl : MonoBehaviour
{
    Player player;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius;
    private Animator anim;


    [SerializeField] private float colorDisapperSpeed;
    public SpriteRenderer sr;
    private float cloneTimer;

    private Transform closeEnemy;

    private float attackMultiplier;
    private bool canDuplicate;
    private bool canAttack;
    private float chanceToDuplicate;
    private int facingDir = 1;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorDisapperSpeed));
        }

        if (sr.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// 初始化分身
    /// </summary>
    /// <param name="_newTransform">生成的位置</param>
    /// <param name="cloneDuration">持续时间</param>
    /// <param name="_canAttack">是否可以攻击</param>
    /// <param name="offSet">偏移量</param>
    /// <param name="_closeEnemy">最近的敌人</param>
    /// <param name="_canDuplicate">是否可以复制</param>
    /// <param name="_chanceToDuplicate">可以复制的概率</param>
    /// <param name="_player">玩家</param>
    /// <param name="_attackMultiplier">伤害</param>
    public void SetupClone(Vector3 _newTransform, float cloneDuration, bool _canAttack, Vector3 offSet, Transform _closeEnemy, bool _canDuplicate, float _chanceToDuplicate, Player _player, float _attackMultiplier)
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));

        player = _player;

        transform.position = _newTransform + offSet;
        cloneTimer = cloneDuration;
        closeEnemy = SkillManager.instance.clone_Skill.FindCloseEnemy(transform);
        canDuplicate = _canDuplicate;
        canAttack = _canAttack;
        chanceToDuplicate = _chanceToDuplicate;
        attackMultiplier = _attackMultiplier;

        FaceTarget();
    }

    /// <summary>
    /// 分身结束
    /// </summary>
    private void AnimationFinish()
    {
        cloneTimer = -.1f;
    }

    /// <summary>
    /// 分身攻击判断 动画事件
    /// </summary>
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(attackCheck.position.x, attackCheck.position.y), attackRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //hit.GetComponent<Enemy>().DamageEffect();
                //player.stats.DoDamage(hit.GetComponent<CharacterStats>());

                hit.GetComponent<Entity>().SetupKnockBackDir(transform);

                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, attackMultiplier);
                if (player.skill.clone_Skill.canApplyOnHitEffect)
                {
                    ItemData_Equipment weaponData = InventoryManager.Instance.GetEquipment(EquipemntType.Weapon);
                    if (weaponData != null)
                        weaponData.Effect(hit.transform);
                }


                if (canDuplicate)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone_Skill.CreateClone(hit.transform.position, new Vector3(.7f * facingDir, 0));
                    }
                }
            }
        }
    }

    /// <summary>
    /// 面向最近的敌人
    /// </summary>
    private void FaceTarget()
    {
        if (closeEnemy == null)
            return;

        if (transform.position.x > closeEnemy.position.x)
        {
            facingDir *= -1;
            transform.Rotate(0, 180, 0);
        }
    }
}
