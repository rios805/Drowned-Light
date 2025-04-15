using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public GameObject door; 
    private bool playerInZone = false;
    private GameObject player;

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            Inventory inv = player.GetComponent<Inventory>();
            if (inv != null && inv.hasKey)
            {
                Debug.Log("Door unlocked!");

                Destroy(door);           // Destroy the visible door
                Destroy(gameObject); // Destroy the trigger object (DoorTrigger)
            }
            else
            {
                Debug.Log("You need a key...");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            player = other.gameObject;
            Debug.Log("Entered door interaction zone.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            player = null;
            Debug.Log("Exited door interaction zone.");
        }
    }
}

