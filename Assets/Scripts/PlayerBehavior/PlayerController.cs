using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform cameraTransform;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D
        float moveZ = Input.GetAxis("Vertical");   // W/S

        // Get the camera's forward and right directions, but flatten the Y axis
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Combine directions based on input
        Vector3 moveDir = (forward * moveZ + right * moveX).normalized;

        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
    }
}
