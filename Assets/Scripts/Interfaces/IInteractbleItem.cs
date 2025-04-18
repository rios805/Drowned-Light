using UnityEngine;

public interface IInteractbleItem
{
    public void OnPlayerInteract();
    public void OnPlayerInteractAlternate();
    public void HasOwner();
}
