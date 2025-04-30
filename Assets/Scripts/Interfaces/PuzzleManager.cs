using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : MonoBehaviour
{
    public DoorAnimator doorAnimator;

    public Pedestal[] pedestals; 
    public PressurePlate[] plates;

    public int requiredStatues = 0;
    public int requiredPlates = 0; 

    private int placedCount = 0;
    private int pressedCount = 0; 

    //private bool isCubeOnPlate = false;

    void Awake()
    {
        if(requiredStatues <= 0 && pedestals != null)
        {
            requiredStatues = pedestals.Length; 
        }

        if(requiredPlates <= 0 && plates != null)
        {
            requiredPlates = plates.Length;
        }
        
    }

    void OnEnable()
    {
        placedCount = 0; 
        pressedCount = 0; 

        foreach(var ped in pedestals)
        {
            ped.onPlaced.AddListener(OnStatuePlaced);
        }

        foreach (var plate in plates)
        {
            plate.onPressed.AddListener(OnPlatePressed);
            plate.onReleased.AddListener(OnPlateReleased);
        }
    }

    void OnDisable()
    {
        foreach (var ped in pedestals)
        {
            ped.onPlaced.RemoveListener(OnStatuePlaced);
        }

        foreach (var plate in plates)
        {
            plate.onPressed.RemoveListener(OnPlatePressed);
            plate.onReleased.RemoveListener(OnPlateReleased);
        }
    }

    private void OnStatuePlaced()
    {
        placedCount++;
        Debug.Log($"Statues placed: {placedCount}/{requiredStatues}");

        if (placedCount >= requiredStatues)
        {
            doorAnimator.OpenDoor();

            foreach(var ped in pedestals)
            {
                ped.onPlaced.RemoveListener(OnStatuePlaced);
            }
        }
    }

    public void OnPlatePressed()
    {
        pressedCount++;
        Debug.Log($"Plates pressed: {pressedCount}/{requiredPlates}");
        if(pressedCount >= requiredPlates)
        {
            doorAnimator.OpenDoor(); 
        }
    }

    public void OnPlateReleased()
    {
        pressedCount--;
        Debug.Log($"Plates pressed: {pressedCount}/{requiredPlates}");
        if(pressedCount < requiredPlates)
        {
            doorAnimator.CloseDoor();
        }
    }
}
