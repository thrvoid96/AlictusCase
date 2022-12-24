using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Instantiate objects into a dictionary in the beginning of the game.

public class ObjectPool : PersistentLazySingleton<ObjectPool>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    private readonly Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private readonly Dictionary<string, Transform> poolParents = new Dictionary<string, Transform>();


    private void OnEnable()
    {
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            // Create child gameobjects so pooled objects will be more organized in the hierarchy
            var poolParent = new GameObject($"{pool.tag} Parent");
            poolParent.transform.parent = gameObject.transform;
            poolParents[pool.tag] = poolParent.transform;
            
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, poolParent.transform, true);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag_, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (!poolDictionary.ContainsKey(tag_))
        {
            Debug.LogError("Pool with tag " + tag_ + " does not exist");
            return null;
        }

        var objToSpawn = poolDictionary[tag_].Peek();

        if (objToSpawn.activeInHierarchy)
        {
            objToSpawn = Instantiate(GetPool(tag_).prefab, poolParents[tag_], true);
        }
        else
        {
            objToSpawn = poolDictionary[tag_].Dequeue();
        }

        objToSpawn.SetActive(true);

        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;
        objToSpawn.transform.SetParent(parent);

        var pooledObj = objToSpawn.GetComponent<IPooledObject>();

        if (pooledObj != null)
        {
            pooledObj.onObjectSpawn();
        }

        poolDictionary[tag_].Enqueue(objToSpawn);

        return objToSpawn;
    }

    public GameObject SpawnFromPool(string tag_)
    {
        if (!poolDictionary.ContainsKey(tag_))
        {
            Debug.LogError("Pool with tag " + tag_ + " does not exist");
            return null;
        }

        var objToSpawn = poolDictionary[tag_].Peek();

        if (objToSpawn.activeInHierarchy)
        {
            objToSpawn = Instantiate(GetPool(tag_).prefab, poolParents[tag_], true);
        }
        else
        {
            objToSpawn = poolDictionary[tag_].Dequeue();
        }

        objToSpawn.SetActive(true);

        var pooledObj = objToSpawn.GetComponent<IPooledObject>();

        if (pooledObj != null)
        {
            pooledObj.onObjectSpawn();
        }

        poolDictionary[tag_].Enqueue(objToSpawn);

        return objToSpawn;
    }

    public void SetEntirePool(string tag_, bool value)
    {
        if (!poolDictionary.ContainsKey(tag_))
        {
            Debug.LogError("Pool with tag" + tag_ + "does not exist");
            return;
        }
        else
        {
            foreach (Pool pool in pools)
            {
                if (pool.tag != tag_)
                {
                    continue;
                }

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject spawnedObj = poolDictionary[tag_].Dequeue();
                    spawnedObj.SetActive(value);
                    poolDictionary[tag_].Enqueue(spawnedObj);
                }
            }
        }
    }

    private Pool GetPool(string tag_)
    {
        return pools.Find(p => p.tag.Equals(tag_));
    }
}