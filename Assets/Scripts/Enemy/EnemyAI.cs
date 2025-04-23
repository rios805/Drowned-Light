using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private int minIdle, maxIdle, damageAmount;
    [SerializeField] private float sightRange, attackRange, runSeconds, runMin, runMax, attackCooldown;
    
    
    public List<Transform> patrolPoints;
    public float walkSpeed;
    public float runSpeed;
    

    [SerializeField]private Vector3 offset;
    private Transform currentPatrolPoint;
    private int rand1;
    private int rand2;
    private Vector3 desiredPatrolPoint;
    
    private float lastAttackTime = 0f;
    
    public Player player;

    private enum EnemyState {
        Patrol,
        Idle,
        Chase,
        Attack
    }
    private EnemyState currentState;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        currentPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Count)];
        currentState = EnemyState.Patrol;
    }

    private void Update() {
        switch (currentState) {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Idle:
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
        CheckForPlayer();
        //Debug.Log(currentState);
    }
    
    private void CheckForPlayer()
    {
        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position + offset, dirToPlayer, out RaycastHit hit, sightRange))
        {
            if (hit.collider.CompareTag("Player") && !(currentState == EnemyState.Attack))
            {
                StopAllCoroutines();
                currentState = EnemyState.Chase;
            }
        }
    }

    private void Patrol()
    {
        agent.speed = walkSpeed;
        agent.SetDestination(currentPatrolPoint.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            int rand = Random.Range(0, 2);

            if (rand == 0)
            {
                currentPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Count)];
            }
            else
            {
                agent.isStopped = true;
                currentState = EnemyState.Idle;
                StartCoroutine(Idle());
            }
        }
    }
    
    private void Chase()
    {
        agent.speed = runSpeed;
        agent.SetDestination(player.transform.position);
        float distance = Vector3.Distance(transform.position, player.transform.position);
        
        if (distance <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        
        else if (distance > sightRange)
        {
            // Lost the player
            StartCoroutine(ResumePatrol());
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
    
    IEnumerator Idle() {
        yield return new WaitForSeconds(Random.Range(minIdle, maxIdle));
        agent.isStopped = false;
        currentPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Count)];
        currentState = EnemyState.Patrol;
        
        // Animation stuff later
    }

    IEnumerator ResumePatrol()
    {
        yield return new WaitForSeconds(Random.Range(runMin, runMax));
        currentPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Count)];
        currentState = EnemyState.Patrol;
    }
}
