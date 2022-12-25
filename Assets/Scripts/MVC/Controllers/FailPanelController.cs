using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Controller responsible for game phase.
/// </summary>
public class FailPanelController : SubController<FailPanelView>
{
    public override void EngageController()
    {
        base.EngageController();
    }

    public override void DisengageController()
    {
        base.DisengageController();
    }
    
    /// <summary>
    /// Handling info panel view.
    /// </summary>
    public void SetupView()
    {
        view.Setup();
    }
    
}