using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KeyPickup : MonoBehaviour
{
    [Tooltip("How far the player can be and still pick this up")]
    [SerializeField] private float pickupRange = 3f;

    [Tooltip("Layer the player is on (so we can find distance correctly)")]
    [SerializeField] private LayerMask playerLayer;

    [Tooltip("The ScriptableObject for this key")]
    [SerializeField] private KeyItemSO keyItem;

    private Transform _playerTf;
    private Collider _col;

    void Awake()
    {
        _col = GetComponent<Collider>();
        _col.isTrigger = true;      // we only need a trigger so no physics blocking
        // assume your player has tag “Player”
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) _playerTf = player.transform;
    }

    void Update()
    {
        if (_playerTf == null) return;
        // 1) check distance
        float dist = Vector3.Distance(_playerTf.position, transform.position);
        if (dist > pickupRange) return;

        // 2) check “use” button
        if (Input.GetKeyDown(KeyCode.E))
        {
            GiveToInventory();
        }
    }

    private void GiveToInventory()
    {
        var inv = _playerTf.GetComponent<Inventory>();
        if (inv != null && keyItem != null)
        {
            inv.AddItem(keyItem);
            Debug.Log($"Picked up key “{keyItem.name}”");
        }
        Destroy(gameObject);
    }

    // optional: show a gizmo in the editor so you can tune pickupRange
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
