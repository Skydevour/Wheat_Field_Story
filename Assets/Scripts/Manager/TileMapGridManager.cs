using System;
using System.Collections.Generic;
using CommonFramework.Runtime;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapGridManager : MonoSingleton<TileMapGridManager>
{
    [SerializeField] private List<MapDataList> mapDataList;
    [SerializeField] private RuleTile digTile;
    [SerializeField] private RuleTile waterTile;

    private Tilemap digTileMap;
    private Tilemap waterTileMap;
    private Dictionary<string, Data.TileDetails> tileDetailsDict = new Dictionary<string, Data.TileDetails>();
    private Grid currentGrid;

    private void OnEnable()
    {
        EventCenter.StartListenToEvent<AfterSceneLoadEvent>(OnAfterSceneLoadEvent);
    }

    private void OnDisable()
    {
        EventCenter.StopListenToEvent<AfterSceneLoadEvent>(OnAfterSceneLoadEvent);
    }

    private void OnAfterSceneLoadEvent(AfterSceneLoadEvent obj)
    {
        currentGrid = FindObjectOfType<Grid>();
        digTileMap = GameObject.FindWithTag("Dig").GetComponent<Tilemap>();
        waterTileMap = GameObject.FindWithTag("Water").GetComponent<Tilemap>();
    }

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

    /// <summary>
    /// 待补充方法
    /// </summary>
    /// <param name="mouseWorldPos"></param>
    /// <param name="itemDetails"></param>
    private void OnExecuteAfterAnimation(Vector3 mouseWorldPos, Data.ItemDetails itemDetails)
    {
        Data.TileDetails currentTile = new Data.TileDetails();
        switch (itemDetails.ItemType)
        {
            case Enums.ItemType.Commodity:
                break;
            case Enums.ItemType.HoeTool:
                SetDigGround(currentTile);
                currentTile.DaySinceDug = 0;
                currentTile.CanDig = false;
                currentTile.CanDrop = false;
                break;
            case Enums.ItemType.WaterTool:
                SetWaterGround(currentTile);
                currentTile.DaySinceWater = 0;
                break;
        }
    }

    private void SetDigGround(Data.TileDetails tileDetails)
    {
        Vector3Int pos = new Vector3Int(tileDetails.TileGridX, tileDetails.TileGridY, 0);
        if (digTileMap != null)
        {
            digTileMap.SetTile(pos, digTile);
        }
    }

    private void SetWaterGround(Data.TileDetails tileDetails)
    {
        Vector3Int pos = new Vector3Int(tileDetails.TileGridX, tileDetails.TileGridY, 0);
        if (waterTileMap != null)
        {
            waterTileMap.SetTile(pos, waterTile);
        }
    }
}