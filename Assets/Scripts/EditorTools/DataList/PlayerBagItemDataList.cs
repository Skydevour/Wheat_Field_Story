using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/PlayerBagItemDataList", fileName = "PlayerBagItemDataList")]
public class PlayerBagItemDataList : ScriptableObject
{
    public List<Data.PlayerBagItemDetails> PlayerBagItemDetailsList;
}