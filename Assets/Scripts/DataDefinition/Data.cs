using System;
using UnityEngine;

public class Data
{
    [Serializable]
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

    [Serializable]
    public struct PlayerBagItemDetails
    {
        public int ItemID; // 物品id
        public int ItemCount;
    }

    [Serializable]
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

    [Serializable]
    public class SceneItem
    {
        public int ItemID; // 物品id
        public SerializableV3 Pos;
    }

    [Serializable]
    public class TileDatas
    {
        public Vector2Int TilePos;
        public Enums.TileType TileType;
        public bool IsUsed;
    }

    [Serializable]
    public class TileDetails
    {
        public int TileGridX, TileGridY;
        public bool CanDig;
        public bool CanDrop;
        public bool CanPlaceFurniture;
        public bool NPCObstacle;
        public int DaySinceDug = -1;
        public int DaySinceWater = -1;
        public int SeedItemID = -1;
        public int GrowthDay = -1;
        public int DaySinceLastHarvest = -1;
    }
    
    [Serializable]
    public class AnimatorType
    {
        public Enums.PartName PartName;
        public Enums.PartType PartType;
        public AnimatorOverrideController OverrideController;
    }
}