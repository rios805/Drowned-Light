using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private float swingAngle = 90f;         // Degrees to swing open
    [SerializeField] private float swingSpeed = 90f;         // Degrees per second
    [SerializeField] private bool swingClockwise = true;     // Direction of swing

    private bool isOpen = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    private void Start()
    {
        initialRotation = transform.rotation;

        float angle = swingClockwise ? -swingAngle : swingAngle;
        targetRotation = Quaternion.Euler(0f, angle, 0f) * initialRotation;
    }

    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;
        StartCoroutine(SwingOpen());
    }

    private System.Collections.IEnumerator SwingOpen()
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, swingSpeed * Time.deltaTime);
            yield return null;
        }
    }
}


