using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerCollectableHolder"))
        {
            PlayerController.Instance.KillPlayer();
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<NavMeshObstacle>().enabled = false;
            DOVirtual.DelayedCall(2f, ResetObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("AICollectableHolder"))
        {
            AIController.Instance.KillAI();
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<NavMeshObstacle>().enabled = false;
            DOVirtual.DelayedCall(2f, ResetObject);
        }
    }


    private void ResetObject()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<NavMeshObstacle>().enabled = true;
    }
}
