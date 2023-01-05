using System;
using System.Collections;
using System.Collections.Generic;
using MVC.Controllers;
using UnityEngine;

public class Level : Singleton<Level>
{
    public LevelData levelData;
    [SerializeField] private GameObject obstaclesParent;
    public void Start()
    {
        SetupLevel(levelData);
    }

    private void SetupLevel(LevelData levelData)
    {
        this.levelData = levelData;

        if (!levelData.hasAI)
        {
            for (int i = 0; i < LevelManager.Instance.getActorsInScene.Count; i++)
            {
                LevelManager.Instance.getActorsInScene[Mathf.Clamp(i+1,1,LevelManager.Instance.getActorsInScene.Count - 1)].GetComponent<IDeactivateable>().SetObjectActivity(false);
                LevelManager.Instance.getActorsInScene[i].GetComponent<IValueSetter>().SetupValues(levelData);
            }
        }
        
        if (!levelData.isRandomCollectLevel)
        {
            LevelGeneratorImage.Instance.GenerateLevel();
        }
        
        if (!levelData.hasObstacles)
        {
            obstaclesParent.SetActive(false);
        }
        
        CollectableSpawner.Instance.SetupSpawnerStats(levelData);
        
        RootController.Instance.SetupFailPanel(levelData);
        RootController.Instance.SetupVictoryPanel(levelData);
        RootController.Instance.SetupTopPanel(levelData);
    }
}
