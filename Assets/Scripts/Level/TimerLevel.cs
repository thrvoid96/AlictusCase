using System.Collections;
using System.Collections.Generic;
using MVC.Controllers;
using UnityEngine;

public class TimerLevel : BaseLevel
{
    public override void SetupLevel(LevelData levelData)
    {
        base.SetupLevel(levelData);
        this.levelData = levelData;

        if (!levelData.hasAI)
        {
            AIController.Instance.transform.parent.gameObject.SetActive(false);
        }
        
        RootController.Instance.SetupTopPanel(levelData);
        RootController.Instance.SetupFailPanel(levelData);
        RootController.Instance.SetupVictoryPanel(levelData);
        
        PlayerController.Instance.SetupPlayerValues(levelData);
        AIController.Instance.SetupAgentStats(levelData);
    }
}
