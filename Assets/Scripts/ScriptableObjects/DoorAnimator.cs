using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorAnimator : MonoBehaviour
{
    public enum OpenDirection {Down, Up, Side}

    public OpenDirection direction = OpenDirection.Down; 

    public enum CloseDirection {Down, Side}

    public CloseDirection closing = CloseDirection.Down; 
    
    public Animator animator;

    // caching the hash for efficiency
    private static readonly int OpenDownHash = Animator.StringToHash("OpenDown");
    private static readonly int OpenUpHash = Animator.StringToHash("OpenUp");
    private static readonly int OpenSideHash = Animator.StringToHash("OpenSide");
    private static readonly int CloseDownHash = Animator.StringToHash("CloseDown");
    private static readonly int CloseSideHash = Animator.StringToHash("CloseSide");


    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void OpenDoor()
    {
        if (animator == null)
        {
            return;
        }

        switch(direction)
        {
            case OpenDirection.Down:
                animator.SetTrigger(OpenDownHash);
                break;

            case OpenDirection.Up:
                animator.SetTrigger(OpenUpHash);
                break; 
            case OpenDirection.Side:
                animator.SetTrigger(OpenSideHash);
                break;
        }
    }

    public void CloseDoor()
    {
        switch(closing)
        {
            case CloseDirection.Down:
                animator.SetTrigger(CloseDownHash);
                break;
            
            case CloseDirection.Side:
                animator.SetTrigger(CloseSideHash);
                break;
        }
    }
}
