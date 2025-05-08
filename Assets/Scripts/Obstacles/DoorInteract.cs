using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractbleItem
{
    [Header("Key + Range Settings")]
    [SerializeField] private KeyItemSO requiredKey;
    [Tooltip("How close you must be to unlock/open this door")]
    [SerializeField] private float interactRange = 3f;

    private DoorController door;
    private Transform playerT;

    private void Awake()
    {
        door = GetComponent<DoorController>();
        if (door == null)
            Debug.LogWarning("DoorController not found on this object!");

        if (Player.Instance != null)
            playerT = Player.Instance.transform;
    }

    public void OnPlayerInteract()
    {
        // 1) are we still within range?
        if (playerT == null ||
            Vector3.Distance(playerT.position, transform.position) > interactRange)
        {
            Debug.Log("Too far from door to interact.");
            return;
        }

        // 2) do we have the right key?
        var inv = Player.Instance.GetComponent<Inventory>();
        if (inv != null && inv.HasItem(requiredKey))
        {
            Debug.Log("Correct key used. Door unlocked!");
            door?.OpenDoor();
        }
        else
        {
            Debug.Log("You don't have the correct key.");
        }
    }

    // unused, but required by the interface
    public void OnPlayerInteractAlternate() { }
    public void HasOwner() { }
}

