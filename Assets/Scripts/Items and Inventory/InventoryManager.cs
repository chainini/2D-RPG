using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 物品管理 单例
/// </summary>
public class InventoryManager : MonoBehaviour, ISaveManager
{
    public static InventoryManager Instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDict;
    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDict;
    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDict;


    [Header("InventoryManager UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_Stat_Slot[] statSlot;


    [Header("Item coolDown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;
    public float flaskCoolDown { get; private set; }
    private float armorCoolDown;


    [Header("Data base")]
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipments;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDict = new Dictionary<ItemData, InventoryItem>();
        stash = new List<InventoryItem>();
        stashDict = new Dictionary<ItemData, InventoryItem>();
        equipment = new List<InventoryItem>();
        equipmentDict = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_Stat_Slot>();


        AddStartingItem();
    }

    /// <summary>
    /// 初始化 预先配置的物品
    /// </summary>
    private void AddStartingItem()
    {
        foreach (ItemData_Equipment item in loadedEquipments)
        {
            EquipItem(item);
        }

        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
            return;
        }


        for (int i = 0; i < startingItems.Count; i++)
        {
            if (startingItems[i] != null)
                AddItem(startingItems[i]);
        }
    }

    /// <summary>
    /// 装备物品
    /// </summary>
    /// <param name="_item">物品</param>
    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDict)
        {
            if (item.Key.equipemntType == newEquipment.equipemntType)
                oldEquipment = item.Key;
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }


        equipment.Add(newItem);
        equipmentDict.Add(newEquipment, newItem);
        newEquipment.AddModifiers();
        RemoveItem(_item);

        UpdateSlotUI();
    }

    /// <summary>
    /// 卸载物品
    /// </summary>
    /// <param name="itemToRemove">物品</param>
    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDict.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDict.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    /// <summary>
    /// 更新UI槽
    /// </summary>
    private void UpdateSlotUI()
    {

        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDict)
            {
                if (item.Key.equipemntType == equipmentSlot[i].slotType)
                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }



        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].ClearUpSlot();
        }
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].ClearUpSlot();
        }



        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStatUI();
    }

    /// <summary>
    /// 更新数值UI
    /// </summary>
    public void UpdateStatUI()
    {
        for (int i = 0; i < statSlot.Length; i++) //update into of stats character UI
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    /// <summary>
    /// 捡起物品
    /// </summary>
    /// <param name="_item">物品</param>
    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddItemToStash(_item);

        }
        else if (_item.itemType == ItemType.Material)
        {
            AddItemToInventory(_item);

        }

        UpdateSlotUI();
    }

    /// <summary>
    /// 将物品加入 材料
    /// </summary>
    /// <param name="_item">物品</param>
    private void AddItemToStash(ItemData _item)
    {
        if (stashDict.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDict.Add(_item, newItem);
        }
    }

    /// <summary>
    /// 将物品加入装备
    /// </summary>
    /// <param name="_item">物品</param>
    private void AddItemToInventory(ItemData _item)
    {
        if (inventoryDict.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDict.Add(_item, newItem);
        }
    }

    /// <summary>
    /// 丢弃物品
    /// </summary>
    /// <param name="_item">物品</param>
    public void RemoveItem(ItemData _item)
    {
        if (inventoryDict.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDict.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }

        if (stashDict.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDict.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateSlotUI();
    }

    /// <summary>
    /// 合成物品
    /// </summary>
    /// <param name="_itemToCraft">要合成的物品</param>
    /// <param name="_requiredMaterials">合成物品的材料</param>
    /// <returns></returns>
    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (inventoryDict.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("not enough materials");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }

            }
            else
            {
                Debug.Log("not enough materials");
                return false;
            }
        }


        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }

        AddItem(_itemToCraft);
        Debug.Log("Here is your item " + _itemToCraft.name);

        return true;
    }

    /// <summary>
    /// 能否捡起物品
    /// </summary>
    /// <returns></returns>
    public bool CanAddItem()
    {
        if (stash.Count >= stashItemSlot.Length)
        {
            Debug.Log("No more space");
            return false;
        }
        return true;
    }
    /// <summary>
    /// 获取装备列表
    /// </summary>
    /// <returns></returns>
    public List<InventoryItem> GetEquipmentList() => equipment;
    /// <summary>
    /// 获取物品列表
    /// </summary>
    /// <returns></returns>
    public List<InventoryItem> GetInventoryList() => inventory;

    /// <summary>
    /// 获取装备
    /// </summary>
    /// <param name="_equipemntType">装备</param>
    /// <returns></returns>
    public ItemData_Equipment GetEquipment(EquipemntType _equipemntType)
    {
        ItemData_Equipment equipmentItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDict)
        {
            if (item.Key.equipemntType == _equipemntType)
                equipmentItem = item.Key;
        }

        return equipmentItem;
    }

    /// <summary>
    /// 使用药水
    /// </summary>
    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipemntType.Flask);

        if (currentFlask == null)
            return;

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCoolDown;

        if (canUseFlask)
        {
            flaskCoolDown = currentFlask.itemCoolDown;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
        {
            Debug.Log("Flask on CoolDown");
        }
    }

    /// <summary>
    /// 能否使用护甲技能
    /// </summary>
    /// <returns></returns>
    public bool CanUseArmor()
    {
        ItemData_Equipment currentEquipment = GetEquipment(EquipemntType.Armor);

        if (Time.time > lastTimeUsedArmor + armorCoolDown)
        {
            armorCoolDown = currentEquipment.itemCoolDown;
            lastTimeUsedArmor = Time.time;
            return true;
        }

        Debug.Log("Armor on coolDown");
        return false;
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemID == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }


        foreach (string loadedItemID in _data.equipmentsID)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && loadedItemID == item.itemID)
                {
                    loadedEquipments.Add(item as ItemData_Equipment);
                }
            }
        }

    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentsID.Clear();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDict)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }
        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDict)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDict)
        {
            _data.equipmentsID.Add(pair.Key.itemID);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("fill up item database")]
    private void FillupItemDatabase() => itemDataBase = new List<ItemData>(GetItemDataBase());
    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
#endif


}
