using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Cube : Collectable
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public override void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }

    public override void ConnectSpringJointTo(Rigidbody rigidbody)
    {
        var springJoint = gameObject.AddComponent<SpringJoint>();
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.connectedAnchor = Vector3.zero;
        springJoint.spring = 10f;
        springJoint.connectedBody = rigidbody;
    }

    public override void SwitchLayers(LayerMask layerMask)
    {
        gameObject.layer = (int) Mathf.Log(layerMask.value, 2);
    }
    
    public override void onObjectSpawn()
    {
        base.onObjectSpawn();
    }
}
