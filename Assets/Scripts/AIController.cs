using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIController : Actor
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    private Vector3 offset = new Vector3(0f,-0.3f,0f);
    
    private Tweener followTween;
    

    private void Start()
    {
        EventManager.Instance.levelStartEvent.AddListener(StartCollecting);
        EventManager.Instance.levelFailEvent.AddListener(StopAI);
        EventManager.Instance.levelWinEvent.AddListener(StopAI);
    }
    
    // private void OnDisable()
    // {
    //     EventManager.Instance.levelStartEvent.RemoveListener(StartCollecting);
    //     EventManager.Instance.levelFailEvent.RemoveListener(StopAI);
    //     EventManager.Instance.levelWinEvent.RemoveListener(StopAI);
    // }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        UpdatePosAndRot(transform.position+offset, transform.rotation);
    }

    public override void SetupValues(LevelData levelData)
    {
        navMeshAgent.speed = levelData.aiSpeed;
        navMeshAgent.angularSpeed = levelData.aiTurnSpeed;
        navMeshAgent.acceleration = levelData.aiAcceleration;
    }

    public void StartCollecting()
    {
        DOVirtual.DelayedCall(0.1f, GoToRandomCollectable);
        
        float randomVal = 0f;
        followTween = DOTween.To(() => randomVal, x => randomVal = x, 1f, 1f).OnStepComplete(() =>
        {
            GoToRandomCollectable();
        }).SetLoops(-1, LoopType.Restart);

    }

    private void GoToRandomCollectable()
    {
        if (collectableHolder.currentCollectables.Count >= 5)
        {
            navMeshAgent.SetDestination(collectArea.transform.position);
            return;
        }

        if (CollectableSpawner.Instance.getAvailableCollectablesList.Count <= 5 && collectableHolder.currentCollectables.Count != 0)
        {
            navMeshAgent.SetDestination(collectArea.transform.position);
            return;
        }
        
        var randomCollectable = CollectableSpawner.Instance.getAvailableCollectablesList
            [Random.Range(0, CollectableSpawner.Instance.getAvailableCollectablesList.Count)];
        navMeshAgent.SetDestination(randomCollectable.transform.position);
    }
    
    public override void KillActor()
    {
        base.KillActor();
        
        followTween?.Kill();
        navMeshAgent.enabled = false;

        rb.isKinematic = false;
    }

    protected override void Respawn()
    {
        base.Respawn();

        transform.rotation = Quaternion.Euler(new Vector3(0f,180f,0f));
        rb.isKinematic = true;
        navMeshAgent.enabled = true;
            
        StartCollecting();
        
    }
    

    private void StopAI()
    {
        navMeshAgent.enabled = false;
    }
}
