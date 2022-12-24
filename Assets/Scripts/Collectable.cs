using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour,IPooledObject
{
    public virtual void onObjectSpawn()
    {
        
    }
}
