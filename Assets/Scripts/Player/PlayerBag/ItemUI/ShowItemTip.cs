using System;
using System.Collections;
using System.Collections.Generic;
using CommonFramework.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Slot))]
public class ShowItemTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Slot slot;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slot.SlotItemDetails != null)
        {
            EventCenter.TriggerEvent(new ShowItemTipEvent(true, slot));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventCenter.TriggerEvent(new ShowItemTipEvent(false, null));
    }
}