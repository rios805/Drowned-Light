using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<KeyItemSO> collectedItems = new List<KeyItemSO>();
    public Dictionary<KeyItemSO, int> itemCounts = new Dictionary<KeyItemSO, int>();

    public void AddItem(KeyItemSO item)
    {
        if (itemCounts.ContainsKey(item))
        {
            itemCounts[item]++;
        }
        else
        {
            itemCounts[item] = 1;
        }

        // Ensure it's shown in the inventory UI
        if (!collectedItems.Contains(item))
        {
            collectedItems.Add(item);
        }

        Debug.Log($"Added item: {item.itemName} (x{itemCounts[item]})");
    }

    public void RemoveItem(KeyItemSO item)
    {
        if (itemCounts.ContainsKey(item))
        {
            itemCounts[item]--;
            if (itemCounts[item] <= 0)
            {
                itemCounts.Remove(item);
                collectedItems.Remove(item);
            }
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
