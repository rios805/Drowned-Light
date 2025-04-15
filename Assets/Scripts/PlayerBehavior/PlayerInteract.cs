using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask interactMask;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward * interactRange, Color.red);

        if (Physics.SphereCast(ray, 0.5f, out hit, interactRange, interactMask))
        {
            InteractableItem item = hit.collider.GetComponent<InteractableItem>();

            if (item != null)
            {
                Debug.Log($"Looking at {item.itemName}");

                if (Input.GetKeyDown(interactKey))
                {
                    Debug.Log("Pressed E while looking at item");
                    // Get Inventory from player
                    Inventory inv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
                    if (inv != null && item.itemName == "Key")
                    {
                        inv.PickUpKey();
                        Destroy(hit.collider.gameObject);
                    }
                }
            }
        }
    }
}
