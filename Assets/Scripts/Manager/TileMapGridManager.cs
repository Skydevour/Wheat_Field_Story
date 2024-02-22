using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TileMapGridManager : MonoBehaviour
{
    [SerializeField] private List<MapDataList> mapDataList;

    private Dictionary<string, Data.TileDetails> tileDetailsDict = new Dictionary<string, Data.TileDetails>();

    private Data.TileDetails GetTileDetails(string key)
    {
        if (tileDetailsDict.ContainsKey(key))
        {
            return tileDetailsDict[key];
        }

        return null;
    }

    private void InitTileDetailsDict(MapDataList mapDataList)
    {
        foreach (var tileData in mapDataList.TileDatas)
        {
            Data.TileDetails tileDetails = new Data.TileDetails
            {
                TileGridX = tileData.TilePos.x,
                TileGridY = tileData.TilePos.y
            };
            string key = tileDetails.TileGridX + "," + tileDetails.TileGridY + "," + mapDataList.SceneName;
            if (GetTileDetails(key) != null)
            {
                tileDetails = GetTileDetails(key);
                switch (tileData.TileType)
                {
                    case Enums.TileType.Diggable:
                        tileDetails.CanDig = true;
                        break;
                    case Enums.TileType.DropItem:
                        tileDetails.CanDrop = true;
                        break;
                    case Enums.TileType.PlaceFurniture:
                        tileDetails.CanPlaceFurniture = true;
                        break;
                    case Enums.TileType.NPCObstacle:
                        tileDetails.NPCObstacle = true;
                        break;
                    default:
                        Debug.Log("类型错误");
                        break;
                }

                tileDetailsDict[key] = tileDetails;
            }
            else
            {
                tileDetailsDict.Add(key, tileDetails);
            }
        }
    }
}