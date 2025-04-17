using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Singleton 
    public static Player Instance{ get; private set; }
    
    private CharacterController controller;
    // Player defaults
    private Vector3 playerVelocity;
    private float defaultPlayerSpeed = 2.0f;
    private float sprintSpeed = 6f;
    private bool groundedPlayer;
    private bool isCrouched;
    private bool isSprinting;
    private float playerSpeed;
    
    private float gravityValue = -9.81f;
    public Transform playerTopPoint;

    private float stamina = 100f;
    private float sanity = 100f;
    private float health = 100f;
    
    private Transform cameraTransform;
    
    [SerializeField]private InputManager inputManager;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        Instance = this;
        playerSpeed = defaultPlayerSpeed;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Crouching
        if (inputManager.PlayerInput_Crouch()) {
            isCrouched = true;
            controller.height = 1f;
        } else {
            if (isCrouched && !Physics.Raycast(playerTopPoint.transform.position, playerTopPoint.transform.up, 1f)) {
                isCrouched = false;
                controller.height = 2f;
            }
        }
        
        // Player Speed
        if (inputManager.PlayerInput_Sprint() && stamina > 0f) {
            isSprinting = true;
            playerSpeed = sprintSpeed;
            stamina -= Time.deltaTime * 20f;
            if (stamina < 0.1f) {
                isSprinting = false;
            }
        } else {
            isSprinting = false;
            playerSpeed = defaultPlayerSpeed;
            if (stamina < 100f) {
                stamina += Time.deltaTime * 5f;
            }
        }
        
        // Falling
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        
        // Player Movement
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0.0f, movement.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0f) {
            Destroy(gameObject);
        }
    }
    
}