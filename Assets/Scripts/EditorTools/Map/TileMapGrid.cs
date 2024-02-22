using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class TileMapGrid : MonoBehaviour
{
    private Tilemap currentTileMap;

    public MapDataList MapDataLists;
    public Enums.TileType TileType;

    private void OnEnable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTileMap = GetComponent<Tilemap>();
            if (MapDataLists != null)
            {
                MapDataLists.TileDatas.Clear();
            }
        }
    }

    private void OnDisable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTileMap = GetComponent<Tilemap>();
            UpdateTileData();
#if UNITY_EDITOR
            if (MapDataLists != null)
            {
                EditorUtility.SetDirty(MapDataLists);
            }
#endif
        }
    }

    private void UpdateTileData()
    {
        currentTileMap.CompressBounds();
        if (!Application.IsPlaying(this))
        {
            if (MapDataLists != null)
            {
                Vector3Int startPos = currentTileMap.cellBounds.min;
                Vector3Int endPos = currentTileMap.cellBounds.max;
                for (int x = startPos.x; x < endPos.x; x++)
                {
                    for (int y = startPos.y; y < endPos.y; y++)
                    {
                        TileBase tileBase = currentTileMap.GetTile(new Vector3Int(x, y, 0));
                        if (tileBase != null)
                        {
                            Data.TileDatas tileData = new Data.TileDatas
                            {
                                TilePos = new Vector2Int(x, y),
                                TileType = TileType,
                                IsUsed = true
                            };
                            MapDataLists.TileDatas.Add(tileData);
                        }
                    }
                }
            }
        }
    }
}