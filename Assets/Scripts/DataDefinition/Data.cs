using UnityEngine;

public class Data
{
    /// <summary>
    /// 各个物品基础信息
    /// </summary>
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

    /// <summary>
    /// 玩家背包物品信息
    /// </summary>
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

    /// <summary>
    /// 瓦片坐标信息
    /// </summary>
    [System.Serializable]
    public class TileDatas
    {
        public Vector2Int TilePos;
        public Enums.TileType TileType;
        public bool IsUsed;
    }

    /// <summary>
    /// 瓦片详细信息
    /// </summary>
    [System.Serializable]
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

    [System.Serializable]
    public class CropDetails
    {
        public int SeedID;
        public int[] SeedGrowthDays; // 生长到不同时期对应天数

        public int TotalGrowthDays
        {
            get
            {
                int amount = 0;
                foreach (var day in SeedGrowthDays)
                {
                    amount += day;
                }

                return amount;
            }
        }

        public GameObject[] SeedGrowthPrefab;
        public Sprite[] SeedGrowthSprite;
        public Enums.Season[] SeedSeasons;
        public int[] SeedHarvestToolID; // 哪些工具能收割
        public int[] SeedRequireActionCount; // 每个工具对应收割次数
        public int SeedTransferID; // 转换新物品的ID，树->树状
        public int[] SeedProductID; // 收割得到的种子id
        public int[] SeedProductMinAmount; // 收获果实最小数量
        public int[] SeedProductMaxAmount; // 收获果实最大数量
        public Vector2 SeedSpawnRadius; // 生成范围
        public int SeedDaysToRegrow; // 再次生长
        public int SeedRegrowTimes; // 可生长次数
        public bool SeedGenerateAtPlayer; // 是否在人物头顶生成
        public bool SeedHasAnimation; // 是否有动画
    }

    [System.Serializable]
    public class AnimatorType
    {
        public Enums.PartName PartName;
        public Enums.PartType PartType;
        public AnimatorOverrideController OverrideController;
    }
}