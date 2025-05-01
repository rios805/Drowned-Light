using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractbleItem
{
    [SerializeField] public KeyItemSO itemData;

    public void OnPlayerInteract()
    {
        Debug.Log($"Picked up: {itemData.itemName}");

        Inventory inv = Player.Instance.GetComponent<Inventory>();
        if (inv != null)
        {
            inv.AddItem(itemData);
        }

        Destroy(gameObject);
    }

    public void OnPlayerInteractAlternate()
    {
        Debug.Log($"Alternate interact with: {itemData.itemName}");
    }

    public void HasOwner()
    {
    }
}

