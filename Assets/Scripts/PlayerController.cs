using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private Rigidbody rb;
    [SerializeField] private Rigidbody holder,trigger;

    public float moveSpeed = 7;
    public float smoothMoveTime = 0.1f;
    public float turnSpeed = 8f;
    
    private float angle;
    private float smoothInputMagnitude;
    private float inputMagnitude;
    private Vector3 velocity;
    private Joystick joystick;
    public CollectArea collectArea;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        joystick = Joystick.Instance;
    }
    
    void FixedUpdate()
    {
        if (LevelManager.gamestate == GameState.Gameplay)
        {
            Vector3 inputDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            inputMagnitude = new Vector2(joystick.Horizontal, joystick.Vertical).magnitude;
            angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);
            rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
            smoothInputMagnitude = Mathf.Lerp(smoothInputMagnitude, inputMagnitude, smoothMoveTime);
            
            velocity = transform.forward * moveSpeed * smoothInputMagnitude;
            
            rb.MovePosition(rb.position + velocity * Time.deltaTime);

            holder.MovePosition(rb.position);
            holder.MoveRotation(rb.rotation);
            trigger.MovePosition(rb.position);
            trigger.MoveRotation(rb.rotation);
        }
    }

    public void SetupPlayerValues(LevelData levelData)
    {
        moveSpeed = levelData.moveSpeed;
        smoothMoveTime = levelData.smoothMoveTime;
        turnSpeed = levelData.turnSpeed;
        transform.localScale = Vector3.one * levelData.playerScale;
    }
}
