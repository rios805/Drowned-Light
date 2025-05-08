using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private float maxStamina;
    [SerializeField] private float maxSanity;
    [SerializeField] private int maxHealth;
    
    [SerializeField] private float stamina;
    [SerializeField] private float sanity;
    [SerializeField] private int health;
    [SerializeField] private FlashLightController flashLight;
    
    [Header("Camera Settings + Input")]
    [SerializeField]private CinemachineCamera virtualCamera;
    [SerializeField]private Transform cameraFollowTransform;
    [SerializeField]private InputManager inputManager;
    [SerializeField]private float mainFOV = 60f;
    [SerializeField] private LayerMask interactLayerMask;
    
    [Header("UI Stuff")]
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private NoteUI noteUI;
    [SerializeField] private GameOverUI gameOverUI;
    
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
    private float playerSpeed,playerTargetHeight, targetFOV, staminaCoolDown, enemyCheckTimer, tooltipCheckTimers;
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
        audioSource = GetComponent<AudioSource>();
        playerSpeed = defaultPlayerSpeed;
        playerTargetHeight = controller.height;
        playerTargetCenter = controller.center;
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        footstepTimer = walkStepRate;
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
        staminaCoolDown -= Time.deltaTime;
        if (inputManager.PlayerInput_Sprint() && stamina > 0f && !isCrouched && staminaCoolDown < 0f) {
            isSprinting = true;
            stamina -= Time.deltaTime * 20f;
            
            if (stamina < 0.1f) {
                stamina = Mathf.Clamp(stamina, 0f, maxStamina);
                isSprinting = false;
                staminaCoolDown = 4f;
            }
            OnPlayerStaminaChanged?.Invoke(this, EventArgs.Empty);
        } else {
            isSprinting = false;
            if (stamina < maxStamina) {
                stamina += Time.deltaTime * 6f;
                stamina = Mathf.Clamp(stamina, 0f, maxStamina);
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
        enemyCheckTimer -= Time.deltaTime;
        if (enemyCheckTimer <= 0f) {
            GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in allEnemies) {
                bool isVisible = IsEnemyVisible(enemy);
                IEnemy ienemy = enemy.GetComponent<IEnemy>();
                
                if (IsEnemyVisible(enemy)) {
                    //Debug.Log("Enemy is on screen and in FOV: " + enemy.name);
                    LoseSanity(0.25f);
                    ienemy.SeenByPlayer(isVisible);
                }
                else {
                    ienemy.SeenByPlayer(isVisible);
                    if (flashLight.isOn) {
                        GainSanity(0.05f);
                    }
                }
            }
            enemyCheckTimer = .1f;
        }

        if (tooltipCheckTimers <= 0f) {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, 2f)) {
                if (hit.collider != null)
                {
                    tooltipText.text = ChangeTooltip(hit.collider);
                }else
                {
                    tooltipText.text = "";
                }
            }else
            {
                tooltipText.text = "";
            }
        }

        if (inputManager.PlayerInput_Interact() && !noteUI.IsShowing()) {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, 2f, interactLayerMask))
            {
                IInteractbleItem interactable = hit.collider.GetComponent<IInteractbleItem>();
                if (interactable != null)
                {
                    interactable.OnPlayerInteract();
                }
            }
        }
        
    }

    public void TakeDamage(int damage) {
        if (damageClips.Count > 0) {
            int index = UnityEngine.Random.Range(0, damageClips.Count);
            audioSource.PlayOneShot(damageClips[index]);
        }      
        health -= damage;
        
        if (health <= 0f) {
            OnPlayerKilled?.Invoke(this, EventArgs.Empty);
            gameOverUI.OnDeath();
        }
        OnPlayerHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void LoseSanity(float lostSanity) {
        sanity -= lostSanity;
        sanity = Mathf.Clamp(sanity,0f, 100f);
        OnPlayerSanityChanged?.Invoke(this, EventArgs.Empty);
    }

    public void GainSanity(float gainedSanity) {
        sanity += gainedSanity;
        sanity = Mathf.Clamp(sanity,0f, 100f);
        OnPlayerSanityChanged?.Invoke(this, EventArgs.Empty);
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

    public bool IsEnemyVisible(GameObject enemy, float fovAngle = 90f) {
        // Enemy on screen?
        Renderer renderer = enemy.GetComponentInChildren<Renderer>();
        if (renderer == null || !renderer.isVisible)
            return false;
        // In Fov?
        Vector3 toEnemy = (enemy.GetComponent<Collider>().bounds.center - cameraTransform.position).normalized;
        Vector3 viewDir = cameraTransform.forward;
        float dot = Vector3.Dot(viewDir, toEnemy);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 100f, Color.red, 0.5f);
        if (angle <= fovAngle / 2f) {
            // Line of sight raycast
            if (Physics.Raycast(cameraTransform.position, toEnemy, out RaycastHit hit, 100f)) {
                //Debug.Log(hit.collider.gameObject.name);
                return hit.collider.gameObject == enemy;
            }
        }
        return false;
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit) {

        var rb = hit.collider.attachedRigidbody;
        if (rb == null || rb.isKinematic) return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);

        rb.linearVelocity = pushDir * pushStrength;
    }

    private String ChangeTooltip(Collider collider) {
        var tempMonoArray  = collider.gameObject.GetComponents<MonoBehaviour>();
        foreach (var mono in tempMonoArray) {
            if (mono is IToolTip toolTip) {
                return toolTip.ShowToolTip();
            }
        }
        
        return "";
    }
}