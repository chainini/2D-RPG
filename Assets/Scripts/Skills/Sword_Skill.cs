using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 剑技能的类型
/// </summary>
public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType;


    [Header("Bounce info")]
    [SerializeField] private UI_SkillTreeSlot unlockBounceSwordButton;
    [SerializeField] private int amountOfBounce;

    [Header("Pierce info")]
    [SerializeField] private UI_SkillTreeSlot unlockPierceSwordButton;
    [SerializeField] private int amountOfPierce;
    [SerializeField] private float pierceGravity;
    [SerializeField] private float pierceSpeed;

    [Header("Spin info")]
    [SerializeField] private UI_SkillTreeSlot unlockSpinSwordButton;
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float intervalHitTime;
    [SerializeField] private float stopDuration;
    [SerializeField] private float spinGravity;


    [Header("Skill info")]
    [SerializeField] private UI_SkillTreeSlot unlockSwordButton;
    public bool unlockSword { get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTime;
    [SerializeField] private float returnSpeed;


    [Header("Passive skill")]
    [SerializeField] private UI_SkillTreeSlot unlockTimeStopButton;
    public bool unlockTimeStop { get; private set; }
    [SerializeField] private UI_SkillTreeSlot unlockVulnurableButton;
    public bool unlockVulnurable { get; private set; }


    [Header("Aim dots")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotParent;
    [SerializeField] private int dotNumber;
    [SerializeField] private float spaceBetweenDot;
    private GameObject[] dots;

    private Vector2 finalDir;

    protected override void Start()
    {
        base.Start();
        GerenateDots();


        unlockSwordButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        unlockBounceSwordButton.GetComponent<Button>().onClick.AddListener(UnlockBounce);
        unlockPierceSwordButton.GetComponent<Button>().onClick.AddListener(UnlockPierce);
        unlockSpinSwordButton.GetComponent<Button>().onClick.AddListener(UnlockSpin);
        unlockTimeStopButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        unlockVulnurableButton.GetComponent<Button>().onClick.AddListener(UnlockVulnurable);


        SetGravity();
    }
    /// <summary>
    /// 设置每种模式下 剑的重力
    /// </summary>
    private void SetGravity()
    {
        if (swordType == SwordType.Bounce)
        {

        }
        else if (swordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        }
        else if (swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dotNumber; i++)
            {
                dots[i].transform.position = DotPosition(i * spaceBetweenDot);
            }
        }

    }

    /// <summary>
    /// 创建剑
    /// </summary>
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, player.transform.rotation);


        if (swordType == SwordType.Bounce)
        {
            newSword.GetComponent<SwordSkillControl>().SetupBounce(true, amountOfBounce);
        }
        else if (swordType == SwordType.Pierce)
        {
            newSword.GetComponent<SwordSkillControl>().SetupPierce(amountOfPierce, pierceSpeed);
        }
        else if (swordType == SwordType.Spin)
        {
            newSword.GetComponent<SwordSkillControl>().SetupSpin(maxTravelDistance, intervalHitTime, stopDuration, true);
        }

        newSword.GetComponent<SwordSkillControl>().SetupSword(finalDir, swordGravity, player, freezeTime, returnSpeed);

        player.SginSword(newSword);

        ActiveDots(false);
    }

    
    public override void CheckUnlock()
    {

        UnlockSword();
        UnlockBounce();
        UnlockPierce();
        UnlockSpin();
        UnlockTimeStop();
        UnlockVulnurable();
    }

    #region Unlock Skill

    private void UnlockTimeStop()
    {
        if (unlockTimeStopButton.unlocked)
            unlockTimeStop = true;
    }
    private void UnlockVulnurable()
    {
        if (unlockVulnurableButton.unlocked)
            unlockVulnurable = true;
    }
    private void UnlockSword()
    {
        if (unlockSwordButton.unlocked)
        {
            swordType = SwordType.Regular;
            unlockSword = true;
        }
    }
    private void UnlockBounce()
    {
        if (unlockBounceSwordButton.unlocked)
            swordType = SwordType.Bounce;
    }
    private void UnlockPierce()
    {
        if (unlockPierceSwordButton.unlocked)
            swordType = SwordType.Pierce;
    }
    private void UnlockSpin()
    {
        if (unlockSpinSwordButton.unlocked)
            swordType = SwordType.Spin;
    }


    #endregion

    #region Aim
    /// <summary>
    /// 投掷方向
    /// </summary>
    /// <returns></returns>
    public Vector2 AimDirection()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return mousePos - (Vector2)player.transform.position;
    }

    /// <summary>
    /// 激活轨迹
    /// </summary>
    /// <param name="_isActive">是否激活</param>
    public void ActiveDots(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    /// <summary>
    /// 生成轨迹
    /// </summary>
    private void GerenateDots()
    {
        dots = new GameObject[dotNumber];
        for (int i = 0; i < dotNumber; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }
    }
    /// <summary>
    /// 轨迹点的位置
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private Vector2 DotPosition(float t)
    {
        Vector2 Pos = (Vector2)player.transform.position + new Vector2(
            launchForce.x * AimDirection().normalized.x,
            launchForce.y * AimDirection().normalized.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return Pos;
    }
    #endregion
}
