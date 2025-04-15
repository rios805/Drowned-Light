using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Events
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    
    public static InputManager Instance{get; private set;}
    // playerInput variable
    private PlayerInput playerInput;

    private void Awake() {
        // get playerInput component from player
        playerInput = new PlayerInput();
        Instance = this;

        playerInput.Player.Interact.performed += Interact_performed;
        playerInput.Player.AlternateInteract.performed += InteractAlternate_performed;

    }

    private void OnEnable() {
        playerInput.Enable();
    }

    private void OnDisable() {
        playerInput.Disable();
    }
    
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    
    
    // Getters
    public Vector2 GetPlayerMovement() {
        return playerInput.Player.Movement.ReadValue<Vector2>();
    }
    
    public Vector2 GetMouseDelta() {
        // This returns mouse movement
        return playerInput.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerInput_Pause() {
        return playerInput.Player.Pause.triggered;
    }

    public bool PlayerInput_AlternateInteract() {
        return playerInput.Player.AlternateInteract.triggered;
    }

    public bool PlayerInput_Crouch() {
        return playerInput.Player.Crouch.triggered;
    }

    public bool PlayerInput_Sprint() {
        return playerInput.Player.Sprint.triggered;
    }
    
}
