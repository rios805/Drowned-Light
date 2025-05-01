using UnityEngine;
using UnityEngine.AI;

public class StatueEnemyAI : MonoBehaviour, IEnemy
{
    
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private int damageAmount;
    [SerializeField] private float sightRange, attackRange, attackCooldown, runSpeed;
    [SerializeField] private GameObject attackVisual,idleVisual,seenVisual, currentVisual;
    
    [SerializeField]private Vector3 offset;
    private float lastAttackTime = 0f;
    private bool playerIsLooking, seenOnce;
    
    public Player player;

    private enum EnemyState {
        Idle,
        Seen,
        Chase,
        Attack
    }
    private EnemyState currentState;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Idle;
        currentVisual = seenVisual;
        seenOnce = false;
    }

    private void Update() {
        switch (currentState) {
            case EnemyState.Idle:
                break;
            case EnemyState.Seen:
                break;
            case EnemyState.Chase:
                if (playerIsLooking) {
                    SeenByPlayer(true);
                    return;
                }
                
                Chase();
                break;
            case EnemyState.Attack:
                if (playerIsLooking) {
                    SeenByPlayer(true);
                    return;
                }
                
                Attack();
                break;
            
        }
    }
    
    private bool CheckForPlayer()
    {
        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;

        if (Physics.Raycast(transform.position + offset, dirToPlayer, out RaycastHit hit, sightRange))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }
    
    private void Chase()
    {
        
        agent.isStopped = false;
        agent.speed = runSpeed;
        agent.SetDestination(player.transform.position);
        float distance = Vector3.Distance(transform.position, player.transform.position);
        //animator.Play("Chase");
        if (distance <= attackRange && !playerIsLooking)
        {
            currentState = EnemyState.Attack;
        }
        
        else if (distance > sightRange)
        {
            // Lost the player
        }
    }

    private void Attack()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero; 
        agent.ResetPath();
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > attackRange)
        {
            agent.isStopped = false;
            currentState = EnemyState.Chase;
            return;
        }

        if (Time.time >= lastAttackTime + attackCooldown && !playerIsLooking)
        {
            player.TakeDamage(damageAmount);
            lastAttackTime = Time.time;
        }
    }

    public string GetEnemyType() {
        return "Statue";
    }

    public void SeenByPlayer(bool isSeen)
    {
        if (playerIsLooking == isSeen) return; // No change, skip

        playerIsLooking = isSeen;

        if (isSeen)
        {
            if (!seenOnce)
            {
                seenOnce = true;
            }

            // Switch to Seen visual
            currentVisual.SetActive(false);
            currentVisual = seenVisual;
            currentVisual.SetActive(true);

            // Freeze
            agent.velocity = Vector3.zero; 
            agent.isStopped = true;
            currentState = EnemyState.Seen;
        }
        else
        {
            if (CheckForPlayer())
            {
                currentVisual.SetActive(false);
                currentVisual = attackVisual;
                currentVisual.SetActive(true);

                agent.isStopped = false;
                currentState = EnemyState.Chase;
            }
            else
            {
                // Remain idle 
            }
        }
    }
}
