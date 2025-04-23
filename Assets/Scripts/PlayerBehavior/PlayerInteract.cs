using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interact Settings")]
    public float interactDistance = 3f; 
    public float sphereCastRadius = 0.5f;
    public LayerMask interactLayer;
    public Transform holdPoint; 

   // private float dropForce = 2f; 

    GameObject heldObject; 

    void Start()
    {
        var cam = Camera.main; 
        if(cam != null && holdPoint != null)
        {
            Vector3 worldPos = holdPoint.position; 
            Quaternion worldRot = holdPoint.rotation; 

            holdPoint.SetParent(cam.transform);

            holdPoint.position = worldPos;
            holdPoint.rotation = worldRot;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            Debug.Log("E PRESSED!");
            if(heldObject == null)
            {
                TryPickUp();
            }
            else
            {
                DropItem(); 
            }
        }

        else if(Input.GetMouseButtonDown(0) && heldObject != null)
        {
            Debug.Log("Left Mouse Click!");
            TryPlace();
        }
    }

    void TryPickUp()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
                            out RaycastHit hit, interactDistance, interactLayer))
        {
            var pick = hit.collider.GetComponent<Pickupable>();
            if (pick != null && !pick.IsPicked){
                heldObject = pick.PickUp(holdPoint);
            }
        }
    }

    void TryPlace()
    {
        var cam = Camera.main;
        if(cam == null)
        {
            return;
        }

        if (Physics.SphereCast(cam.transform.position, sphereCastRadius, cam.transform.forward,
                            out RaycastHit hit, interactDistance))
        {
            var ped = hit.collider.GetComponent<Pedestal>();
            if (ped != null)
            {
                var pick = heldObject.GetComponent<Pickupable>();
                if (ped.TryPlace(pick)){
                    heldObject = null;
                }
            }
        }
    }

    void DropItem()
    {
        if(heldObject == null)
        {
            return; 
        }

        var pickable = heldObject.GetComponent<Pickupable>();
        pickable.Drop(); 

        heldObject.transform.SetParent(null); 

        if(heldObject.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.useGravity = true;   
        }

        heldObject = null;
    }
}
