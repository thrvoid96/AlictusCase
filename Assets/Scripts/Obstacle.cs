using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private bool respawnOnDeath;
    
    private MeshRenderer meshRenderer;
    private Collider collider;
    private NavMeshObstacle nmObstacle;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        nmObstacle = GetComponent<NavMeshObstacle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.GetChild(0).TryGetComponent<Actor>(out var actor))
        {
            actor.KillActor();
            meshRenderer.enabled = false;
            collider.enabled = false;
            nmObstacle.enabled = false;
            if (respawnOnDeath)
            {
                DOVirtual.DelayedCall(2f, RespawnObject);
            }
            
        }
    }

    private void RespawnObject()
    {
        meshRenderer.enabled = true;
        collider.enabled = true;
        nmObstacle.enabled = true;
    }
}
