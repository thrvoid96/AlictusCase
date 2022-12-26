using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerCollectableHolder"))
        {
            PlayerController.Instance.KillPlayer();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("AICollectableHolder"))
        {
            AIController.Instance.KillAI();
        }
    }
}
