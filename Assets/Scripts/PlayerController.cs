using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : Actor
{
    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float smoothMoveTime = 0.1f;
    [SerializeField] private float turnSpeed = 8f;
    
    private float angle;
    private float smoothInputMagnitude;
    private float inputMagnitude;
    private Vector3 velocity;
    private Joystick joystick;

    private void Start()
    {
        joystick = Joystick.Instance;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
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

        UpdatePosAndRot(rb.position, rb.rotation);
    }
    
    public override void SetupValues(LevelData levelData)
    {
        base.SetupValues(levelData);
        
        moveSpeed = levelData.moveSpeed;
        smoothMoveTime = levelData.smoothMoveTime;
        turnSpeed = levelData.turnSpeed;
        transform.localScale = Vector3.one * levelData.playerScale;
        holderModel.gameObject.transform.localScale = transform.localScale;
        holderTrigger.gameObject.transform.localScale = transform.localScale;
    }
    
    public override void KillActor()
    {
        base.KillActor();
        
        velocity = Vector3.zero;
        smoothInputMagnitude = 0f;
        inputMagnitude = 0f;
        angle = 0f;
        
        rb.drag = 1f;
        rb.angularDrag = 1f;
    }

    protected override void Respawn()
    {
        base.Respawn();
        transform.rotation = Quaternion.Euler(Vector3.zero);
        
        rb.drag = 100f;
        rb.angularDrag = 100f;
    }
    
}
