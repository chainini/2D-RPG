using System.Collections;
using UnityEngine;

public enum StatType
{
    Strength,
    Agility,
    Intelligence,
    Vitality,
    Damage,
    CritChance,
    CritPower,
    MaxHp,
    Armor,
    Evasion,
    MagicResistance,
    FireDamage,
    IceDamage,
    LightingDamage
}
public class CharacterStats : MonoBehaviour
{
    public EntityFX fx;

    [Header("Major stats")]
    public Stat strength;// 1 point increase damage by 1 and crit.power by 1%
    public Stat agility;// 1 point increase evasion by 1% and crit.chance by 1% 
    public Stat intelligence;// 1 point increase magic damage by 1 and magic resistance by 3
    public Stat vitality;// 1 point incredase health by 3 or 5points

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;      //default value 150%

    [Header("Defensive stats")]
    public Stat maxHp;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited;// does damage over time
    public bool ischilled;// reduce armor by 20%
    public bool isShocked;// reduce accurary by 20%

    [SerializeField] private float defaultEffectDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCoolDonw = .3f;
    private float ignitedDamageTimer;
    private float igniteDamage;


    [SerializeField] private GameObject shockStrikePrefab;
    private float shockStrikeDamage;


    public float currentHp;

    public System.Action onHealthChanged;
    public bool isDead { get; private set; }
    public bool isInvincible { get; private set; }
    public bool isVulnurable;

    protected virtual void Start()
    {
        //这里拿不到子物体里的脚本？？
        //fx.GetComponentInChildren<EntityFX>();

        critPower.SetDefaultValue(150);
        currentHp = GetMaxHealthValue();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;
        if (chilledTimer < 0)
            ischilled = false;
        if (shockedTimer < 0)
        {
            isShocked = false;
        }

        if (isIgnited)
            ApplyIgniteDamage();
    }

    public void MakeVulnurableFor(float _duration) => StartCoroutine(VulnurableForCoroutine(_duration));

    private IEnumerator VulnurableForCoroutine(float _duration)
    {
        isVulnurable = true;

        yield return new WaitForSeconds(_duration);

        isVulnurable = false;
    }


    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statModifier)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statModifier));
    }
    IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statModifier)
    {
        _statModifier.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statModifier.RemoveModifier(_modifier);
    }


    public virtual void DoDamage(CharacterStats _targetStats)
    {
        bool criticalStrike = false;

        if (TargetCanMissAttack(_targetStats))
            return;

        if (_targetStats.isInvincible)
            return;

        _targetStats.GetComponent<Entity>().SetupKnockBackDir(transform);

        float totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            criticalStrike = true;
        }
        fx.CreateHitFX(_targetStats.transform, criticalStrike);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TackDamage(totalDamage);
        //if inventeroy current weapon has fire effect
        //  then  DoMagicDamage(_targetStats);

        DoMagicDamage(_targetStats);
    }
    #region Magical damage and aliment
    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        float _fireDamage = fireDamage.GetValue();
        float _iceDamage = iceDamage.GetValue();
        float _lightingDamage = lightingDamage.GetValue();

        float totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);
        _targetStats.TackDamage(totalMagicDamage);


        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
        {
            return;
        }

        AttemptyToApplyAliment(_targetStats, _fireDamage, _iceDamage, _lightingDamage);

    }

    private void AttemptyToApplyAliment(CharacterStats _targetStats, float _fireDamage, float _iceDamage, float _lightingDamage)
    {
        bool isApplyIgnite = fireDamage.GetValue() > iceDamage.GetValue() && fireDamage.GetValue() > lightingDamage.GetValue();
        bool isApplyChill = iceDamage.GetValue() > lightingDamage.GetValue() && iceDamage.GetValue() > lightingDamage.GetValue();
        bool isApplyShock = lightingDamage.GetValue() > fireDamage.GetValue() && lightingDamage.GetValue() > iceDamage.GetValue();

        while (!isApplyIgnite && !isApplyChill && !isApplyShock)
        {
            if (UnityEngine.Random.value < .3f && _fireDamage > 0)
            {
                isApplyIgnite = true;
                _targetStats.ApplyAilments(isApplyIgnite, isApplyChill, isApplyShock);
                return;
            }
            if (UnityEngine.Random.value < .5f && _iceDamage > 0)
            {
                isApplyChill = true;
                _targetStats.ApplyAilments(isApplyIgnite, isApplyChill, isApplyShock);
                return;
            }
            if (UnityEngine.Random.value < .5f && _lightingDamage > 0)
            {
                isApplyShock = true;
                _targetStats.ApplyAilments(isApplyIgnite, isApplyChill, isApplyShock);
                return;
            }
        }

        if (isApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }
        if (isApplyShock)
        {
            _targetStats.SetupShockDamage(Mathf.RoundToInt(_lightingDamage * 1.5f));
        }

        _targetStats.ApplyAilments(isApplyIgnite, isApplyChill, isApplyShock);
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !ischilled && !isShocked;
        bool canApplyChill = !isIgnited && !ischilled && !isShocked;
        bool canApplyShock = !isIgnited && !ischilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = defaultEffectDuration;

            fx.IgniteFXFor(defaultEffectDuration);
        }
        if (_chill && canApplyChill)
        {
            ischilled = _chill;
            chilledTimer = defaultEffectDuration;

            float slowPercentage = .2f;

            GetComponent<Entity>().SlowEntityBy(slowPercentage, defaultEffectDuration);
            fx.ChillFXFor(defaultEffectDuration);
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                AlppyShock(_shock);
            }
            else
            {
                //shockStrikeAttack not allow use to player
                if (GetComponent<Player>() != null)
                    return;
                HitNearestTargetWithShockStrike();
            }
        }
    }

    public void AlppyShock(bool _shock)
    {
        isShocked = _shock;
        shockedTimer = defaultEffectDuration;

        fx.ShockFXFor(defaultEffectDuration);
    }
    private void ApplyIgniteDamage()
    {
        if (ignitedDamageTimer < 0)
        {

            DecreaseHealthBy(igniteDamage);

            if (currentHp < 0 && !isDead)
                Die();

            ignitedDamageTimer = igniteDamageCoolDonw;
        }
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closeDistance = Mathf.Infinity;

        Transform closeEnemy = null;

        foreach (var hit in colliders)
        {
            if ((hit.GetComponent<Enemy>() != null) && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceFormEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceFormEnemy < closeDistance)
                {
                    closeDistance = distanceFormEnemy;
                    closeEnemy = hit.transform;
                }
            }
            if (closeEnemy == null)
                closeEnemy = transform;
        }

        GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
        newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockStrikeDamage, closeEnemy.GetComponent<CharacterStats>());
    }
    public void SetupIgniteDamage(float _damage) => igniteDamage = _damage;
    public void SetupShockDamage(float _damage) => shockStrikeDamage = _damage;
    #endregion

    public virtual void TackDamage(float damage)
    {
        if (isInvincible)
            return;

        DecreaseHealthBy(damage);


        fx.StartFlasHFX();

        if (currentHp < 0 && !isDead)
        {
            Die();
        }
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHp += _amount;

        if (currentHp > GetMaxHealthValue())
            currentHp = GetMaxHealthValue();

        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void DecreaseHealthBy(float _damage)
    {
        if (isVulnurable)
            _damage *= 1.1f;

        currentHp -= _damage;

        if (_damage > 0)
            fx.CreatePopUpText(_damage.ToString());

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }

    }

    protected virtual void Die()
    {
        isDead = true;
    }

    public void killEnetity()
    {
        if (!isDead)
            Die();
    }

    public void MakeInvincible(bool _Invincible)
    {
        isInvincible = _Invincible;
    }

    #region Stat Calculations

    private float CheckTargetResistance(CharacterStats _targetStats, float totalMagicDamage)
    {
        totalMagicDamage -= magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }
    protected float CheckTargetArmor(CharacterStats _targetStats, float totalDamage)
    {
        if (ischilled)
        {
            totalDamage -= _targetStats.armor.GetValue() * .8f;
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }


        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    public virtual void OnEvasion()
    {

    }

    protected bool TargetCanMissAttack(CharacterStats _targetStats)
    {
        float totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }

        return false;
    }
    protected bool CanCrit()
    {
        float totalCritcalChance = critChance.GetValue() + agility.GetValue();
        if (UnityEngine.Random.Range(0, 100) <= totalCritcalChance)
        {
            return true;
        }

        return false;
    }
    protected int CalculateCriticalDamage(float _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critialDamage = _damage + totalCritPower;

        return Mathf.RoundToInt(critialDamage);
    }

    public float GetMaxHealthValue()
    {
        return maxHp.GetValue() + vitality.GetValue() * 5;
    }
    #endregion


    public Stat GetStat(StatType _statType)
    {


        if (_statType == StatType.Strength) return strength;
        else if (_statType == StatType.Agility) return agility;
        else if (_statType == StatType.Intelligence) return intelligence;
        else if (_statType == StatType.Vitality) return vitality;
        else if (_statType == StatType.Damage) return damage;
        else if (_statType == StatType.CritChance) return critChance;
        else if (_statType == StatType.CritPower) return critPower;
        else if (_statType == StatType.MaxHp) return maxHp;
        else if (_statType == StatType.Armor) return armor;
        else if (_statType == StatType.Evasion) return evasion;
        else if (_statType == StatType.MagicResistance) return magicResistance;
        else if (_statType == StatType.FireDamage) return fireDamage;
        else if (_statType == StatType.IceDamage) return iceDamage;
        else if (_statType == StatType.LightingDamage) return lightingDamage;

        return null;
    }
}
