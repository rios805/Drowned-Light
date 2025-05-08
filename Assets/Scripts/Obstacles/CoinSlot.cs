using UnityEngine;

public class CoinSlot : MonoBehaviour, IInteractbleItem
{
    public KeyItemSO placedCoin;
    public bool isSideAUp = true;

    public SpriteRenderer coinIconRenderer; 

    public bool IsFilled => placedCoin != null;

    public void OnPlayerInteract()
    {
        Inventory inv = Player.Instance.GetComponent<Inventory>();

        if (!IsFilled)
        {
            foreach (var item in inv.collectedItems)
            {
                if (item.IsCoin) 
                {
                    placedCoin = item;
                    inv.RemoveItem(item);
                    isSideAUp = true;

                    UpdateVisual();
                    CoinBoardManager.Instance.CheckBoardState();
                    break;
                }
            }
        }
        else
        {
            // Flip side
            isSideAUp = !isSideAUp;
            UpdateVisual();
            CoinBoardManager.Instance.CheckBoardState();
        }
    }

    public void OnPlayerInteractAlternate()
    {
        if (IsFilled)
        {
            Inventory inv = Player.Instance.GetComponent<Inventory>();
            inv.AddItem(placedCoin);
            placedCoin = null;
            isSideAUp = true;

            UpdateVisual();
            CoinBoardManager.Instance.CheckBoardState();
        }
    }

    public string GetVisibleSideID()
    {
        if (!IsFilled) return "";
        return isSideAUp ? placedCoin.sideA_ID : placedCoin.sideB_ID;
    }

    public void ClearSlot()
    {
        placedCoin = null;
        isSideAUp = true;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (coinIconRenderer == null) return;

        if (!IsFilled)
        {
            coinIconRenderer.sprite = null;
        }
        else
        {
            coinIconRenderer.sprite = isSideAUp
                ? placedCoin.sideA_Icon
                : placedCoin.sideB_Icon;
        }
    }

    public void HasOwner()
    {
    }
}

