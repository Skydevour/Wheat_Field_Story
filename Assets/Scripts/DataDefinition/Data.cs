using UnityEngine;

public class Data
{
    [System.Serializable]
    public class ItemDetails // 物品详细信息
    {
        public int Item_ID;
        public string Item_Name;
        public Enums.ItemType ItemType;
        public Sprite Item_Icon;
        public Sprite Item_BuildIcon; // 建造图纸
        public string Item_Description;
        public int Item_UseRange; // 物品适用范围
        public bool CanPick;
        public bool CanDrop;
        public bool CanCarry;
        public int Item_price;
        [Range(0, 1)] public float SellRate;
    }
}