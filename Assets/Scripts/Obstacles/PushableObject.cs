using UnityEngine;

public class PushableObject : MonoBehaviour, IInteractbleItem
{
    private Rigidbody rb;
    private bool isBeingPushed = false;

    [SerializeField] private float pushForce = 50f;
    [SerializeField] private float pushDistance = 1.2f; // how close the player has to be

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnPlayerInteract()
    {
        isBeingPushed = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        Debug.Log("Started pushing object");
    }

    public void OnPlayerInteractAlternate()
    {
        isBeingPushed = false;
        Debug.Log("Stopped pushing object");
    }

    public void HasOwner() { }

    private void Update()
    {
        if (isBeingPushed && !Input.GetKey(KeyCode.E))
        {
            OnPlayerInteractAlternate();
        }
    }

  private void FixedUpdate()
{
    if (!isBeingPushed) return;

    Transform player = Player.Instance.transform;
    Vector3 toBox = transform.position - player.position;
    toBox.y = 0f;

    float distance = toBox.magnitude;
    if (distance > pushDistance) return;

    rb.AddForce(toBox.normalized * pushForce * Time.fixedDeltaTime, ForceMode.Impulse);
}

}



