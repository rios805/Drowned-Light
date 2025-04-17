using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private int minIdle, maxIdle, damageAmount;
    [SerializeField] private float sightRange, attackRange, runSeconds, runMin, runMax, attackCooldown;
    
    
    public List<Transform> patrolPoints;
    private int patrolAmt;
    public float walkSpeed;
    public float runSpeed;
    public float idleSeconds;
    
    private bool isWalking;
    private bool isRunning;

    [SerializeField]private Vector3 offset;
    private Transform currentPatrolPoint;
    private int rand1;
    private int rand2;
    private Vector3 desiredPatrolPoint;
    
    public Player player;

    private void Start() {
        isWalking = true;
        patrolAmt = patrolPoints.Count;
        rand1 = Random.Range(0, patrolAmt);
        currentPatrolPoint = patrolPoints[rand1];
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        Vector3 directionToPlayerNormalized = (player.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position + offset, directionToPlayerNormalized, out RaycastHit hit, sightRange)) {
            Debug.DrawRay(transform.position + offset, directionToPlayerNormalized * sightRange, Color.blue);
            if (hit.collider.gameObject.CompareTag("Player")) {
                isWalking = false;
                
                StopCoroutine("Idle");
                StopCoroutine("Run");
                StartCoroutine("Run");
                
                isRunning = true;
                
            }
        }
        if (isWalking) {
            desiredPatrolPoint = currentPatrolPoint.position;
            agent.SetDestination(desiredPatrolPoint);
            agent.speed = walkSpeed;

            if (agent.remainingDistance <= agent.stoppingDistance) {
                rand2 = Random.Range(0, 2);
                // Walk to new point
                if (rand2 == 0) {
                    rand1 = Random.Range(0, patrolAmt);
                    currentPatrolPoint = patrolPoints[rand1];
                }
                // Idle at point
                if (rand2 == 1) {
                    
                    agent.speed = 0;
                    isWalking = false;
                    StopCoroutine("Idle");
                    StartCoroutine("Idle");
                }
            }
        }

        if (isRunning) {
            
            desiredPatrolPoint = player.transform.position ;
            agent.SetDestination(desiredPatrolPoint);
            agent.speed = runSpeed;
            
            if (agent.remainingDistance <= attackRange) {
                // Animation stuff later
                
                // Take the amount of damage
                player.TakeDamage(damageAmount);
                
                // Setup attack cooldown
            }
        }
    }

    IEnumerator Idle() {
        idleSeconds = Random.Range(minIdle, maxIdle);
        yield return new WaitForSeconds(idleSeconds);
        isWalking = true;
        rand1 = Random.Range(0, patrolAmt);
        currentPatrolPoint = patrolPoints[rand1];
        
        // Animation stuff later
    }

    IEnumerator Run() {
        runSeconds = Random.Range(runMin, runMax);
        yield return new WaitForSeconds(runSeconds);
        isWalking = true;
        rand1 = Random.Range(0, patrolAmt);
        currentPatrolPoint = patrolPoints[rand1];
        // Animation stuff later
    }
}
