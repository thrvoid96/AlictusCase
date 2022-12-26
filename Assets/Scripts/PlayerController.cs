using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : Singleton<PlayerController>
{
    private Rigidbody rb;
    [SerializeField] private Rigidbody holder,trigger;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    public CollectArea collectArea;
    public CollectableHolder collectableHolder;

    public float moveSpeed = 7;
    public float smoothMoveTime = 0.1f;
    public float turnSpeed = 8f;
    
    private float angle;
    private float smoothInputMagnitude;
    private float inputMagnitude;
    private int currentScore;
    private Vector3 velocity;
    private Joystick joystick;
    private bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        joystick = Joystick.Instance;
    }
    
    void FixedUpdate()
    {
        if (LevelManager.gamestate == GameState.Gameplay)
        {
            if (!isDead)
            {
                Vector3 inputDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;
                float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                inputMagnitude = new Vector2(joystick.Horizontal, joystick.Vertical).magnitude;
                angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);
                rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
                smoothInputMagnitude = Mathf.Lerp(smoothInputMagnitude, inputMagnitude, smoothMoveTime);
                
                velocity = transform.forward * moveSpeed * smoothInputMagnitude;
            
                rb.MovePosition(rb.position + velocity * Time.deltaTime);
            }
        }
        
        holder.MovePosition(rb.position);
        holder.MoveRotation(rb.rotation);
        trigger.MovePosition(rb.position);
        trigger.MoveRotation(rb.rotation);
    }

    public void SetupPlayerValues(LevelData levelData)
    {
        moveSpeed = levelData.moveSpeed;
        smoothMoveTime = levelData.smoothMoveTime;
        turnSpeed = levelData.turnSpeed;
        transform.localScale = Vector3.one * levelData.playerScale;
    }

    public void UpdateScoreText()
    {
        scoreText.text = collectArea.collectedObjects.Count.ToString();
    }

    public void KillPlayer()
    {
        if (!isDead)
        {
            isDead = true;
            holder.gameObject.GetComponent<Collider>().enabled = false;
            trigger.gameObject.GetComponent<Collider>().enabled = false;
            
            velocity = Vector3.zero;
            smoothInputMagnitude = 0f;
            inputMagnitude = 0f;
            angle = 0f;

            rb.useGravity = true;
            rb.drag = 1f;
            rb.angularDrag = 1f;
            rb.AddExplosionForce(500f,transform.position + new Vector3(Random.Range(-1f,1f), -1f, Random.Range(-1f,1f)),10f);
            rb.AddTorque(new Vector3(Random.Range(500f,1000f),Random.Range(500f,1000f),Random.Range(500f,1000f)));

            for (int i = 0; i < collectableHolder.currentCollectables.Count; i++)
            {
                collectableHolder.currentCollectables[i].rb.AddExplosionForce(250f,transform.position + Vector3.down,10f);
                collectableHolder.currentCollectables[i].SwitchLayers(LevelManager.Instance.defaultLayer);
            }
            
            collectableHolder.currentCollectables.Clear();

            DOVirtual.DelayedCall(2f, RespawnPlayer);
        }
    }

    private void RespawnPlayer()
    {
        transform.position = transform.parent.position;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        rb.useGravity = false;
        rb.drag = 100f;
        rb.angularDrag = 100f;
        
        DOVirtual.DelayedCall(0.1f, ColliderDelay);
    }

    private void ColliderDelay()
    {
        isDead = false;
        holder.gameObject.GetComponent<Collider>().enabled = true;
        trigger.gameObject.GetComponent<Collider>().enabled = true;
    }
}
