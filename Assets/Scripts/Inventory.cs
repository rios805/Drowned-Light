using UnityEngine;

public class Inventory : MonoBehaviour
{
     public bool hasKey = false;

    public void PickUpKey()
    {
        hasKey = true;
        Debug.Log("Key collected!");
    }
}
