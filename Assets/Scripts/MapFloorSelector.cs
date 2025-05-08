using UnityEngine;

public class MapFloorSelector : MonoBehaviour
{
    public Camera mapCamera;

    public LayerMask floor1Layer;
    public LayerMask floor2Layer;
    public LayerMask floor3Layer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            mapCamera.cullingMask = floor1Layer;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            mapCamera.cullingMask = floor2Layer;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            mapCamera.cullingMask = floor3Layer;
    }
}
