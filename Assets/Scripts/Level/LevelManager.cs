using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using DG.Tweening;
using MVC.Controllers;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    public static GameState gamestate = GameState.BeforeStart;

    public List<Color> collectableColors;
    [SerializeField] private GameData gameData;
    public LayerMask goldenLayer, defaultLayer;
    
    public GameData getData => gameData;
    [NonSerialized] public LevelAssetCreate levelAsset;
    [SerializeField] private List<Actor> actorsInScene;

    public List<Actor> getActorsInScene => actorsInScene;

    private void Awake()
    {
        SetValues();
    }

    private void Start()
    {
        EventManager.Instance.levelCompleteEvent.AddListener(FinishLevel);
    }

    //--------------------------------------------------------------------------//
    void SetValues()
    {
        levelAsset = Resources.Load<LevelAssetCreate>("Scriptables/LevelAsset");
        collectableColors = levelAsset.collectableColors;
        actorsInScene = FindObjectsOfType<Actor>().ToList();
        //LoadData();
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
            gameData.RandomLevel = Random.Range(1, levelAsset.levelPrefabs.Count + 1);
            Instantiate(levelAsset.levelPrefabs[gameData.RandomLevel - 1]);
        }
    }

    private void FinishLevel()
    {
        DOTween.KillAll();

        bool playerWins = true;
        for (int i = 1; i < actorsInScene.Count; i++)
        {
            if (actorsInScene[0].getScore <= actorsInScene[i].getScore)
            {
                playerWins = false;
            }
        }
        
        if (playerWins)
        {
            LevelVictory();
        }
        else
        {
            LevelFail();
        }
    }

    private void LevelVictory()
    {
        gamestate = GameState.Victory;
        EventManager.Instance.levelWinEvent.Invoke(actorsInScene);
        RootController.Instance.SwitchToController(RootController.ControllerTypeEnum.VictoryPanel);
    }

    private void LevelFail()
    {
        gamestate = GameState.Fail;
        EventManager.Instance.levelFailEvent.Invoke(actorsInScene);
        RootController.Instance.SwitchToController(RootController.ControllerTypeEnum.FailPanel);
    }

    public void ChangeLevel(int levelToSet)
    {
        gameData.Level = levelToSet;
        //SaveData();
        DOVirtual.DelayedCall(0.1f, ReloadScene);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
