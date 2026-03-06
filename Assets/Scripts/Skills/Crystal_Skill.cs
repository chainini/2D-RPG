using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;
    private Vector3 playerPriorPos;

    [Header("Crystal simple")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Crystal mirage")]
    [SerializeField] private UI_SkillTreeSlot unlockCloneInsteadButton;
    [SerializeField] private bool cloneInsteadCrystal;

    [Header("Crystal Explode info")]
    [SerializeField] private UI_SkillTreeSlot unlockExplodeButton;
    [SerializeField] private bool canExplode;

    [Header("Crystal Ability info")]
    [SerializeField] private float crystalDuation;
    [SerializeField] private float gorwSpeed;

    [Header("Moving crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi Stack Crystal info")]
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackCrystalButton;
    [SerializeField] private int amountOfMultiCrystal;
    [SerializeField] private float multiStackCoolDown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private bool canUseMultiCrystal;
    [SerializeField] private List<GameObject> multiCrystalList = new List<GameObject>();


    protected override void Start()
    {
        base.Start();

        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplodeButton.GetComponent<Button>().onClick.AddListener(UnlockExplodeCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultiStackCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
    }

    public override void CheckUnlock()
    {

        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockExplodeCrystal();
        UnlockMovingCrystal();
        UnlockMultiStack();
    }

    #region Unlock Skill 
    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
            crystalUnlocked = true;
    }
    private void UnlockCrystalMirage()
    {
        if (unlockCloneInsteadButton.unlocked)
            cloneInsteadCrystal = true;
    }
    private void UnlockExplodeCrystal()
    {
        if (unlockExplodeButton.unlocked)
            canExplode = true;
    }
    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
            canMoveToEnemy = true;
    }
    private void UnlockMultiStack()
    {
        if (unlockMultiStackCrystalButton.unlocked)
            canUseMultiCrystal = true;
    }
    #endregion


    public override void UseSkill()
    {
        base.UseSkill();

        if (CanMulti())
            return;

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
                return;

            playerPriorPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPriorPos;

            if (cloneInsteadCrystal)
            {
                SkillManager.instance.clone_Skill.CreateClone(currentCrystal.transform.position, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>().CrystalFinish();
            }


        }

    }

    /// <summary>
    /// 创建水晶
    /// </summary>
    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller crystal_Script = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        crystal_Script.SetupCrystal(crystalDuation, gorwSpeed, canExplode, canMoveToEnemy, moveSpeed, FindCloseEnemy(currentCrystal.transform), player);
    }

    /// <summary>
    /// 选择随机敌人
    /// </summary>
    public void CurrentCrystalChooseRandomEnemy() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    /// <summary>
    /// 是否能多个水晶技能
    /// </summary>
    /// <returns></returns>
    private bool CanMulti()
    {
        if (canUseMultiCrystal)
        {
            if (multiCrystalList.Count > 0)
            {
                if (amountOfMultiCrystal == multiCrystalList.Count)
                    Invoke("ResetAbility", useTimeWindow);

                coolDown = 0;
                GameObject crystalToSpawn = multiCrystalList[multiCrystalList.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                multiCrystalList.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(crystalDuation, gorwSpeed, canExplode,
                    canMoveToEnemy, moveSpeed, FindCloseEnemy(newCrystal.transform), player);

                if (multiCrystalList.Count <= 0)
                {
                    coolDown = multiStackCoolDown;
                    GerenateMultiCrystal();
                }
                return true;
            }

        }

        return false;
    }

    /// <summary>
    /// 生成多个水晶
    /// </summary>
    private void GerenateMultiCrystal()
    {
        int addAmountOfCrystal = amountOfMultiCrystal - multiCrystalList.Count;
        for (int i = 0; i < addAmountOfCrystal; i++)
        {
            multiCrystalList.Add(crystalPrefab);
        }
    }
    /// <summary>
    /// 超出时间 后重置水晶数量
    /// </summary>
    private void ResetAbility()
    {
        if (coolDownTimer > 0)
            return;
        coolDownTimer = multiStackCoolDown;
        GerenateMultiCrystal();
    }
}
