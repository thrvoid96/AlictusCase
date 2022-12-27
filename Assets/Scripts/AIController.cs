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
    private Rigidbody rb;

    private Tweener followTween;

    private bool isDead;
    
    public int getScore => collectArea.collectedObjects.Count;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        EventManager.Instance.levelStartEvent.AddListener(StartCollecting);
        EventManager.Instance.levelFailEvent.AddListener(StopAI);
        EventManager.Instance.levelWinEvent.AddListener(StopAI);
    }
    
    private void OnDisable()
    {
        EventManager.Instance.levelStartEvent.RemoveListener(StartCollecting);
        EventManager.Instance.levelFailEvent.RemoveListener(StopAI);
        EventManager.Instance.levelWinEvent.RemoveListener(StopAI);
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
        var randomCollectable = CollectableSpawner.Instance.getAvailableCollectablesList
            [Random.Range(0, CollectableSpawner.Instance.getAvailableCollectablesList.Count)];
        navMeshAgent.SetDestination(randomCollectable.transform.position);
    }
    
    public void UpdateScoreText()
    {
        scoreText.text = collectArea.collectedObjects.Count.ToString();
    }
    
    public void KillAI()
    {
        if (!isDead)
        {
            isDead = true;
            
            followTween?.Kill();
            navMeshAgent.enabled = false;
            holder.gameObject.GetComponent<Collider>().enabled = false;
            trigger.gameObject.GetComponent<Collider>().enabled = false;

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddExplosionForce(500f,transform.position + new Vector3(Random.Range(-1f,1f), -1f, Random.Range(-1f,1f)),10f);
            rb.AddTorque(new Vector3(Random.Range(500f,1000f),Random.Range(500f,1000f),Random.Range(500f,1000f)));

            for (int i = 0; i < collectableHolder.currentCollectables.Count; i++)
            {
                collectableHolder.currentCollectables[i].rb.AddExplosionForce(250f,transform.position + Vector3.down,10f);
                collectableHolder.currentCollectables[i].SwitchLayers(LevelManager.Instance.defaultLayer);
            }
            
            collectableHolder.currentCollectables.Clear();

            DOVirtual.DelayedCall(2f, RespawnAI);
        }
    }

    private void RespawnAI()
    {
        transform.position = transform.parent.position;
        transform.rotation = Quaternion.Euler(new Vector3(0f,180f,0f));
        holder.gameObject.GetComponent<Collider>().enabled = true;
        trigger.gameObject.GetComponent<Collider>().enabled = true;
        rb.isKinematic = true;
        rb.useGravity = false;
        navMeshAgent.enabled = true;
            
        StartCollecting();
        DOVirtual.DelayedCall(0.1f, ColliderDelay);
    }

    private void ColliderDelay()
    {
        isDead = false;
        
        holder.gameObject.GetComponent<Collider>().enabled = true;
        trigger.gameObject.GetComponent<Collider>().enabled = true;
    }

    private void StopAI()
    {
        navMeshAgent.enabled = false;
    }
}
