using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectableSpawner : Singleton<CollectableSpawner>
{
    public float spawnRate;
    public int maxAmount;

    [SerializeField]private BoxCollider cubeSpawnArea;
    [SerializeField]private List<Collectable> availableCollectables;
    public List<Collectable> getAvailableCollectablesList => availableCollectables;
    
    public void SetupSpawnerStats(LevelData levelData)
    {
        spawnRate = levelData.spawnRate;
        maxAmount = levelData.maxAmount;

        if (levelData.isRandomCollectLevel)
        {
            EventManager.Instance.levelStartEvent.AddListener(StartSpawn);
        }
    }

    private void StartSpawn()
    {
        SpawnCollectable();
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
        spawnedCollectable.SetColor(LevelManager.Instance.collectableColors[Random.Range(1,LevelManager.Instance.collectableColors.Count)]);
        AddToList(spawnedCollectable);
    }

    public void RemoveFromList(Collectable collectable)
    {
        if (availableCollectables.Contains(collectable))
        {
            availableCollectables.Remove(collectable);
        }
        
    }
    
    public void AddToList(Collectable collectable)
    {
        availableCollectables.Add(collectable);
    }
}
