using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipemntType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null) return;

        InventoryManager.Instance.UnequipItem(item.data as ItemData_Equipment);
        InventoryManager.Instance.AddItem(item.data as ItemData_Equipment);

        ui.itemTooltip.HideToolTip();

        ClearUpSlot();
    }
}
