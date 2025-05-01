using UnityEngine;
using UnityEngine.AI;

public class StatueEnemyAI : MonoBehaviour
{
    
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private int damageAmount;
    [SerializeField] private float sightRange, attackRange, attackCooldown, runSpeed;
    
    [SerializeField]private Vector3 offset;
    private float lastAttackTime = 0f;
    
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
    }

    private void Update() {
        switch (currentState) {
            case EnemyState.Idle:
                break;
            case EnemyState.Seen:
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
        CheckForPlayer();
    }
    
    private void CheckForPlayer()
    {
        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position + offset, dirToPlayer, out RaycastHit hit, sightRange))
        {
            if (hit.collider.CompareTag("Player") && !(currentState == EnemyState.Attack))
            {
                StopAllCoroutines();
                currentState = EnemyState.Attack;
            }
        }
    }
    
    private void Chase()
    {
        agent.isStopped = false;
        agent.speed = runSpeed;
        agent.SetDestination(player.transform.position);
        float distance = Vector3.Distance(transform.position, player.transform.position);
        //animator.Play("Chase");
        if (distance <= attackRange)
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

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            player.TakeDamage(damageAmount);
            lastAttackTime = Time.time;
        }
    }
}
