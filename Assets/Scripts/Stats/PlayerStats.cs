using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TackDamage(float damage)
    {
        base.TackDamage(damage);

    }

    protected override void Die()
    {
        base.Die();
        player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(float _damage)
    {
        base.DecreaseHealthBy(_damage);

        if (isDead) return;

        if (_damage > GetMaxHealthValue() * .3f)
        {
            player.SetupKnockBackPower(new Vector2(10, 9));
            player.playerFX.ScreenShake(new Vector3(player.playerFX.shakeHighDamage.x, player.playerFX.shakeHighDamage.y));

            GetComponent<Entity>().DamageImpact();
            AudioManager.instance.PlaySFX(34, null);
        }
        AudioManager.instance.PlaySFX(35, null);


        ItemData_Equipment currentEquipment = InventoryManager.Instance.GetEquipment(EquipemntType.Armor);

        if (currentEquipment != null)
        {
            currentEquipment.Effect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        player.skill.dodge_Skill.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats, float _multiplier)
    {
        if (TargetCanMissAttack(_targetStats))
            return;

        float totalDamage = damage.GetValue() + strength.GetValue();

        if (_multiplier > 0)
        {
            totalDamage = totalDamage * _multiplier;
        }

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TackDamage(totalDamage);
        //if inventeroy current weapon has fire effect
        //  then  DoMagicDamage(_targetStats);

        DoMagicDamage(_targetStats);
    }
}
