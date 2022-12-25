using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectArea : MonoBehaviour
{
    public List<Collectable> collectedObjects;
    public float scaleIncrease;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Collectable>(out var collectable))
        {
            if (!collectedObjects.Contains(collectable))
            {
                collectable.SwitchLayers(LevelManager.Instance.goldenLayer);
                collectable.SetMaterial(LevelManager.Instance.getGoldenMat);
                collectable.ConnectSpringJointTo(rb);
                collectable.isCollected = true;
                collectedObjects.Add(collectable);
                transform.localScale += (Vector3.one * scaleIncrease);
                CollectableSpawner.Instance.CollectableCollected(collectable);
            }
        }
    }
}
