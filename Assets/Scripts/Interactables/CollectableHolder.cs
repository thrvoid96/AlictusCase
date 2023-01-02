using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHolder : MonoBehaviour
{
    public List<Collectable> currentCollectables;
    public bool isAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Collectable>(out var collectable))
        {
            if (isAI)
            {
                collectable.SwitchLayers(LevelManager.Instance.aiLayer);
            }
            else
            {
                collectable.SwitchLayers(LevelManager.Instance.playerLayer);
            }
            
            currentCollectables.Add(collectable);
            collectable.isBeingHeld = true;
            CollectableSpawner.Instance.RemoveFromList(collectable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Collectable>(out var collectable))
        {
            if (!collectable.isCollected)
            {
                collectable.SwitchLayers(LevelManager.Instance.defaultLayer);
                CollectableSpawner.Instance.AddToList(collectable);
            }
            currentCollectables.Remove(collectable);
            collectable.isBeingHeld = false;

            if (currentCollectables.Count == 0)
            {
                EventManager.Instance.aiCartEmptiedEvent.Invoke();
            }
        }
    }
}
