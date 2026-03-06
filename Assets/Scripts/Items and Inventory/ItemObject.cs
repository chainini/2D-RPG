using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private Rigidbody2D rb;

    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
        gameObject.name = "Item object -" + itemData.name;
    }

    public void SetupItem(ItemData _itemData,Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }


    public void PickUpItem()
    {
        if (!InventoryManager.Instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0,7);
            PlayerManager.instance.player.playerFX.CreatePopUpText("InventoryManager is full");
            return;
        }
        AudioManager.instance.PlaySFX(18, transform);
        InventoryManager.Instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
