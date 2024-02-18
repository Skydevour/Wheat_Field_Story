using System;
using System.Collections;
using System.Collections.Generic;
using CommonFramework.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBagUI : MonoBehaviour
{
    [SerializeField] private bool isOpen;
    [SerializeField] private GameObject BagUI;
    [SerializeField] private Slot[] PlayerSlots;

    private void OnEnable()
    {
        EventCenter.StartListenToEvent<UpdatePlayerBagEvent>(UpdatePlayerBag);
    }

    private void OnDisable()
    {
        EventCenter.StopListenToEvent<UpdatePlayerBagEvent>(UpdatePlayerBag);
    }

    private void Start()
    {
        for (int i = 0; i < PlayerSlots.Length; i++)
        {
            PlayerSlots[i].SlotIndex = i;
        }

        isOpen = BagUI.activeInHierarchy;
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
                for (int i = 0; i < PlayerSlots.Length; i++)
                {
                    if (evt.List[i].ItemCount > 0)
                    {
                        var itemDetail = PlayerBagManager.Instance.GetItemDetails(evt.List[i].ItemID);
                        PlayerSlots[i].UpdateSlot(itemDetail, evt.List[i].ItemCount);
                    }
                    else
                    {
                        PlayerSlots[i].UpdateEmptySlot();
                    }
                }

                break;
            case Enums.BagLocation.Box:
                break;
            default:
                break;
        }
    }

    private void OpenBag()
    {
        isOpen = !isOpen;
        BagUI.SetActive(isOpen);
    }

    public void UpdateSlotHighLight(int index)
    {
        foreach (var slot in PlayerSlots)
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