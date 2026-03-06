using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装备类型
/// </summary>
public enum EquipemntType
{
    Weapon,
    Armor,
    Amuler,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipemntType equipemntType;

    [Header("Unique effect")]
    public float itemCoolDown;
    public ItemEffect[] itemEffects;

    [Header("Major stats")]
    public float strength;
    public float agility;
    public float intelligance;
    public float vitality;

    [Header("Offensive stats")]
    public float damage;
    public float critChance;
    public float critPower;

    [Header("Defensive stats")]
    public float health;
    public float armor;
    public float evasion;
    public float magicResistance;

    [Header("Magic stats")]
    public float fireDamage;
    public float iceDamage;
    public float lightingDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;


    private int descriptionLength;

    public void Effect(Transform _enemyPos)
    {
        foreach (var effect in itemEffects)
        {
            effect.ExecuteEffect(_enemyPos);
        }
    }

    /// <summary>
    /// 角色+装备的属性
    /// </summary>
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligance);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHp.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }
    /// <summary>
    /// 角色-装备的属性
    /// </summary>
    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligance) ;
        playerStats.vitality.RemoveModifier(vitality) ;

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance) ;
        playerStats.critPower.RemoveModifier(critPower) ;

        playerStats.maxHp.RemoveModifier(health) ;
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion) ;
        playerStats.magicResistance.RemoveModifier(magicResistance) ;

        playerStats.fireDamage.RemoveModifier(fireDamage) ;
        playerStats.iceDamage.RemoveModifier(iceDamage) ;
        playerStats.lightingDamage.RemoveModifier(lightingDamage) ;
    }

    /// <summary>
    /// 描述
    /// </summary>
    /// <returns></returns>
    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(Mathf.RoundToInt(strength), "Strength");
        AddItemDescription(Mathf.RoundToInt(agility), "Agility");
        AddItemDescription(Mathf.RoundToInt(intelligance), "Intelligance");
        AddItemDescription(Mathf.RoundToInt(vitality), "Vitality");

        AddItemDescription(Mathf.RoundToInt(damage), "Damage");
        AddItemDescription(Mathf.RoundToInt(critChance), "CritChance");
        AddItemDescription(Mathf.RoundToInt(critPower), "CritPower");

        AddItemDescription(Mathf.RoundToInt(health), "Health");
        AddItemDescription(Mathf.RoundToInt(evasion), "Evasion");
        AddItemDescription(Mathf.RoundToInt(armor), "Armor");
        AddItemDescription(Mathf.RoundToInt(magicResistance), "MagicResistance");
        AddItemDescription(Mathf.RoundToInt(fireDamage), "FireDamage");
        AddItemDescription(Mathf.RoundToInt(iceDamage), "IceDamage");
        AddItemDescription(Mathf.RoundToInt(lightingDamage), "LightingDamage");


        for(int i = 0;i<itemEffects.Length;i++)
        {
            if (itemEffects[i].effectDescription.Length>0)
            {
                sb.AppendLine();
                sb.Append("Unique: " +  itemEffects[i].effectDescription);
                descriptionLength++;
            }
        }

        if(descriptionLength<5)
        {
            for(int i = 0;i< 5 - descriptionLength;i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        return sb.ToString();
    }

    private void AddItemDescription(int _value,string _name)
    {
        if(_value !=0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if(_value>0)
                sb.Append("+ "+_value + " "+_name);

            descriptionLength++;
        }
    }
}
