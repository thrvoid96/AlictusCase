using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectableSpawner : Singleton<CollectableSpawner>
{
    public float spawnRate;
    public int maxAmount;

    public BoxCollider cubeSpawnArea;
    public List<Collectable> availableCollectables;

    private List<Material> collectableMats;

    private void Start()
    {
        collectableMats = LevelManager.Instance.getcollectableMats;
    }

    public void StartSpawn()
    {
        DOVirtual.DelayedCall(spawnRate, SpawnCollectable).SetLoops(-1,LoopType.Restart);
    }
    
    private Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    private void SpawnCollectable()
    {
        if (availableCollectables.Count == maxAmount)
        {
            return;
        }
        
        var spawnedCollectable = ObjectPool.Instance.SpawnFromPool("Cube",RandomPointInBounds(cubeSpawnArea.bounds), Random.rotation, transform).GetComponent<Collectable>();
        spawnedCollectable.SetMaterial(collectableMats[Random.Range(0,collectableMats.Count)]);
        availableCollectables.Add(spawnedCollectable);
    }

    public void CollectableCollected(Collectable collectable)
    {
        availableCollectables.Remove(collectable);
    }
}
