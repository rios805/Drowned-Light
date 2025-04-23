using UnityEngine;
using UnityEngine.Events;

public class Pedestal : MonoBehaviour
{

    public string requiredStatueID;

    public Transform placePoint;

    public UnityEvent onPlaced;

    bool placed;

    public bool TryPlace(Pickupable pickup)
    {
        if(placed)
        {
            Debug.Log("There is already a statue placed here");
            return false;
        }
        if (pickup.statueID != requiredStatueID)
        {
            Debug.Log("Incorrect Statue for the pedestal");
            return false;
        }

        placed = true;
        pickup.IsPicked = false;

        // snaps into place
        pickup.transform.SetParent(placePoint);
        pickup.transform.localPosition = Vector3.zero;
        pickup.transform.localRotation = Quaternion.identity;

        // keeps it static
        if (pickup.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        
        onPlaced.Invoke();
        return true;
    }
}
