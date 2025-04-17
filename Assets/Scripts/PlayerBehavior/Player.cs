using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Singleton 
    public static Player Instance{ get; private set; }
    
    // Editable values
    private Vector3 playerVelocity;
    [Header("Movement Settings")]
    [SerializeField] private float defaultPlayerSpeed = 2.0f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private Transform playerTopPoint;
    
    [Header("Player Settings")]
    [SerializeField] private float stamina = 100f;
    [SerializeField] private float sanity = 100f;
    [SerializeField] private float health = 100f;
    
    [Header("Camera Settings + Input")]
    [SerializeField]private CinemachineCamera virtualCamera;
    [SerializeField]private Transform cameraFollowTransform;
    [SerializeField]private InputManager inputManager;
    [SerializeField]private float mainFOV = 60f;
    
    // Needed variables
    private bool groundedPlayer, isCrouched, isSprinting;
    private float playerSpeed,playerTargetHeight, targetFOV;
    private CharacterController controller;
    private Vector3 playerTargetCenter;
    private Transform cameraTransform;
    private float cameraOffset = 0.5f;

    private void Awake() {
        Instance = this;
    }
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerSpeed = defaultPlayerSpeed;
        playerTargetHeight = controller.height;
        playerTargetCenter = controller.center;
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        // Check if player is crouching
        if (inputManager.PlayerInput_Crouch()) {
            isCrouched = true;
            playerTargetHeight = 1f;
            playerTargetCenter = new Vector3(0, 0.5f, 0);
        } else {
            if (isCrouched && !Physics.Raycast(playerTopPoint.transform.position, playerTopPoint.transform.up, 1f)) {
                isCrouched = false;
                playerTargetHeight = 2f;
                playerTargetCenter = new Vector3(0, 1f, 0);
            }
        }
        // Check if player is sprinting
        if (inputManager.PlayerInput_Sprint() && stamina > 0f && !isCrouched) {
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
        
        // Gravity check
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        
        // Player X and Z Movement
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0.0f, movement.y);
        
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        
        // Move based off the camera 
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
        
        Vector3 moveDirection = camForward * move.z + camRight * move.x;
        controller.Move(moveDirection * Time.deltaTime * playerSpeed);
        
        // Player Y Movement
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Change FOV based off if player is standing, sprinting, or crouching
        if (isSprinting || isCrouched) {
            if (isSprinting) {
                targetFOV = 70;
            }
            else {
                targetFOV = 50;
            }
        } else {
            targetFOV = mainFOV;
        }
        // Set camera height and FOV
        controller.height = Mathf.Lerp(controller.height, playerTargetHeight, Time.deltaTime * 5f);
        controller.center = Vector3.Lerp(controller.center, playerTargetCenter, Time.deltaTime * 5f);
        cameraFollowTransform.localPosition = Vector3.Lerp(cameraFollowTransform.localPosition, new Vector3(cameraFollowTransform.localPosition.x,playerTargetCenter.y + cameraOffset,cameraFollowTransform.localPosition.z) , Time.deltaTime * 5f);
        virtualCamera.Lens.FieldOfView = Mathf.Lerp(virtualCamera.Lens.FieldOfView, targetFOV, Time.deltaTime * 3f);
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0f) {
            Destroy(gameObject);
        }
    }
    
}