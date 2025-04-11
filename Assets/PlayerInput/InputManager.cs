using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance{get; private set;}
    // playerInput variable
    private PlayerInput playerInput;

    private void Awake() {
        // get playerInput component from player
        playerInput = new PlayerInput();
        Instance = this;
    }

    private void OnEnable() {
        playerInput.Enable();
    }

    private void OnDisable() {
        playerInput.Disable();
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

    public bool PlayerInput_Interact() {
        return playerInput.Player.Interact.triggered;
    }

    public bool PlayerInput_AlternateInteract() {
        return playerInput.Player.AlternateInteract.triggered;
    }

    public bool PlayerInput_Crouch() {
        return playerInput.Player.Crouch.triggered;
    }
}
