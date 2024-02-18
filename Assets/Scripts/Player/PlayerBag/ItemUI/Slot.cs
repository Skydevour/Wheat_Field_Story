using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image slotImage; // 物品图片
    [SerializeField] private TextMeshProUGUI slotText; // 物品数量文本
    [SerializeField] private Button slotButton;

    private PlayerBagUI playerBagUI;

    public Image SlotHighLight; // 高亮边框
    public Enums.SlotType SlotType;
    public bool IsSelected;
    public int SlotIndex;
    public Data.ItemDetails SlotItemDetails;
    public int SlotAmount; // 物品数量

    private void Start()
    {
        playerBagUI = GetComponentInParent<PlayerBagUI>();
        IsSelected = false;
        if (SlotItemDetails == null)
        {
            UpdateEmptySlot();
        }
    }

    public void UpdateSlot(Data.ItemDetails itemDetails, int amount)
    {
        SlotItemDetails = itemDetails;
        slotImage.sprite = itemDetails.ItemIcon;
        SlotAmount = amount;
        slotText.text = amount.ToString();
        slotImage.enabled = true;
        slotButton.interactable = true;
    }

    public void UpdateEmptySlot()
    {
        if (IsSelected)
        {
            IsSelected = false;
            playerBagUI.UpdateSlotHighLight(-1);
        }

        SlotItemDetails = null;
        slotText.text = string.Empty;
        slotImage.enabled = false;
        slotButton.interactable = false;
    }

    /// <summary>
    /// 点击高亮显示
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (SlotItemDetails == null)
        {
            return;
        }

        IsSelected = !IsSelected;
        playerBagUI.UpdateSlotHighLight(SlotIndex);
    }
}