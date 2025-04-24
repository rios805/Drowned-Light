using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public DoorAnimator doorAnimator;

    private int requiredStatues = 3;

    private int placedCount;

    void OnEnable()
    {
        foreach(var ped in FindObjectsOfType<Pedestal>())
        {
            ped.onPlaced.AddListener(OnStatuePlaced);
        }
    }

    void OnDisable()
    {
        foreach (var ped in FindObjectsOfType<Pedestal>())
        {
            ped.onPlaced.RemoveListener(OnStatuePlaced);
        }
    }

    private void OnStatuePlaced()
    {
        placedCount++;
        Debug.Log($"Statues placed: {placedCount}/{requiredStatues}");
        if (placedCount >= requiredStatues)
        {
            doorAnimator.OpenDoor();
        }
    }
}
