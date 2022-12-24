using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    public List<CustomEvent> availableEvents;
    private Dictionary<string, UnityEvent> eventsDictionary = new Dictionary<string, UnityEvent>();

    [Serializable]
    public class CustomEvent
    {
        public string eventName;
        public UnityEvent unityEvent;
    }

    private void Start()
    {
        foreach (CustomEvent custEvent in availableEvents)
        {
            eventsDictionary.Add(custEvent.eventName,custEvent.unityEvent);
        }
    }

    public void RegisterToEvent(string eventName, UnityAction callBack)
    {
        if (eventsDictionary.ContainsKey(eventName))
        {
            eventsDictionary[eventName].AddListener(callBack);
        }
    }

    public void RemoveFromEvent(string eventName, UnityAction callBack)
    {
        if (eventsDictionary.ContainsKey(eventName))
        {
            eventsDictionary[eventName].RemoveListener(callBack);
        }
    }

    public void RemoveAllFromEvent(string eventName)
    {
        if (eventsDictionary.ContainsKey(eventName))
        {
            eventsDictionary[eventName].RemoveAllListeners();
        }
    }

    public void TriggerEvent(string eventName)
    {
        if (eventsDictionary.ContainsKey(eventName))
        {
            eventsDictionary[eventName].RemoveAllListeners();
        }
    }
}
