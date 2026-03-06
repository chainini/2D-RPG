using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    Player player;
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float crystalDuration;

    private bool canExplode;
    private float growSpeed;
    private bool canGrow;
    private bool canMoveToEnemy;
    private float moveSpeed;

    private Transform closeEnemy;
    [SerializeField] private LayerMask whatIsEnemy;

    /// <summary>
    /// 初始化水晶
    /// </summary>
    /// <param name="_crystalDuration">持续时间</param>
    /// <param name="_growSpeed">增长速度</param>
    /// <param name="_canExplode">是否可爆炸</param>
    /// <param name="_canMoveToEnemy">是否可向敌人移动</param>
    /// <param name="_moveSpeed">移动速度</param>
    /// <param name="_closeEnemy">最近的敌人</param>
    /// <param name="_player">玩家</param>
    public void SetupCrystal(float _crystalDuration, float _growSpeed, bool _canExplode, bool _canMoveToEnemy, float _moveSpeed, Transform _closeEnemy, Player _player)
    {
        player = _player;
        crystalDuration = _crystalDuration;
        growSpeed = _growSpeed;
        canExplode = _canExplode;
        canMoveToEnemy = _canMoveToEnemy;
        moveSpeed = _moveSpeed;
        closeEnemy = _closeEnemy;
    }

    private void Update()
    {
        crystalDuration -= Time.deltaTime;
        if (crystalDuration < 0)
        {
            CrystalFinish();
        }

        if (canMoveToEnemy && closeEnemy != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, closeEnemy.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closeEnemy.position) < 1)
            {
                canMoveToEnemy = false;
                CrystalFinish();
            }
        }

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 在范围内选择随机的敌人
    /// </summary>
    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blockHole_Skill.GetBlackHoleRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        if (colliders.Length > 0)
        {
            closeEnemy = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }

    /// <summary>
    /// 水晶爆炸
    /// </summary>
    public void CrystalFinish()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetBool("Explode", canExplode);
        }
        else
        {
            SelfDestory();
        }
    }


    /// <summary>
    /// 水晶爆炸后的伤害判断  动画事件
    /// </summary>
    private void AnimationExplotEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockBackDir(transform);
                player.stats.DoMagicDamage(hit.GetComponent<CharacterStats>());
                //hit.GetComponent<Enemy>().DamageEffect();

                ItemData_Equipment equipAmulet = InventoryManager.Instance.GetEquipment(EquipemntType.Amuler);

                if (equipAmulet != null)
                    equipAmulet.Effect(hit.transform);

            }
        }
    }

    /// <summary>
    /// 销毁自身
    /// </summary>
    public void SelfDestory() => Destroy(gameObject);
}
