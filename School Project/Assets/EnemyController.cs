using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Patrol variables
    public Transform[] patrolPoints;
    private int currentPatrolPoint = 0;
    public float patrolSpeed = 2f;
    public float minDistanceToChangePoint = 0.1f;

    // Chase variables
    public Transform playerTransform;
    public float chaseRange = 10f;
    public float attackRange = 1.5f;
    public float chaseSpeed = 4f;

    // Attack variables
    public int attackDamage = 1;
    public float attackRate = 1f;
    private float lastAttackTime = 0f;

    // NavMeshAgent
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on " + gameObject.name);
        }

        UpdateDestination();
    }

    void Update()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform not assigned to " + gameObject.name);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Patrol behavior
        if (distanceToPlayer > chaseRange)
        {
            if (!agent.pathPending && agent.remainingDistance < minDistanceToChangePoint)
            {
                UpdateDestination();
            }
        }
        // Chase behavior
        else if (distanceToPlayer <= chaseRange)
        {
            agent.SetDestination(playerTransform.position);
            ChasePlayer();
        }
        // Attack behavior
        else if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
    }

    void UpdateDestination()
    {
        if (patrolPoints.Length == 0)
        {
            Debug.LogError("No patrol points assigned to " + gameObject.name);
            return;
        }

        agent.SetDestination(patrolPoints[currentPatrolPoint].position);
        currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        transform.LookAt(playerTransform);
    }

    void AttackPlayer()
    {
        agent.isStopped = true;
        transform.LookAt(playerTransform);

        if (Time.time - lastAttackTime >= attackRate)
        {
            // Deal damage to player here (e.g., using a trigger collider)
            Debug.Log(gameObject.name + " attacked player for " + attackDamage + " damage!");
            lastAttackTime = Time.time;
        }
    }
}
