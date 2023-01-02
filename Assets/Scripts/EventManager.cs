using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    public UnityEvent levelStartEvent,
        levelWinEvent,
        levelFailEvent,
        levelCompleteEvent,
        chooseLevelButtonEvent,
        nextLevelButtonEvent,
        retryLevelButtonEvent,
        aiCartEmptiedEvent;
}
