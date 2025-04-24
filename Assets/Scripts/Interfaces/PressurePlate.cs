using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PressurePlate : MonoBehaviour
{
    public string requiredID;

    [Header("Plate Events")]
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    private HashSet<GameObject> _objects = new HashSet<GameObject>();

    private void Reset()
    {
        var c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var push = other.GetComponent<Pushable>();

        if (push == null || push.pushID != requiredID)
        {
            return;
        }

        if (_objects.Add(push.gameObject) && _objects.Count == 1)
        {
            onPressed?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var push = other.GetComponent<Pushable>();
        if (push == null || push.pushID != requiredID)
        {
            return;
        }

        if (_objects.Remove(push.gameObject) && _objects.Count == 0)
        {
            onReleased?.Invoke();
        }
    }
}