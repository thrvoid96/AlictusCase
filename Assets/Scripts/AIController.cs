using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIController : Singleton<AIController>
{
    [SerializeField]private NavMeshAgent navMeshAgent;
    [SerializeField] private Rigidbody holder,trigger;
    [SerializeField] private TextMeshProUGUI scoreText;
    public CollectArea collectArea;
    public CollectableHolder collectableHolder;

    private Vector3 offset = new Vector3(0f,-0.3f,0f);
    private void Start()
    {
        EventManager.Instance.levelStartEvent.AddListener(StartCollecting);
    }

    private void FixedUpdate()
    {
        holder.MovePosition(transform.position+offset);
        holder.MoveRotation(transform.rotation);
        
        trigger.MovePosition(transform.position+offset);
        trigger.MoveRotation(transform.rotation);
    }

    public void SetupAgentStats(LevelData levelData)
    {
        navMeshAgent.speed = levelData.aiSpeed;
        navMeshAgent.angularSpeed = levelData.aiTurnSpeed;
        navMeshAgent.acceleration = levelData.aiAcceleration;
    }

    public void StartCollecting()
    {
        DOVirtual.DelayedCall(0.1f, GoToRandomCollectable);
        
        float randomVal = 0f;
        DOTween.To(() => randomVal, x => randomVal = x, 1f, 1f).OnStepComplete(() =>
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
        var randomCollectable = CollectableSpawner.Instance.availableCollectables[Random.Range(0, CollectableSpawner.Instance.availableCollectables.Count)];
        navMeshAgent.SetDestination(randomCollectable.transform.position);
    }
    
    public void UpdateScoreText()
    {
        scoreText.text = collectArea.collectedObjects.Count.ToString();
    }
}
