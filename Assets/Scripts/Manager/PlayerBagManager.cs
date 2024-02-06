using System.Collections;
using System.Collections.Generic;
using CommonFramework.Runtime;
using UnityEngine;

public class PlayerBagManager : MonoSingleton<PlayerBagManager>
{
    public ItemDataList ItemDataList; // 物品数据
    public PlayerBagItemDataList PlayerBagItemDataList; // 背包数据

    // 根据物品id，返回对应物品详细列表
    public Data.ItemDetails GetItemDetails(int itemId)
    {
        return ItemDataList.ItemDetailsList.Find(i => i.ItemID == itemId);
    }

    public void AddItem(PlayerBagItem item, bool isDelete)
    {
        // 背包满了，无法继续装新物品
        if (!CheckBagHaveEmptySpace())
        {
            Debug.Log("背包已满，无法继续拾取物品！！！");
            return;
        }

        var index = GetItemIndex(item.itemId);
        AddItemAtIndex(item.itemId, index, 1);
        if (isDelete)
        {
            Destroy(item.gameObject);
        }
    }

    private void AddItemAtIndex(int itemId, int index, int count)
    {
        // 物品不存在
        if (index == -1)
        {
            var item = new Data.PlayerBagItemDetails { ItemID = itemId, ItemCount = count };
            for (int i = 0; i < PlayerBagItemDataList.PlayerBagItemDetailsList.Count; i++)
            {
                // 找背包的空位
                if (PlayerBagItemDataList.PlayerBagItemDetailsList[i].ItemID == 0)
                {
                    PlayerBagItemDataList.PlayerBagItemDetailsList[i] = item;
                    break;
                }
            }
        }
        else // 物品存在
        {
            var item = new Data.PlayerBagItemDetails
            {
                ItemID = itemId, ItemCount = PlayerBagItemDataList.PlayerBagItemDetailsList[index].ItemCount + count
            };
            PlayerBagItemDataList.PlayerBagItemDetailsList[index] = item;
        }
    }

    private int GetItemIndex(int itemId)
    {
        for (int i = 0; i < PlayerBagItemDataList.PlayerBagItemDetailsList.Count; i++)
        {
            if (PlayerBagItemDataList.PlayerBagItemDetailsList[i].ItemID == itemId)
            {
                return i;
            }
        }

        return -1;
    }

    private bool CheckBagHaveEmptySpace()
    {
        for (int i = 0; i < PlayerBagItemDataList.PlayerBagItemDetailsList.Count; i++)
        {
            if (PlayerBagItemDataList.PlayerBagItemDetailsList[i].ItemID == 0)
            {
                return true;
            }
        }

        return false;
    }
}