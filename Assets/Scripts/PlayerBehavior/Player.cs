using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Singleton 
    public static Player Instance{ get; private set; }
    
    // Events
    public EventHandler OnPlayerHealthChanged;
    public EventHandler OnPlayerStaminaChanged;
    public EventHandler OnPlayerSanityChanged;
    public EventHandler OnPlayerKilled;
    
    
    // Editable values
    private Vector3 playerVelocity;
    [Header("Movement Settings")]
    [SerializeField] private float defaultPlayerSpeed = 2.0f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float crouchSpeed = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private Transform playerTopPoint;
    
    [Header("Player Settings")]
    [SerializeField] private float startStamina = 100f;
    [SerializeField] private float startSanity = 100f;
    [SerializeField] private int startHealth = 100;
    [SerializeField] private float stamina;
    [SerializeField] private float sanity;
    [SerializeField] private int health;

    [Header("Player UI")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;
    [SerializeField] private Image sanityBar;
    
    [Header("Camera Settings + Input")]
    [SerializeField]private CinemachineCamera virtualCamera;
    [SerializeField]private Transform cameraFollowTransform;
    [SerializeField]private InputManager inputManager;
    [SerializeField]private float mainFOV = 60f;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> footstepClips;
    [SerializeField] private List<AudioClip> damageClips;
    [SerializeField] private float walkStepRate = 0.5f;
    [SerializeField] private float sprintStepRate = 0.3f;
    [SerializeField] private float crouchStepRate = 0.8f;

    [Header("Push Settings")]
    [SerializeField] private float pushStrength = 3f;

    private float footstepTimer = .5f;
    private bool isMoving;
    
    // Needed variables
    private bool groundedPlayer, isCrouched, isSprinting;
    private float playerSpeed,playerTargetHeight, targetFOV;
    private CharacterController controller;
    private Vector3 playerTargetCenter, lastPosition, currentVelocity;
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
        footstepTimer = walkStepRate;
        health = startHealth;
        stamina = startStamina;
        sanity = startSanity;
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
            stamina -= Time.deltaTime * 20f;

            staminaBar.fillAmount = stamina/startStamina; 
            if (stamina < 0.1f) {
                stamina = Mathf.Clamp(stamina, 0f, 100f);
                isSprinting = false;
            }
            OnPlayerStaminaChanged?.Invoke(this, EventArgs.Empty);
        } else {
            isSprinting = false;
            if (stamina < 100f) {
                stamina += Time.deltaTime * 5f;
                stamina = Mathf.Clamp(stamina, 0f, 100f);
                staminaBar.fillAmount = stamina/startStamina; 
                OnPlayerStaminaChanged?.Invoke(this, EventArgs.Empty);
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

        // Change FOV and speed based off if player is standing, sprinting, or crouching
        if (isSprinting || isCrouched) {
            if (isSprinting) {
                targetFOV = 70;
                playerSpeed = sprintSpeed;
            }
            else {
                targetFOV = 50;
                playerSpeed = crouchSpeed;
            }
        } else {
            targetFOV = mainFOV;
            playerSpeed = defaultPlayerSpeed;
            
        }
        // Set camera height and FOV
        controller.height = Mathf.Lerp(controller.height, playerTargetHeight, Time.deltaTime * 5f);
        controller.center = Vector3.Lerp(controller.center, playerTargetCenter, Time.deltaTime * 5f);
        cameraFollowTransform.localPosition = Vector3.Lerp(cameraFollowTransform.localPosition, new Vector3(cameraFollowTransform.localPosition.x,playerTargetCenter.y + cameraOffset,cameraFollowTransform.localPosition.z) , Time.deltaTime * 5f);
        virtualCamera.Lens.FieldOfView = Mathf.Lerp(virtualCamera.Lens.FieldOfView, targetFOV, Time.deltaTime * 3f);
        
        currentVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
        isMoving = controller.isGrounded && currentVelocity.magnitude > .1f;
        if (isMoving) {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f) {
                if (isCrouched)
                    footstepTimer = crouchStepRate;
                else if (isSprinting)
                    footstepTimer = sprintStepRate;
                else
                    footstepTimer = walkStepRate;
                PlayFootstep();
            }
        }
    }

    public void TakeDamage(int damage) {
        if (damageClips.Count > 0) {
            int index = UnityEngine.Random.Range(0, damageClips.Count);
            audioSource.PlayOneShot(damageClips[index]);
        }
        
        health -= damage;

        healthBar.fillAmount = health/startHealth; 
        if (health <= 0f) {
            OnPlayerKilled?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 0f;
        }
        OnPlayerHealthChanged?.Invoke(this, EventArgs.Empty);
    }
    
    private void PlayFootstep()
    {
        audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        if (footstepClips.Count == 0) return;
        int index = UnityEngine.Random.Range(0, footstepClips.Count);
        audioSource.PlayOneShot(footstepClips[index]);
    }

    public int GetHealth() {
        return health;
    }

    public float GetStamina() {
        return stamina;
    }

    public float GetSanity() {
        return sanity;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        var rb = hit.collider.attachedRigidbody;
        if (rb == null || rb.isKinematic) return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);

        rb.linearVelocity = pushDir * pushStrength;
    }
}