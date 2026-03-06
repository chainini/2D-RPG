using System.Collections.Generic;
using UnityEngine;

public class SwordSkillControl : MonoBehaviour
{
    public float returnSpeed;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private Animator anim;
    private Player player;

    private bool canRotate = false;
    private bool isReturning;

    private float freezeTime;

    [Header("Pierce info")]
    private int amountOfPierce;
    private float pierceSpeed;

    [Header("Bounce info")]
    public float bounceSpeed;
    private bool isBouncing;
    private int amountOfBounce;
    private List<Transform> enemyTarget = new List<Transform>();
    private int targetIndex = 0;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float intervalHitTime;
    private float intervalHitDuration;
    private float stopTime;
    private float stopDuration;
    private bool isStop;
    private bool isSpinning;
    private float spinDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        anim = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// 销毁自己
    /// </summary>
    private void DestoryMe()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
            }
        }

        BounceLogic();
        SpinLogic();
    }
    /// <summary>
    /// Spin模式的逻辑
    /// </summary>
    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(transform.position, player.transform.position) >= maxTravelDistance && !isStop)
            {
                StopWhenSpin();
            }
            if (isStop)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                stopTime -= Time.deltaTime;

                if (stopTime < 0)
                {
                    isStop = false;
                    isSpinning = false;
                    isReturning = true;
                }

                intervalHitDuration -= Time.deltaTime;

                if (intervalHitDuration < 0)
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                    intervalHitDuration = intervalHitTime;
                }
            }
        }
    }

    private void StopWhenSpin()
    {
        isStop = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        stopTime = stopDuration;
    }
    /// <summary>
    /// Bounce模式的逻辑
    /// </summary>
    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
                targetIndex++;
                amountOfBounce--;
                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }

        }
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounce)
    {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounce;
    }
    public void SetupPierce(int _amountOfPierce, float _pierceSpeed)
    {
        amountOfPierce = _amountOfPierce;
        pierceSpeed = _pierceSpeed;
    }
    public void SetupSpin(float _maxTravelDistance, float _intervalHitTime, float _stopDuration, bool _isSpinning)
    {
        maxTravelDistance = _maxTravelDistance;
        intervalHitTime = _intervalHitTime;
        stopDuration = _stopDuration;
        isSpinning = _isSpinning;
    }
    public void SetupSword(Vector2 direction, float _gravityScale, Player _player, float _freezeTime, float _returnSpeed)
    {
        rb.velocity = new Vector2(direction.x, direction.y);
        rb.gravityScale = _gravityScale;
        player = _player;
        freezeTime = _freezeTime;
        returnSpeed = _returnSpeed;
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
        if (amountOfPierce <= 0)
            anim.SetBool("Rotation", true);

        Invoke("DestoryMe", 7f);
    }
    /// <summary>
    /// 收回丢出的剑
    /// </summary>
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy newEnemyScript = collision.GetComponent<Enemy>();
            SwordSkillDamage(newEnemyScript);
        }

        SetupTargetForBounce(collision);
        StuckSword(collision);
    }
    /// <summary>
    /// 剑伤害判断
    /// </summary>
    /// <param name="enemy"></param>
    private void SwordSkillDamage(Enemy enemy)
    {
        //enemy.DamageEffect();
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

        player.stats.DoDamage(enemyStats);


        if (player.skill.Sword_Skill.unlockTimeStop)
            enemy.StartCoroutine("FrozenFor", freezeTime);
        if (player.skill.Sword_Skill.unlockVulnurable)
            enemyStats.MakeVulnurableFor(freezeTime);

        ItemData_Equipment equipAmulet = InventoryManager.Instance.GetEquipment(EquipemntType.Amuler);

        if (equipAmulet != null)
            equipAmulet.Effect(enemy.transform);
    }

    /// <summary>
    ///在Bounce模式下 把攻击范围内的敌人加入列表
    /// </summary>
    /// <param name="collision"></param>
    private void SetupTargetForBounce(Collider2D collision)
    {
        if (isBouncing)
        {
            if (collision.GetComponent<Enemy>() != null)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 将丢出的剑停留在碰到的物体上
    /// 然后将剑的物理组件关了
    /// </summary>
    /// <param name="collision"></param>
    private void StuckSword(Collider2D collision)
    {
        if (amountOfPierce > 0 && collision.GetComponent<Enemy>() != null)
        {
            amountOfPierce--;
            return;
        }
        if (isSpinning)
        {
            StopWhenSpin();
            return;
        }
        canRotate = true;
        circleCollider.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponentInChildren<ParticleSystem>().Play();

        if (isBouncing && enemyTarget.Count > 0)
            return;
        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
