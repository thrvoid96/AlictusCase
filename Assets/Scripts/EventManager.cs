using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    public UnityEvent levelStartEvent,
        levelCompleteEvent,
        chooseLevelButtonEvent,
        nextLevelButtonEvent,
        retryLevelButtonEvent,
        playerCollectedEvent,
        aiCollectedEvent,
        aiCartEmptiedEvent;

    public UnityEvent<List<Actor>> levelWinEvent, levelFailEvent;
}
