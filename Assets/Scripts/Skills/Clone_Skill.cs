using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{

    [Header("Clone info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuartion;
    [Space]

    [Header("Clone Attack")]
    [SerializeField] private UI_SkillTreeSlot unlockCloneAttackButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggresive Clone")]
    [SerializeField] private UI_SkillTreeSlot unlockAggresiveCloneButton;
    [SerializeField] private float aggresiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }

    [Header("Multiple Clone")]
    [SerializeField] private UI_SkillTreeSlot unlockMultiCloneAttackButton;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicate;
    [SerializeField] private float chanceToDuplicate;
    [Header("Crystal instead of Clone")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalInsteadOfCloneButton;
    public bool crystalInsteadClone;


    protected override void Start()
    {
        base.Start();

        unlockCloneAttackButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        unlockAggresiveCloneButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveCloneAttack);
        unlockMultiCloneAttackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiCloneAttack);
        unlockCrystalInsteadOfCloneButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadOfClone);
    }

    public override void CheckUnlock()
    {

        UnlockCloneAttack();
        UnlockAggresiveCloneAttack();
        UnlockMultiCloneAttack();
        UnlockCrystalInsteadOfClone();
    }

    #region Unlock  Skill
    private void UnlockCloneAttack()
    {
        if (unlockCloneAttackButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }
    private void UnlockAggresiveCloneAttack()
    {
        if (unlockAggresiveCloneButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggresiveCloneAttackMultiplier;
        }
    }
    private void UnlockMultiCloneAttack()
    {
        if (unlockMultiCloneAttackButton.unlocked)
        {
            canDuplicate = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }
    private void UnlockCrystalInsteadOfClone()
    {
        if (unlockCrystalInsteadOfCloneButton.unlocked)
        {
            crystalInsteadClone = true;
        }
    }


    #endregion

    /// <summary>
    /// 创建分身
    /// </summary>
    /// <param name="_newPosition"></param>
    /// <param name="offSet"></param>
    public void CreateClone(Vector3 _newPosition, Vector3 offSet)
    {
        if (crystalInsteadClone)
        {
            SkillManager.instance.crystal_Skill.CreateCrystal();
            return;
        }

        GameObject clone = Instantiate(clonePrefab);

        clone.GetComponent<CloneSkillControl>().SetupClone(_newPosition, cloneDuartion, canAttack, offSet,
            FindCloseEnemy(clone.transform), canDuplicate, chanceToDuplicate, player, attackMultiplier);
    }

    /// <summary>
    /// 在目标附近延迟创建分身
    /// </summary>
    /// <param name="_enemyTransform">目标</param>
    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(CloneDelayCoroutine(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    private IEnumerator CloneDelayCoroutine(Transform _enemyTransform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.3f);
        CreateClone(_enemyTransform.position, _offset);
    }
}
