using UnityEngine;

public class Data
{
    [System.Serializable]
    public class ItemDetails // 物品详细信息
    {
        public int ItemID; // 物品id
        public string ItemName;
        public Enums.ItemType ItemType;
        public Sprite ItemIcon;
        public Sprite ItemBuildIcon; // 建造图纸
        public string ItemDescription;
        public int ItemUseRange; // 物品适用范围
        public bool CanPick;
        public bool CanDrop;
        public bool CanCarry;
        public int ItemPrice;
        [Range(0, 1)] public float SellRate;
    }

    [System.Serializable]
    public struct PlayerBagItemDetails
    {
        public int ItemID; // 物品id
        public int ItemCount;
    }
}