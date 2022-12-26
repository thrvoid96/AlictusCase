using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectableSpawner : Singleton<CollectableSpawner>
{
    public float spawnRate;
    public int maxAmount;

    private BoxCollider cubeSpawnArea;
    public List<Collectable> availableCollectables;

    private List<Material> collectableMats;

    private void Awake()
    {
        cubeSpawnArea = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        collectableMats = LevelManager.Instance.getcollectableMats;
    }
    
    private void OnEnable()
    {
        EventManager.Instance.levelStartEvent.AddListener(StartSpawn);
    }

    public void StartSpawn()
    {
        DOVirtual.DelayedCall(spawnRate, SpawnCollectable).SetLoops(-1,LoopType.Restart);
    }
    
    public Vector3 RandomPointInBounds() {
        return new Vector3(
            Random.Range(cubeSpawnArea.bounds.min.x, cubeSpawnArea.bounds.max.x),
            Random.Range(cubeSpawnArea.bounds.min.y, cubeSpawnArea.bounds.max.y),
            Random.Range(cubeSpawnArea.bounds.min.z, cubeSpawnArea.bounds.max.z)
        );
    }

    private void SpawnCollectable()
    {
        if (availableCollectables.Count == maxAmount)
        {
            return;
        }
        
        var spawnedCollectable = ObjectPool.Instance.SpawnFromPool("Cube",RandomPointInBounds(), Random.rotation, transform).GetComponent<Collectable>();
        spawnedCollectable.SetMaterial(collectableMats[Random.Range(0,collectableMats.Count)]);
        availableCollectables.Add(spawnedCollectable);
    }

    public void CollectableCollected(Collectable collectable)
    {
        availableCollectables.Remove(collectable);
    }
}
