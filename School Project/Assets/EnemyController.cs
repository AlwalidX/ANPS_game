using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Patrolling
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    // Chasing
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;

    public float distanceToPlayer;
    // Returning to patrol
    public float loseSightTime = 5f; 
    private float loseSightTimer;
    private Vector3 lastKnownPlayerPosition;

    // Attacking
    public float attackCooldown = 2f; 
    private float attackTimer;

    // Components
    private NavMeshAgent agent;
    private Animator animator; // If you have animations

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // If you have animations

        // Ensure player object is assigned (find it if not)
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        // Check the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else if (loseSightTimer > 0)
        {
            ReturnToPatrol();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return; // No points to patrol

        // Move to the next patrol point
        agent.destination = patrolPoints[currentPatrolIndex].position;

        // Check if reached the patrol point
        if (Vector3.Distance(transform.position, agent.destination) < 1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        agent.destination = player.position;
        loseSightTimer = 0f; // Player in sight, reset the timer

        // Attack if in range
        if (distanceToPlayer <= attackRange) 
        {
            Attack();
        }
    }

    void Attack()
    {
        if (attackTimer <= 0f)
        {
            // Play attack animation (if you have one)
            if (animator != null)
            {
                animator.SetTrigger("Attack");                
            }

            // Apply damage to player (you'll need to get a reference to the player's health script)
            // player.GetComponent<PlayerHealth>().TakeDamage(damageAmount);

            attackTimer = attackCooldown;
        }

        attackTimer -= Time.deltaTime; 
    }

    void ReturnToPatrol()
    {
        loseSightTimer -= Time.deltaTime;

        if (loseSightTimer <= 0)
        {
            // Move back towards last known player position or resume patrol route
            agent.destination = lastKnownPlayerPosition; 

            // Check if roughly back at last position to resume patrol (adjust tolerance as needed)
            if (Vector3.Distance(transform.position, lastKnownPlayerPosition) < 2f) 
            {
                loseSightTimer = 0f; 
            }
        }
    }

    // Call this if the player gets out of sight during a chase
    public void PlayerLostSight()
    {
        loseSightTimer = loseSightTime;
        lastKnownPlayerPosition = player.position;
    }
}
