using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    private SkillManager skills;

    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate = 100;


    private void Start()
    {
        if (playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHealthUI;
        }

        skills = SkillManager.instance;
    }

    private void Update()
    {
        UpdateSoulsUI();

        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash_Skill.dashUnlocked)
        {
            SetCoolDownOf(dashImage);
        }
        if (Input.GetKeyDown(KeyCode.F) && skills.parry_Skill.parryUnlocked)
        {
            SetCoolDownOf(parryImage);
        }
        if (Input.GetKeyDown(KeyCode.C) && skills.crystal_Skill.crystalUnlocked)
        {
            SetCoolDownOf(crystalImage);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.Sword_Skill.unlockSword)
        {
            SetCoolDownOf(swordImage);
        }
        if (Input.GetKeyDown(KeyCode.R) && skills.blockHole_Skill.unlockBlackHole)
        {
            SetCoolDownOf(blackholeImage);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && InventoryManager.Instance.GetEquipment(EquipemntType.Flask) != null)
        {
            SetCoolDownOf(flaskImage);
        }

        CheckCoolDownOf(dashImage, skills.dash_Skill.coolDown);
        CheckCoolDownOf(parryImage, skills.parry_Skill.coolDown);
        CheckCoolDownOf(crystalImage, skills.crystal_Skill.coolDown);
        CheckCoolDownOf(swordImage, skills.Sword_Skill.coolDown);
        CheckCoolDownOf(blackholeImage, skills.blockHole_Skill.coolDown);
        CheckCoolDownOf(flaskImage, InventoryManager.Instance.flaskCoolDown);
    }

    private void UpdateSoulsUI()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrentCurrency())
            soulsAmount += Time.deltaTime * increaseRate;
        else
            soulsAmount = PlayerManager.instance.GetCurrentCurrency();

        currentSouls.text = ((int)soulsAmount).ToString();
    }

    public void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHp;
    }

    private void SetCoolDownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCoolDownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }
}
