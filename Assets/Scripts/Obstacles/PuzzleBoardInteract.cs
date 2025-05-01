using UnityEngine;

public class PuzzleBoardInteract : MonoBehaviour, IInteractbleItem
{
    public void OnPlayerInteract()
    {
        PuzzleCameraController.Instance.EnterPuzzleMode();
    }

    public void OnPlayerInteractAlternate() { }

    public void HasOwner() { }
}

