using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class AIController : Actor
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private int amountToReturn;
    private Vector3 offset = new Vector3(0f,-0.3f,0f);
    
    private Tweener followTween;

    private Collectable targetCollectable;

    private void Start()
    {
        EventManager.Instance.levelStartEvent.AddListener(StartCollecting);
        EventManager.Instance.levelFailEvent.AddListener(StopAI);
        EventManager.Instance.levelWinEvent.AddListener(StopAI);
        EventManager.Instance.aiCartEmptiedEvent.AddListener(StartCollecting);
        
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

    public override UnityEvent GetCollectEvent()
    {
        return EventManager.Instance.aiCollectedEvent;
    }

    public void StartCollecting()
    {
        followTween?.Kill();
        float randomVal = 0f;
        followTween = DOTween.To(() => randomVal, x => randomVal = x, 1f, 1f).OnUpdate(() =>
        {
            GoToRandomCollectable();
        }).SetLoops(-1, LoopType.Restart);
    }

    private void GoToRandomCollectable()
    {
        // If the AI is holding too many collectibles, drop them and return

        if (collectableHolder.currentCollectables.Count >= amountToReturn)
        {
            navMeshAgent.destination = transform.parent.position;
            targetCollectable = null;
            followTween.Kill();
            return;
        }
        
        if (targetCollectable == null)
        {
            targetCollectable = CollectableSpawner.Instance.getAvailableCollectablesList[Random.Range(0, CollectableSpawner.Instance.getAvailableCollectablesList.Count)];
        }
        else if (targetCollectable.isBeingHeld)
        {
            targetCollectable = null;
            return;
        }
        
        // Set the destination of the NavMeshAgent to be the position of the closest collectible
        navMeshAgent.destination = targetCollectable.transform.position;
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
    

    private void StopAI(List<Actor> allActors)
    {
        navMeshAgent.enabled = false;
    }
}
