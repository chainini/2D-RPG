using UnityEngine;

/// <summary>
/// 管理所有技能  是个单例
/// </summary>
public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Dash_Skill dash_Skill { get; private set; }
    public Clone_Skill clone_Skill { get; private set; }
    public Sword_Skill Sword_Skill { get; private set; }
    public BlockHole_Skill blockHole_Skill { get; private set; }
    public Crystal_Skill crystal_Skill { get; private set; }
    public Parry_Skill parry_Skill { get; private set; }
    public Dodge_Skill dodge_Skill { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dash_Skill = GetComponent<Dash_Skill>();
        clone_Skill = GetComponent<Clone_Skill>();
        Sword_Skill = GetComponent<Sword_Skill>();
        blockHole_Skill = GetComponent<BlockHole_Skill>();
        crystal_Skill = GetComponent<Crystal_Skill>();
        parry_Skill = GetComponent<Parry_Skill>();
        dodge_Skill = GetComponent<Dodge_Skill>();
    }
}
