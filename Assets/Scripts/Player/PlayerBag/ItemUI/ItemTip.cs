using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemTip : MonoBehaviour
{
    [SerializeField] private Text tipName;
    [SerializeField] private Text tipType;
    [SerializeField] private Text tipDescription;
    [SerializeField] private Text tipPrice;
    [SerializeField] private GameObject tipBottom;

    public void SetTip(Data.ItemDetails itemDetails, Enums.SlotType slotType)
    {
        tipName.text = "物品名字：" + itemDetails.ItemName;
        tipType.text = "物品类型：" + itemDetails.ItemType;
        tipDescription.text = "物品描述：" + itemDetails.ItemDescription;
        if (itemDetails.ItemType == Enums.ItemType.Seed || itemDetails.ItemType == Enums.ItemType.Commodity ||
            itemDetails.ItemType == Enums.ItemType.Furniture)
        {
            tipBottom.SetActive(true);
            var price = itemDetails.ItemPrice;
            if (slotType == Enums.SlotType.Bag)
            {
                price = Convert.ToInt32(price * itemDetails.SellRate);
            }

            tipPrice.text = price.ToString();
        }
        else
        {
            tipBottom.SetActive(false);
        }
    }
}