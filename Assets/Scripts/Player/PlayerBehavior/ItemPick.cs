using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPick : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            PlayerBagItem playerBagItem = other.gameObject.GetComponent<PlayerBagItem>();
            if (playerBagItem.itemDetail.CanPick)
            {
                PlayerBagManager.Instance.AddItem(playerBagItem, true);
            }
        }
    }
}