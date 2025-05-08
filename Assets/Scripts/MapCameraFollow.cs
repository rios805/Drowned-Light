using UnityEngine;

public class MapCameraFollow : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform player;

    [Header("Camera Settings")]
    public float height = 100f;
    public float followSpeed = 10f;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        Camera cam = GetComponent<Camera>();
        if (cam != null)
        {
            cam.orthographic = true;
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPos = new Vector3(player.position.x, height, player.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
    }
}

