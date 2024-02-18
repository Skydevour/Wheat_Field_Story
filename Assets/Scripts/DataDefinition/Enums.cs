public class Enums
{
    // 季节
    public enum Season
    {
        Spring, Summer, Autumn, Winter
    }

    // 物品种类
    public enum ItemType
    {
        // 种子，商品，家具，除草工具，砍树工具，破坏工具等一系列工具
        Seed, Commodity, Furniture, HoeTool, ChopTool, BreakTool, ReapTool, WaterTool, CollectTool
    }

    public enum SlotType
    {
        // 背包，箱子，商店
        Bag,
        Box,
        Shop
    }

    public enum BagLocation
    {
        Player,
        Box
    }
}
