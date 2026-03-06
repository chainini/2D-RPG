using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] private float evasionAmount;
    public bool unlockDodge;

    [Header("Dodge mirage")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeMirageButton;
    public bool unlockDodgeMirage;


    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockDodgeMirageButton.GetComponent<Button>().onClick.AddListener(UnlockDodgeMirage);
    }

    public override void CheckUnlock()
    {

        UnlockDodge();
        UnlockDodgeMirage();
    }

    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !unlockDodge)
        {
            unlockDodge = true;
            player.stats.evasion.AddModifier(evasionAmount);
            InventoryManager.Instance.UpdateStatUI();
        }
    }
    private void UnlockDodgeMirage()
    {
        if (unlockDodgeMirageButton.unlocked)
            unlockDodgeMirage = true;
    }

    public void CreateMirageOnDodge()
    {
        if (unlockDodgeMirage)
            SkillManager.instance.clone_Skill.CreateClone(player.transform.position, Vector3.zero);
    }
}
