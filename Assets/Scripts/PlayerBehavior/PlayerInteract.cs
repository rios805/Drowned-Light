using UnityEngine;
using System;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 3f;
    public LayerMask interactMask;

    private IInteractbleItem currentItem;
    private Transform currentItemTransform;

    private void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnInteractAction += HandleInteract;
        }
        else
        {
            Debug.LogWarning("InputManager not initialized when PlayerInteract started.");
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnInteractAction -= HandleInteract;
        }
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward * interactRange, Color.red);

        currentItem = null;
        currentItemTransform = null;

        if (Physics.SphereCast(ray, 0.5f, out hit, interactRange, interactMask))
        {
            IInteractbleItem item = hit.collider.GetComponent<IInteractbleItem>();
            if (item != null)
            {
                currentItem = item;
                currentItemTransform = hit.transform;
                Debug.Log($"Looking at: {currentItemTransform.name}");
            }
        }
    }

    private void HandleInteract(object sender, EventArgs e)
    {
        if (currentItem != null)
        {
            currentItem.OnPlayerInteract();
        }
    }
}
