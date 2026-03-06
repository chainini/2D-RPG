using UnityEngine;
using UnityEngine.UI;

public class BlockHole_Skill : Skill
{
    [SerializeField] private UI_SkillTreeSlot unlockBlackHoleButton;
    public bool unlockBlackHole { get; private set; }
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float blackHoleDuation;
    [Space]
    [SerializeField] private int amountOfAttack;
    [SerializeField] private float attackCoolDown;

    private BlackHole_Skill_Controller blackHoleControl;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        blackHoleControl = newBlackHole.GetComponent<BlackHole_Skill_Controller>();
        blackHoleControl.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttack, attackCoolDown, blackHoleDuation, player);

        AudioManager.instance.PlaySFX(3, player.transform);
        AudioManager.instance.PlaySFX(6, player.transform);
    }

    protected override void Start()
    {
        base.Start();

        unlockBlackHoleButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void CheckUnlock()
    {

        UnlockBlackHole();
    }

    private void UnlockBlackHole()
    {
        if (unlockBlackHoleButton.unlocked)
            unlockBlackHole = true;
    }

    /// <summary>
    /// ÍË³öºÚ¶´
    /// </summary>
    /// <returns></returns>
    public bool ExitBlackHole()
    {
        if (!blackHoleControl)
            return false;

        if (blackHoleControl.playerCanExitBlackHole)
        {
            blackHoleControl = null;
            return true;
        }

        return false;
    }

    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }
}
