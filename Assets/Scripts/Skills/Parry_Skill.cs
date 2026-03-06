using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked;

    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot parryRestoreUnlockButton;
    public bool parryRestoreUnlocked;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercentage;

    [Header("Parrt with mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked;

    public override void UseSkill()
    {
        base.UseSkill();

        if (parryRestoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercentage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        parryRestoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    public override void CheckUnlock()
    {

        UnlockParry();
        UnlockParryRestore();
        UnlockParryWithMirage();
    }
    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;
    }
    private void UnlockParryRestore()
    {
        if (parryRestoreUnlockButton.unlocked)
            parryRestoreUnlocked = true;
    }
    private void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
            parryWithMirageUnlocked = true;
    }

    /// <summary>
    /// 反击后 创建一个分身
    /// </summary>
    /// <param name="_responTransform"></param>
    public void MakeMirageOnParry(Transform _responTransform)
    {
        if (parryWithMirageUnlocked)
            SkillManager.instance.clone_Skill.CreateCloneWithDelay(_responTransform);
    }
}
