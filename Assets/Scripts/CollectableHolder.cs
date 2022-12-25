using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHolder : MonoBehaviour
{
    public List<Collectable> currentCollectables;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Collectable>(out var collectable))
        {
            collectable.SwitchLayers(LevelManager.Instance.playerLayer);
            currentCollectables.Add(collectable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Collectable>(out var collectable))
        {
            if (!collectable.isCollected)
            {
                collectable.SwitchLayers(LevelManager.Instance.defaultLayer);
            }
            currentCollectables.Remove(collectable);
        }
    }
}
