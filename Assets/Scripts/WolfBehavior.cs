using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class WolfBehavior : MonoBehaviour
{
    public float speed = 5f;
    public float fleeSpeed = 15f;
    public float waitTime = 2f;
    public int sheepEaten = 0; // Counter for sheep eaten

    public Transform[] patrolPoints; // Waypoints for patrol
    private int currentPatrolIndex = 0;

    private GameObject[] sheep;
    private GameObject targetSheep;
    private NavMeshAgent agent;
    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip wolfSound;
    public bool isChasing = false;
    private bool isFleeing = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    void Start()
    {
        audioSource.PlayOneShot(wolfSound);
        sheep = GameObject.FindGameObjectsWithTag("Sheep");
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        agent.speed = speed;
        animator = GetComponentInChildren<Animator>();
        if (patrolPoints.Length > 0)
        {
            Debug.Log("Starting patrol...");
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
        else
        {
            Debug.LogWarning("No patrol points assigned!");
        }
    }

    void Update()
    {
        if (isFleeing) return;

        if (isChasing)
        {
            ChaseTargetSheep();
            CheckForCamping(); // Avoid camping behavior
        }
        else
        {
            Patrol();
        }

        UpdateAnimation();
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            Debug.Log("Reached patrol point " + currentPatrolIndex);
            StartCoroutine(MoveToNextPatrolPoint());
        }
    }
    public void StartFleeing(Vector3 sourcePosition)
    {
        if (isFleeing) return;

        isFleeing = true;
        isChasing = false; // Stop chasing sheep

        Vector3 fleeDirection = (transform.position - sourcePosition).normalized;
        Vector3 fleeTarget = transform.position + fleeDirection * 20f; // Move 10 units away
        agent.speed = fleeSpeed; 
        agent.SetDestination(fleeTarget);
        if(fleeDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if(fleeDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        // Start a coroutine to stop fleeing after a short time
        StartCoroutine(StopFleeing());
    }

    IEnumerator StopFleeing()
    {
        yield return new WaitForSeconds(3f); // Flee for 2 seconds
        isFleeing = false;
        agent.speed = speed;
        // Return to normal behavior
        if (targetSheep != null)
        {

            isChasing = true; // Resume chasing if there is a target
        }
        else
        {
            
            Patrol(); // Resume patrolling
        }
    }
    IEnumerator MoveToNextPatrolPoint()
    {
        yield return new WaitForSeconds(waitTime);
        
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        Debug.Log("Moving to patrol point: " + currentPatrolIndex);
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    public void StartChasing()
    {
        FindClosestSheep();
        if (targetSheep != null)
        {
            Debug.Log("Sheep detected. Switching to chase mode.");
            isChasing = true;
            agent.SetDestination(targetSheep.transform.position);
        }
    }

    void FindClosestSheep()
    {
        float closestDistance = Mathf.Infinity; // Reset to find the closest sheep
        targetSheep = null;

        foreach (GameObject s in sheep)
        {
            if (s.activeSelf)
            {
                float distance = Vector3.Distance(transform.position, s.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetSheep = s;
                }
            }
        }
        if (targetSheep == null)
        {
            Debug.Log("No active sheep found. Returning to patrol.");
            isChasing = false;
        }
    }

    void ChaseTargetSheep()
    {
        if (targetSheep != null && targetSheep.activeSelf)
        {
            float distanceToSheep = Vector3.Distance(transform.position, targetSheep.transform.position);

            if (distanceToSheep > 0.5f) // Move closer to the sheep
            {
                agent.SetDestination(targetSheep.transform.position);
            }
            else
            {
                Debug.Log("Reached sheep. Returning to patrol.");
                isChasing = false;
                targetSheep = null;
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            }
        }
        else
        {
            Debug.Log("Target sheep lost. Returning to patrol.");
            isChasing = false;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    public void EatSheep(GameObject sheep)
    {
        if (sheep.activeSelf)
        {
            sheep.SetActive(false); // Deactivate the sheep
            sheepEaten++;
            Debug.Log("Sheep eaten: " + sheepEaten);
            audioSource.PlayOneShot(wolfSound);
        }
    }
    void UpdateAnimation()
    {
        if (animator != null)
        {
            // Set `isMoving` based on agent velocity
            bool isMoving = agent.velocity.magnitude > 0.2f; // Adjust threshold as needed
            animator.SetBool("isMoving", isMoving);
        }
        if (spriteRenderer != null)
        {
            // Flip the sprite based on horizontal movement direction
            if (agent.velocity.x < -0.1f) // Moving left
            {
                spriteRenderer.flipX = true;
            }
            else if (agent.velocity.x > 0.1f) // Moving right
            {
                spriteRenderer.flipX = false;
            }
        }
    }
    void CheckForCamping()
    {
        if (isChasing && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            Debug.Log("Wolf is camping. Returning to patrol.");
            isChasing = false;
            targetSheep = null;
            Patrol();
        }
    }
}
