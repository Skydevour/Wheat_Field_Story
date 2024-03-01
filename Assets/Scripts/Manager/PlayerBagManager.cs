using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using CommonFramework.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBagManager : MonoSingleton<PlayerBagManager>
{
    public ItemDataList ItemDataList; // 物品数据
    public PlayerBagItemDataList PlayerBagItemDataList; // 背包数据

    private void OnEnable()
    {
        EventCenter.StartListenToEvent<HarvestAtPlayer>(OnHarvestAtPlayer);
    }

    private void OnDisable()
    {
        EventCenter.StopListenToEvent<HarvestAtPlayer>(OnHarvestAtPlayer);
    }

    public void Start()
    {
        EventCenter.TriggerEvent(new UpdatePlayerBagEvent(Enums.BagLocation.Player,
            PlayerBagItemDataList.PlayerBagItemDetailsList));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(2);
        }
    }

    // 根据物品id，返回对应物品详细列表
    public Data.ItemDetails GetItemDetails(int itemId)
    {
        return ItemDataList.ItemDetailsList.Find(i => i.ItemID == itemId);
    }

    // 往背包添加物品，并设置该物品是一次性拾取还是可永久性拾取
    public void AddItem(PlayerBagItem item, bool isDelete)
    {
        // 背包满了，无法继续装新物品
        if (!CheckBagCanPutItem(item.itemId))
        {
            Debug.Log("背包已满，无法继续拾取物品！！！");
            return;
        }
        
        var index = GetItemIndex(item.itemId);
        AddItemAtIndex(item.itemId, index, 1);
        EventCenter.TriggerEvent(new UpdatePlayerBagEvent(Enums.BagLocation.Player,
            PlayerBagItemDataList.PlayerBagItemDetailsList));
        if (isDelete)
        {
            Destroy(item.gameObject);
        }
    }

    public void SwapItem(int originIndex, int targetIndex)
    {
        Data.PlayerBagItemDetails currentItemDetails = PlayerBagItemDataList.PlayerBagItemDetailsList[originIndex];
        Data.PlayerBagItemDetails targetItemDetails = PlayerBagItemDataList.PlayerBagItemDetailsList[targetIndex];
        if (targetItemDetails.ItemID != 0)
        {
            PlayerBagItemDataList.PlayerBagItemDetailsList[originIndex] = targetItemDetails;
            PlayerBagItemDataList.PlayerBagItemDetailsList[targetIndex] = currentItemDetails;
        }
        else
        {
            PlayerBagItemDataList.PlayerBagItemDetailsList[targetIndex] = currentItemDetails;
            PlayerBagItemDataList.PlayerBagItemDetailsList[originIndex] = new Data.PlayerBagItemDetails();
        }

        EventCenter.TriggerEvent(new UpdatePlayerBagEvent(Enums.BagLocation.Player,
            PlayerBagItemDataList.PlayerBagItemDetailsList));
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

    private bool CheckBagCanPutItem(int itemId)
    {
        for (int i = 0; i < PlayerBagItemDataList.PlayerBagItemDetailsList.Count; i++)
        {
            // 有空位，或者物品已经放过
            if (PlayerBagItemDataList.PlayerBagItemDetailsList[i].ItemID == 0 ||
                PlayerBagItemDataList.PlayerBagItemDetailsList[i].ItemID == itemId)
            {
                return true;
            }
        }

        return false;
    }

    private void OnHarvestAtPlayer(HarvestAtPlayer evt)
    {
        var index = GetItemIndex(evt.ID);
        AddItemAtIndex(evt.ID, index, 1);
        EventCenter.TriggerEvent(new UpdatePlayerBagEvent(Enums.BagLocation.Player,
            PlayerBagItemDataList.PlayerBagItemDetailsList));
    }
}