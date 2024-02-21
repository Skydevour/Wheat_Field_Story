using System;
using System.Collections;
using System.Collections.Generic;
using CommonFramework.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBagUI : MonoBehaviour
{
    [SerializeField] private GameObject bagUI;
    [SerializeField] private Slot[] playerSlots;
    [SerializeField] private ItemTip itemTip;

    private bool isOpen;

    public Image dragItem;

    private void OnEnable()
    {
        EventCenter.StartListenToEvent<UpdatePlayerBagEvent>(UpdatePlayerBag);
        EventCenter.StartListenToEvent<ShowItemTipEvent>(ShowTip);

    }

    private void OnDisable()
    {
        EventCenter.StopListenToEvent<UpdatePlayerBagEvent>(UpdatePlayerBag);
        EventCenter.StopListenToEvent<ShowItemTipEvent>(ShowTip);

    }

    private void Start()
    {
        for (int i = 0; i < playerSlots.Length; i++)
        {
            playerSlots[i].SlotIndex = i;
        }

        isOpen = bagUI.GetComponent<CanvasGroup>().alpha == 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenBag();
        }
    }

    private void UpdatePlayerBag(UpdatePlayerBagEvent evt)
    {
        switch (evt.Location)
        {
            case Enums.BagLocation.Player:
                for (int i = 0; i < playerSlots.Length; i++)
                {
                    if (evt.List[i].ItemCount > 0)
                    {
                        var itemDetail = PlayerBagManager.Instance.GetItemDetails(evt.List[i].ItemID);
                        playerSlots[i].UpdateSlot(itemDetail, evt.List[i].ItemCount);
                    }
                    else
                    {
                        playerSlots[i].UpdateEmptySlot();
                    }
                }

                break;
            case Enums.BagLocation.Box:
                break;
            default:
                break;
        }
    }

    private void ShowTip(ShowItemTipEvent evt)
    {
        if (evt.IsShow)
        {
            itemTip.SetTip(evt.SlotBag.SlotItemDetails, evt.SlotBag.SlotType);
            // 避免Content layout没有及时自适应，该代码可以强行刷新，使得自适应及时
            LayoutRebuilder.ForceRebuildLayoutImmediate(itemTip.GetComponent<RectTransform>());
            itemTip.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            itemTip.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    private void OpenBag()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            bagUI.GetComponent<CanvasGroup>().alpha = 1;
            bagUI.GetComponent<CanvasGroup>().interactable = true;
            bagUI.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            bagUI.GetComponent<CanvasGroup>().alpha = 0;
            bagUI.GetComponent<CanvasGroup>().interactable = false;
            bagUI.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void UpdateSlotHighLight(int index)
    {
        foreach (var slot in playerSlots)
        {
            if (slot.IsSelected && slot.SlotIndex == index)
            {
                slot.SlotHighLight.gameObject.SetActive(true);
            }
            else
            {
                slot.IsSelected = false;
                slot.SlotHighLight.gameObject.SetActive(false);
            }
        }
    }
}