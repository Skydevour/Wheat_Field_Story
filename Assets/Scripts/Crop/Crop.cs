using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    private Data.TileDetails tileDetails;
    private int harvestCount;

    public Data.CropDetails CropDetails;

    public void ProcessTool(Data.ItemDetails tool, Data.TileDetails tile)
    {
        tileDetails = tile;
        int requireCount = CropDetails.GetToolTotalCount(tool.ItemID);
        if (requireCount == -1)
        {
            return;
        }

        if (harvestCount < requireCount)
        {
            harvestCount++;
        }

        if (harvestCount >= requireCount)
        {
            if (CropDetails.SeedGenerateAtPlayer)
            {
            }
        }
    }
}
