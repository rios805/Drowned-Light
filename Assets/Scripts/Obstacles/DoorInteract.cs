using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractbleItem
{
    [SerializeField] private KeyItemSO requiredKey;
    private DoorController door;

    private void Awake()
    {
        door = GetComponent<DoorController>();
        if (door == null)
            UnityEngine.Debug.LogWarning("DoorController not found on this object!");
    }
    
    public void OnPlayerInteract()
    {
        Inventory inv = Player.Instance.GetComponent<Inventory>();

        if (inv != null && inv.HasItem(requiredKey))
        {
            Debug.Log("Correct key used. Door unlocked!");
            if (door != null)
            {
                door.OpenDoor();
            }
            else
            {
                Debug.LogWarning("DoorController not assigned");
            }   
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

