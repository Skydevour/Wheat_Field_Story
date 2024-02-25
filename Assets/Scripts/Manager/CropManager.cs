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
    }

    private void OnDisable()
    {
        EventCenter.StopListenToEvent<AfterSceneLoadEvent>(OnAfterSceneLoadEvent);
        EventCenter.StopListenToEvent<GameDayEvent>(OnGameDayEvent);
    }

    public Data.CropDetails GetCropDetailsByID(int cropID)
    {
        return CropDataList.CropDetailsList.Find(crop => crop.SeedID == cropID);
    }

    public bool CanSeasonPlant(Data.CropDetails crop)
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

    private void OnAfterSceneLoadEvent(AfterSceneLoadEvent evt)
    {
        currentGrid = FindObjectOfType<Grid>();
        cropParent = GameObject.FindWithTag("CropParent").transform;
    }

    private void OnGameDayEvent(GameDayEvent evt)
    {
        currentSeason = evt.Season;
    }
}