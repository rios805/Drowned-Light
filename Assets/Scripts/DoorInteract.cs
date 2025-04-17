using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractbleItem
{
    [SerializeField] private KeyItemSO requiredKey;
    [SerializeField] private GameObject door;

    private bool playerInZone = false;

    public void OnPlayerInteract()
    {

        Inventory inv = Player.Instance.GetComponent<Inventory>();
        if (inv != null && inv.HasItem(requiredKey))
        {
            Debug.Log("Correct key used. Door unlocked!");
            Destroy(door);       
        }
        else
        {
            Debug.Log("You don't have the correct key.");
        }
    }

    public void OnPlayerInteractAlternate()
    {
    }

    public void HasOwner()
    {
    }
}

