using UnityEngine;

public class CoinVisual : MonoBehaviour
{
    public SpriteRenderer iconRenderer;
    public PickupItem pickupItem;

    void Start()
    {
        if (pickupItem != null && pickupItem.itemData != null)
        {
            iconRenderer.sprite = pickupItem.itemData.sideA_Icon;
        }
    }
}

