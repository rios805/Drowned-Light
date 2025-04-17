using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<KeyItemSO> collectedItems = new List<KeyItemSO>();

    public void AddItem(KeyItemSO item)
    {
        if (!collectedItems.Contains(item))
        {
            collectedItems.Add(item);
            Debug.Log($"Item added to inventory: {item.itemName}");
        }
    }

    public bool HasItem(string itemName)
    {
        return collectedItems.Exists(i => i.itemName == itemName);
    }

    public bool HasItem(KeyItemSO item)
    {
        return collectedItems.Contains(item);
    }
}
