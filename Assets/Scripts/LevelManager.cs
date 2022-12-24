using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    public static GameState gamestate = GameState.BeforeStart;
    
    [NonSerialized] public LevelAssetCreate levelAsset;
    [SerializeField] private GameData gameData;
    public GameData getData => gameData;
    
    private void Awake()
    {
        SetValues();
    }
    
    //--------------------------------------------------------------------------//
    void SetValues()
    {
        levelAsset = Resources.Load<LevelAssetCreate>("Scriptables/LevelAsset");
        CreateLevel();
    }


    //-------------------------------------------------------------------//
    void CreateLevel()
    {
        if (gameData.Level <= levelAsset.levelPrefabs.Count)
        {
            Instantiate(levelAsset.levelPrefabs[gameData.Level - 1]);
        }
        else
        {
            gameData.RandomLevel = Random.Range(0, levelAsset.levelPrefabs.Count);
            Instantiate(levelAsset.levelPrefabs[gameData.RandomLevel]);
        }
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveData();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveData();
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveData()
    {
        var filename2 = Path.Combine(Application.persistentDataPath, "GameData.json");
        if (!string.IsNullOrEmpty(filename2))
        {
            gameData.DumpJsonToFile(filename2);
        }
    }

    private void LoadData()
    {
        var filename2 = Path.Combine(Application.persistentDataPath, "GameData.json");
        if (!string.IsNullOrEmpty(filename2))
        {
            gameData.LoadJsonFromFile(filename2);
        }
    }
}
