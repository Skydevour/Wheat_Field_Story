using System.Collections;
using System.Collections.Generic;
using CommonFramework.Runtime;
using UnityEngine;

public class PlayerBagManager : MonoSingleton<PlayerBagManager>
{
    public ItemDataList ItemDataList;

    // 根据物品id，返回对应物品详细列表
    public Data.ItemDetails GetItemDetails(int itemId)
    {
        return ItemDataList.ItemDetailsList.Find(i => i.ItemID == itemId);
    }
}