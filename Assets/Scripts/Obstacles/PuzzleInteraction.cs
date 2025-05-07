using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleInteraction : MonoBehaviour
{
    public LayerMask interactMask;

    void Update()
    {
        if (!PuzzleCameraController.Instance.IsPuzzleActive)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, 5f, interactMask))
            {
                if (hit.collider.TryGetComponent(out IInteractbleItem interactable))
                {
                    Debug.Log("Left-clicked on: " + hit.collider.name);
                    interactable.OnPlayerInteract();
                }
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, 5f, interactMask))
            {
                if (hit.collider.TryGetComponent(out IInteractbleItem interactable))
                {
                    Debug.Log("Right-clicked on: " + hit.collider.name);
                    interactable.OnPlayerInteractAlternate();
                }
            }
        }
    }
}
