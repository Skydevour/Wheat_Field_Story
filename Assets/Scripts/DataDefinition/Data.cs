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

    [System.Serializable]
    public class SerializableV3
    {
        public float X, Y, Z;

        public SerializableV3(Vector3 pos)
        {
            X = pos.x;
            Y = pos.y;
            Z = pos.z;
        }

        public Vector3 ToV3()
        {
            return new Vector3(X, Y, Z);
        }

        public Vector2Int ToV2Int()
        {
            return new Vector2Int((int)X, (int)Y);
        }
    }

    [System.Serializable]
    public class SceneItem
    {
        public int ItemID; // 物品id
        public SerializableV3 Pos;
    }
}