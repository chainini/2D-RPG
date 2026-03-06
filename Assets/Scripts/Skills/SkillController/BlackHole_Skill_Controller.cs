using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    Player player;
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> hotKeyList;

    private bool canCreateHotKey = true;
    private bool canGrow = true;
    private float maxSize;
    private float growSpeed;
    private bool canShrink;
    private float shrinkSpeed;
    private float blackHoleTimer;

    private bool canClearColor = true;
    /// <summary>
    /// 有Bug 当在blackhole中 按下hotkey不释放cloneAttack  在黑洞没消失前在按R  黑洞不会销毁
    /// </summary>
    //private bool blackHoleCanDestory;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createHotKeyList = new List<GameObject>();

    private float amountOfAttack = 4;
    private float attackCoolDown = .3f;
    private float attackCoolTimer;
    private bool canAttack;

    public bool playerCanExitBlackHole { get; private set; }

    /// <summary>
    /// 初始化黑洞
    /// </summary>
    /// <param name="_maxSize">最大Size</param>
    /// <param name="_growSpeed">增长速度</param>
    /// <param name="_shrinkSpeed">消失速度</param>
    /// <param name="_amountOfAttack">攻击次数</param>
    /// <param name="_attackCoolDown">攻击间隔</param>
    /// <param name="_blackHoleDuation">持续事件</param>
    /// <param name="_player">玩家</param>
    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttack, float _attackCoolDown, float _blackHoleDuation, Player _player)
    {
        player = _player;
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttack = _amountOfAttack;
        attackCoolDown = _attackCoolDown;
        blackHoleTimer = _blackHoleDuation;
        if (SkillManager.instance.clone_Skill.crystalInsteadClone)
            canClearColor = false;
    }

    private void Update()
    {
        attackCoolTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;

        if (blackHoleTimer < 0)
        {
            blackHoleTimer = Mathf.Infinity;

            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                ExitBlackHole();
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            ReleaseCloneAttack();

        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canShrink && !canAttack)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// 释放分身攻击
    /// </summary>
    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
            return;

        DestroyHotKey();
        canAttack = true;
        canCreateHotKey = false;
        if (canClearColor)
        {
            canClearColor = false;
            player.playerFX.MakeTransprent(true);
        }
    }

    /// <summary>
    /// 分身攻击的逻辑
    /// </summary>
    private void CloneAttackLogic()
    {
        if (attackCoolTimer <= 0 && canAttack && amountOfAttack > 0)
        {
            attackCoolTimer = attackCoolDown;

            float offsetX;
            if (UnityEngine.Random.Range(0, 100) > 50)
                offsetX = 1.2f;
            else
                offsetX = -1.2f;
            int randomIndex = UnityEngine.Random.Range(0, targets.Count);

            if (SkillManager.instance.clone_Skill.crystalInsteadClone)
            {
                SkillManager.instance.crystal_Skill.CreateCrystal();
                SkillManager.instance.crystal_Skill.CurrentCrystalChooseRandomEnemy();
            }
            else
                SkillManager.instance.clone_Skill.CreateClone(targets[randomIndex].position, new Vector3(offsetX, 0));

            amountOfAttack--;
            if (amountOfAttack <= 0)
            {
                //Delay Exit BlackHole 
                Invoke("ExitBlackHole", 1f);

            }
        }
    }

    /// <summary>
    /// 黑洞结束
    /// </summary>
    private void ExitBlackHole()
    {
        DestroyHotKey();
        playerCanExitBlackHole = true;
        canAttack = false;
        canShrink = true;
    }

    //释放黑洞冻住的敌人
    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);

    //将黑洞范围内的敌人 冻住
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);

        }
    }

    /// <summary>
    /// 将黑洞范围内的敌人加入列表
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemyToList(Transform enemy) => targets.Add(enemy);

    /// <summary>
    /// 销毁热键
    /// </summary>
    private void DestroyHotKey()
    {
        if (createHotKeyList.Count <= 0)
            return;
        foreach (var hotkey in createHotKeyList)
        {
            Destroy(hotkey.gameObject);
        }
    }
    /// <summary>
    /// 创建热键
    /// </summary>
    /// <param name="collision"></param>
    private void CreateHotKey(Collider2D collision)
    {
        if (hotKeyList.Count <= 0)
        {
            Debug.LogWarning("none HotKey!!!");
            return;
        }

        if (!canCreateHotKey)
            return;

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.GetComponent<Enemy>().transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        BlackHole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<BlackHole_HotKey_Controller>();

        createHotKeyList.Add(newHotKey);

        KeyCode hotKey = hotKeyList[UnityEngine.Random.Range(0, hotKeyList.Count)];
        hotKeyList.Remove(hotKey);

        newHotKeyScript.SetupHotKey(hotKey, collision.GetComponent<Enemy>().transform, this);
    }


}
