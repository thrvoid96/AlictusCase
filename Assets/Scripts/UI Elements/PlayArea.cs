using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayArea : Singleton<PlayArea>
{
    public void GameStart()
    {
        LevelManager.gamestate = GameState.Gameplay;
        gameObject.SetActive(false);
    }
}
