using System;
using System.Collections;
using System.Collections.Generic;
using MVC.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class FirstInputListener : Singleton<FirstInputListener>,IPointerDownHandler
{
    private void OnEnable()
    {
        EventManager.Instance.levelStartEvent.AddListener(GameStart);
    }
    
    public void GameStart()
    {
        RootController.Instance.DisengageController(RootController.ControllerTypeEnum.TapToBeginPanel);
        LevelManager.gamestate = GameState.Gameplay;
        enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        EventManager.Instance.levelStartEvent?.Invoke();
    }
}
