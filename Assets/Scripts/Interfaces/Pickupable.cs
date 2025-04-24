using UnityEngine;

public class Pickupable : MonoBehaviour
{
    [Tooltip("Must match the pedestals requiredStatueID")]
    public string statueID;

    public bool IsPicked { get; set; }

    Collider col;
    Rigidbody rb;

    public GameObject PickUp(Transform holdPoint)
    {
        IsPicked = true;
        // disable physics so it won’t fall
        if (rb != null)
        {
            rb.isKinematic = true; 
            rb.useGravity = false; 
        }

        if(col != null)
        {
            col.enabled = false; 
        }

        // parent under the holdPoint
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        return gameObject;
    }

    public void Drop()
    {
        IsPicked = false;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true; 
        }

        if(col != null)
        {
            col.enabled = true; 
        }
    }
}
