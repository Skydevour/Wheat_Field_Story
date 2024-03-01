using System;
using System.Collections;
using System.Collections.Generic;
using CommonFramework.Runtime;
using UnityEngine;

public class CropManager : MonoSingleton<CropManager>
{
    private Transform cropParent;
    private Grid currentGrid;
    private Enums.Season currentSeason;

    public CropDataList CropDataList;

    private void OnEnable()
    {
        EventCenter.StartListenToEvent<AfterSceneLoadEvent>(OnAfterSceneLoadEvent);
        EventCenter.StartListenToEvent<GameDayEvent>(OnGameDayEvent);
        EventCenter.StartListenToEvent<PlantSeedEvent>(OnPlantSeedEvent);
    }

    private void OnDisable()
    {
        EventCenter.StopListenToEvent<AfterSceneLoadEvent>(OnAfterSceneLoadEvent);
        EventCenter.StopListenToEvent<GameDayEvent>(OnGameDayEvent);
        EventCenter.StopListenToEvent<PlantSeedEvent>(OnPlantSeedEvent);
    }

    public Data.CropDetails GetCropDetailsByID(int cropID)
    {
        return CropDataList.CropDetailsList.Find(crop => crop.SeedID == cropID);
    }

    private bool CanSeasonPlant(Data.CropDetails crop)
    {
        for (int i = 0; i < crop.SeedSeasons.Length; i++)
        {
            if (crop.SeedSeasons[i] == currentSeason)
            {
                return true;
            }
        }

        return false;
    }

    private void DisplayCrop(Data.TileDetails tileDetails, Data.CropDetails cropDetails)
    {
        int growthStags = cropDetails.SeedGrowthDays.Length;
        int currentStags = 0;
        int totalDay = cropDetails.TotalGrowthDays;

        // 目的，找出当前是哪一阶段的庄稼，进而展示贴图
        for (int i = growthStags - 1; i >= 0; i--)
        {
            // 从后往前，逐个阶段所需的天数进行比较排除
            if (tileDetails.GrowthDay >= totalDay)
            {
                currentStags = i;
                break;
            }

            totalDay -= cropDetails.SeedGrowthDays[i];
        }

        GameObject cropPrefab = cropDetails.SeedGrowthPrefab[currentStags];
        Sprite cropSprite = cropDetails.SeedGrowthSprite[currentStags];
        Vector3 pos = new Vector3(tileDetails.TileGridX + 0.5f, tileDetails.TileGridY + 0.5f, 0);
        GameObject cropInstance = Instantiate(cropPrefab, pos, Quaternion.identity, cropParent);
        cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = cropSprite;
        cropInstance.GetComponent<Crop>().CropDetails = cropDetails;
    }

    #region EventCenter
    private void OnAfterSceneLoadEvent(AfterSceneLoadEvent evt)
    {
        currentGrid = FindObjectOfType<Grid>();
        cropParent = GameObject.FindWithTag("CropParent").transform;
    }

    private void OnGameDayEvent(GameDayEvent evt)
    {
        currentSeason = evt.Season;
    }

    private void OnPlantSeedEvent(PlantSeedEvent evt)
    {
        Data.CropDetails currentCrop = GetCropDetailsByID(evt.SeedID);
        if (evt.TileDetails.SeedItemID == -1 && currentCrop != null && CanSeasonPlant(currentCrop))
        {
            evt.TileDetails.SeedItemID = evt.SeedID;
            evt.TileDetails.GrowthDay = 0;
            DisplayCrop(evt.TileDetails, currentCrop);
        }
        else if (evt.TileDetails.SeedItemID != -1)
        {
            DisplayCrop(evt.TileDetails, currentCrop);
        }
    }

    #endregion
}