using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/ItemDataList", fileName = "ItemDataList")]
public class ItemDataList : ScriptableObject
{
    // 数据存成配置表
    public List<Data.ItemDetails> ItemDetailsList;
}