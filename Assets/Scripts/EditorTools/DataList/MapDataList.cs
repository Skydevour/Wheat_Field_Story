using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/MapDataList", fileName = "MapDataList")]
public class MapDataList : ScriptableObject
{
    public string SceneName;
    public List<Data.TileDatas> TileDatas;
}