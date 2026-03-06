using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_DeathBriner_Boss : Enemy
{
    public bool bossFight;
    public GameObject fadeScreenPrafab;

    [Header("Teleport detail")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;
    public float chanceToTeleport;
    public float defaultChanceToTeleport=25;

    [Header("Spell cast detail")]
    [SerializeField] private GameObject spellPrefab;
    public int amountOfSpell;
    public float spellCoolDown;
    public float lasttimeCast;
    [SerializeField] private float spellStateCoolDown;

    #region States
    public DeathBrinerBattleState battleState { get; private set; }
    public DeathBrinerAttackState attackState { get; private set; }
    public DeathBrinerIdleState idleState { get; private set; }
    public DeathBrinerDeadState deadState { get; private set; }
    public DeathBrinerSpellCastState spellCastState { get; private set; }
    public DeathBrinerTeleportState teleportState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        idleState = new DeathBrinerIdleState(stateMachine, this, "Idle", this);
        battleState = new DeathBrinerBattleState(stateMachine, this, "Move", this);
        attackState = new DeathBrinerAttackState(stateMachine, this, "Attack", this);
        deadState = new DeathBrinerDeadState(stateMachine, this, "Idle", this);
        spellCastState = new DeathBrinerSpellCastState(stateMachine, this, "SpellCast", this);
        teleportState = new DeathBrinerTeleportState(stateMachine, this, "Teleport", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);


        Invoke("DieAndReturnMainMenu", .5f);
    }

    private void DieAndReturnMainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));

        if (!GroundBelow() || SomethingIsAround())
        {
            Debug.Log("Looking for new position");
            FindPosition();
        }
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }

    public bool canTeleport()
    {
        if (Random.Range(0, 100) <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }

        return false;
    }

    public bool canDoSpellCast()
    {
        if (Time.time > lasttimeCast + spellStateCoolDown)
        {
            return true;
        }
        return false;
    }

    public void CastSpell()
    {
        Player player = PlayerManager.instance.player;
        float xOffset = 0;

        if (player.rb.velocity.x != 0)
            xOffset = player.facingDir * 2;

        Vector3 spellPosition = new Vector3(player.transform.position.x+xOffset, player.transform.position.y + 1.5f);

        GameObject newSpell = Instantiate(spellPrefab,spellPosition,Quaternion.identity);
        newSpell.GetComponent<DeathBrinerSpell_Controller>().SetupSpell(stats);
    }
}
