using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private float slideDistance = 3f;
    [SerializeField] private float slideSpeed = 1f;

    private bool isOpen = false;
    private Vector3 targetPosition;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + transform.right * -slideDistance; // slide left
    }

    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;
        StartCoroutine(SlideOpen());
    }

    private System.Collections.IEnumerator SlideOpen()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, slideSpeed * Time.deltaTime);
            yield return null;
        }
    }
}

