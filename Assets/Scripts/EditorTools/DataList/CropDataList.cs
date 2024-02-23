using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Crop/CropDataList", fileName = "CropDataList")]
public class CropDataList : ScriptableObject
{
    // 数据存成配置表
    public List<Data.CropDetails> CropDetailsList;
}