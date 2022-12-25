using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLevel : MonoBehaviour
{
    public LevelData levelData;
    public void Awake()
    {
        SetupLevel(levelData);
    }

    public virtual void SetupLevel(LevelData levelData)
    {
        
    }
}
