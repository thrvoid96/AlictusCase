using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class Actor : MonoBehaviour, IValueSetter, IDeactivateable,ILayerSwitch
{
    [SerializeField] protected Rigidbody holderModel, holderTrigger;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected LayerMask layerToSwitchCollectables;
    
    public CollectArea collectArea;
    public CollectableHolder collectableHolder;

    protected bool isDead;
    
    public int getScore => collectArea.getCollectedCount;
    
    protected virtual void FixedUpdate()
    {
        
    }

    protected void UpdatePosAndRot(Vector3 targetPos, Quaternion targetRot)
    {
        holderModel.MovePosition(targetPos);
        holderModel.MoveRotation(targetRot);
        holderTrigger.MovePosition(targetPos);
        holderTrigger.MoveRotation(targetRot);
    }
    
    public virtual void SetupValues(LevelData levelData)
    {
        
    }

    public virtual UnityEvent GetCollectEvent()
    {
        return null;
    }

    public virtual void KillActor()
    {
        if (!isDead)
        {
            isDead = true;
            
            holderModel.gameObject.GetComponent<Collider>().enabled = false;
            holderTrigger.gameObject.GetComponent<Collider>().enabled = false;
            
            rb.useGravity = true;
            
            rb.AddExplosionForce(500f,transform.position + new Vector3(Random.Range(-1f,1f), -1f, Random.Range(-1f,1f)),10f);
            rb.AddTorque(new Vector3(Random.Range(500f,1000f),Random.Range(500f,1000f),Random.Range(500f,1000f)));
            
            for (int i = 0; i < collectableHolder.currentCollectables.Count; i++)
            {
                collectableHolder.currentCollectables[i].rb.AddExplosionForce(250f,transform.position + Vector3.down,10f);
                collectableHolder.currentCollectables[i].SwitchLayers(LevelManager.Instance.defaultLayer);
            }
            
            collectableHolder.currentCollectables.Clear();
            
            DOVirtual.DelayedCall(2f, Respawn);
            
        }
    }

    protected virtual void Respawn()
    {
        transform.position = transform.parent.position;
        rb.useGravity = false;
        
        DOVirtual.DelayedCall(0.1f, ColliderDelay);
    }

    private void ColliderDelay()
    {
        isDead = false;
        holderModel.gameObject.GetComponent<Collider>().enabled = true;
        holderTrigger.gameObject.GetComponent<Collider>().enabled = true;
    }

    public void SetObjectActivity(bool value)
    {
        transform.parent.gameObject.SetActive(value);
    }

    public LayerMask GetLayerToSwitch()
    {
        return layerToSwitchCollectables;
    }
}
