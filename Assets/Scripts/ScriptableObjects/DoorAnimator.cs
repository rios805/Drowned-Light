using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorAnimator : MonoBehaviour
{
    public enum OpenDirection {Down, Up}

    public OpenDirection direction = OpenDirection.Down; 
    
    public Animator animator;

    // caching the hash for efficiency
    private static readonly int OpenDownHash = Animator.StringToHash("OpenDown");
    private static readonly int OpenUpHash = Animator.StringToHash("OpenUp");
    private static readonly int CloseDownHash = Animator.StringToHash("CloseDown");

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
        }
    }

    public void CloseDoor()
    {
        animator.SetTrigger(CloseDownHash);
    }
}
