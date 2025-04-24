using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorAnimator : MonoBehaviour
{
    
    public Animator animator;

    // caching the hash for efficiency
    private static readonly int OpenHash = Animator.StringToHash("Open");

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
        animator.SetTrigger(OpenHash);
    }
}
