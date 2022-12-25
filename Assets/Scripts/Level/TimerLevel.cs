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
        
        RootController.Instance.SetupTopPanel(levelData);
        PlayerController.Instance.SetupPlayerValues(levelData);
    }
}
