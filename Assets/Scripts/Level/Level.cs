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
            LevelManager.Instance.aiActor.transform.parent.gameObject.SetActive(false);
        }
        
        RootController.Instance.SetupFailPanel(levelData);
        RootController.Instance.SetupVictoryPanel(levelData);
        
        RootController.Instance.SetupTopPanel(levelData);
        LevelManager.Instance.playerActor.SetupValues(levelData);
        LevelManager.Instance.aiActor.SetupValues(levelData);
        CollectableSpawner.Instance.SetupSpawnerStats(levelData);

        if (!levelData.isRandomCollectLevel)
        {
            LevelGeneratorImage.Instance.GenerateLevel();
        }
    }
}
