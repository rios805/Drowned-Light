using UnityEngine;

public class MapToggleUI : MonoBehaviour
{
    public GameObject fullMapPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            fullMapPanel.SetActive(!fullMapPanel.activeSelf);
        }
    }
}

