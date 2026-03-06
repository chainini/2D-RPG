using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField]private TextMeshProUGUI itemDescription;

    private int defalutFontSize = 32;

    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item == null) return;

        itemNameText.text = item.itemName;
        itemTypeText.text = item.itemType.ToString();
        itemDescription.text = item.GetDescription();

        AdjustFontSize(itemNameText);
        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        itemNameText.fontSize = defalutFontSize;

        gameObject.SetActive(false);
    }
}
