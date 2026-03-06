using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{

    protected override void Start()
    {
        base.Start();


    }

    /// <summary>
    /// 设置装备信息
    /// </summary>
    /// <param name="_data">装备</param>
    public void SetupCraftSlot(ItemData_Equipment _data)
    {
        if (_data == null) return;

        item.data = _data;

        itemImage.sprite = _data.itemIcon;
        itemText.text = _data.itemName;

        if (itemText.text.Length > 12)
            itemText.fontSize = itemText.fontSize * .8f;
        else
            itemText.fontSize = 24;

    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
    }
}
