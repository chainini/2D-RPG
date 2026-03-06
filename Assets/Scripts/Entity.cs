using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision info")]
    public Transform attackCheck;
    public float attackRadius = 1.2f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 1;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = .8f;


    [Header("KnockBack info")]
    [SerializeField] protected Vector2 knockBackPower = new Vector2(7,12);
    [SerializeField] protected Vector2 knockBackOffset = new Vector2(.5f,2);
    [SerializeField] protected float knockBackDuration = .07f;
    protected bool isKnock;
    public int knockBackDir { get; private set; }

    #region Component
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public System.Action onFilped;

    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();


        sr = GetComponentInChildren<SpriteRenderer>();

        stats = GetComponent<CharacterStats>();
    }
    protected virtual void Update()
    {

    }

    /// <summary>
    /// 减速Entity
    /// </summary>
    /// <param name="_percentage">减多少</param>
    /// <param name="_slowDration">持续时间</param>
    public virtual void SlowEntityBy(float _percentage, float _slowDration)
    {

    }

    /// <summary>
    /// 恢复默认速度
    /// </summary>
    protected virtual void ReturnDufaultSpeed()
    {
        anim.speed = 1;
    }

    /// <summary>
    /// 设置击退向量
    /// </summary>
    /// <param name="_knockBackPower"></param>
    public void SetupKnockBackPower(Vector2 _knockBackPower) => knockBackPower = _knockBackPower;

    protected virtual IEnumerator HitKnockBack()
    {
        isKnock = true;

        float xOffset = Random.Range(knockBackOffset.x, knockBackOffset.y);

        rb.velocity = new Vector2((knockBackPower.x+xOffset) * knockBackDir, knockBackPower.y);
        yield return new WaitForSeconds(knockBackDuration);
        isKnock = false;

        SetupZeroKnockBackPower();
    }

    /// <summary>
    /// 击退效果 调用了Entity里的协程 HitKnockBack
    /// </summary>
    public virtual void DamageImpact() => StartCoroutine("HitKnockBack");

    public virtual void SetupKnockBackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)
            knockBackDir = -1;
        else if (_damageDirection.position.x < transform.position.x)
            knockBackDir = 1;
    }

    /// <summary>
    /// 设置击退向量为0
    /// </summary>
    protected virtual void SetupZeroKnockBackPower()
    {

    }

    #region Collision
    /// <summary>
    /// 射线检测 是否在地面
    /// </summary>
    /// <returns></returns>
    public bool isGrounded() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    /// <summary>
    /// 射线检测 Entity正面是否靠着墙
    /// </summary>
    /// <returns></returns>
    public bool isWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        //在场景显示检测地面的范围
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        //在场景显示检测墙的范围
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance*facingDir, wallCheck.position.y));
        //在场景显示检测攻击的范围
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
    #endregion
    #region Velocity
    /// <summary>
    /// 设置Entity的Velocity 如果isKnock则设置失败
    /// </summary>
    /// <param name="_VelocityX">X</param>
    /// <param name="_VelocityY"></param>
    public void SetVelocity(float _VelocityX, float _VelocityY)
    {
        if (isKnock)
            return;
        rb.velocity = new Vector2(_VelocityX, _VelocityY);
        FilpController(_VelocityX);
    }
    /// <summary>
    /// 设置Entity的Velocity为0 如果isKnock则设置失败
    /// </summary>
    public void ZeroVelocity()
    {
        if (isKnock)
            return;
        rb.velocity = new Vector2(0, 0);
    }
    #endregion
    #region Filp
    /// <summary>
    /// 翻转Entity
    /// </summary>
    public void Filp()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFilped != null)
            onFilped();
    }
    /// <summary>
    /// 翻转控制器
    /// </summary>
    /// <param name="_x"></param>
    protected void FilpController(float _x)
    {
        if (_x > 0 && !facingRight)
            Filp();
        if (_x < 0 && facingRight)
            Filp();
    }

    /// <summary>
    /// 设置默认的正方向  因为有些角色的默认方向不一样
    /// </summary>
    /// <param name="_direction"></param>
    public virtual void SetupDefaultFacingDir(int _direction)
    {
        facingDir = _direction;

        if (facingDir == -1)
            facingRight = false;
    }
    #endregion

    /// <summary>
    /// Entity 死亡
    /// </summary>
    public virtual void Die()
    {
        
    }
}
