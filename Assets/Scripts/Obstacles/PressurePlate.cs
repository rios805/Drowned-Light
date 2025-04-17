using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private DoorController door;
    [SerializeField] private Animator plateAnimator;

    private bool doorOpened = false;
    private int objectsOnPlate = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable") && other.attachedRigidbody != null)
        {
            objectsOnPlate++;

            if (!doorOpened)
            {
                door.OpenDoor();
                doorOpened = true;
            }

            if (plateAnimator != null)
                plateAnimator.SetBool("isPressed", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pushable") && other.attachedRigidbody != null)
        {
            objectsOnPlate = Mathf.Max(0, objectsOnPlate - 1);

            if (objectsOnPlate == 0 && plateAnimator != null)
                plateAnimator.SetBool("isPressed", false);
        }
    }
}

