using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour,IPooledObject
{
    public bool isCollected { get; set; }
    public virtual void SetMaterial(Material material)
    {
        
    }

    public virtual void ConnectSpringJointTo(Rigidbody rigidbody)
    {
        
    }

    public virtual void SwitchLayers(LayerMask layerMask)
    {
        
    }
    
    public virtual void onObjectSpawn()
    {
        
    }
}
