using System;
using System.Collections;
using System.Collections.Generic;
using MVC.Controllers;
using UnityEngine;

public class Level : Singleton<Level>
{
    public LevelData levelData;
    public void Start()
    {
        SetupLevel(levelData);
    }

    private void SetupLevel(LevelData levelData)
    {
        this.levelData = levelData;

        if (!levelData.hasAI)
        {
            AIController.Instance.transform.parent.gameObject.SetActive(false);
        }
        
        RootController.Instance.SetupFailPanel(levelData);
        RootController.Instance.SetupVictoryPanel(levelData);
        
        RootController.Instance.SetupTopPanel(levelData);
        PlayerController.Instance.SetupPlayerValues(levelData);
        AIController.Instance.SetupAgentStats(levelData);
        CollectableSpawner.Instance.SetupSpawnerStats(levelData);

        if (!levelData.isRandomCollectLevel)
        {
            LevelGeneratorImage.Instance.GenerateLevel();
        }
    }
}
