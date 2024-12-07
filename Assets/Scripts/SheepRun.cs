using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SheepRun : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float runRange = 10f;
    [Header("Movement Variables")]
    [SerializeField]  private float moveSpeed = 20f;
    [SerializeField]  private float acceleration = 30f;
    [Header("Flockings")]
    [SerializeField] private float flockingRadius = 20f;
    [SerializeField] private float seperationDistance = 2f;
    [SerializeField] private float seperationWeight = 1f;
    [SerializeField] private float cohesionWeight = 5f;
    [SerializeField] private float alignmentWeight = 10f;
    [SerializeField] Animator animator;

    [SerializeField] private Transform playerPosition;
    [SerializeField] PlayerController playerControl;
    Transform childTransform;
    private GameObject[] sheeps;
    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        childTransform = transform.Find("SheepSprite");
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        sheeps = GameObject.FindGameObjectsWithTag("Sheep");
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.acceleration = acceleration;
    }

    // Update is called once per frame
    private bool slowingDown = false;
    void Update()
    {
        if(playerPosition != null && Vector3.Distance(transform.position, playerPosition.position) < detectionRange)
        {
            slowingDown = false;
            navMeshAgent.speed = moveSpeed;
            StopCoroutine(SlowDown());
            
            Vector3 runDirection = transform.position - playerPosition.position;
            Vector3 targetPosition = transform.position + runDirection.normalized * runRange;
            if(runDirection.x < 0.1)
            {
                childTransform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            }
            else if(runDirection.x > 0.1)
            {
                childTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            if (playerControl.shouldFlock)
            {
                Vector3 flockingDirection = GetFlockingDirection(sheeps);
                targetPosition += flockingDirection;
            }
            if (navMeshAgent.isActiveAndEnabled)
            {
                navMeshAgent.updateRotation = false;
                navMeshAgent.SetDestination(targetPosition);
            }
        }
        else
        {
            if(!slowingDown)
            {
                slowingDown = true;
                StartCoroutine(SlowDown());
            }
        }
        if(navMeshAgent.speed > 0.1)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }
    private Vector3 GetFlockingDirection(GameObject[] sheeps)
    {
        Vector3 flockingDirection = Vector3.zero;
        Vector3 averagePosition = Vector3.zero;
        Vector3 averageVelocity = Vector3.zero;
        int flockSize = 0;
        foreach (GameObject sheep in sheeps)
        {
            if (!sheep.activeInHierarchy)
                continue;
            if (sheep.GetComponent<NavMeshAgent>().velocity.magnitude == 0)
                continue;
            float distance = Vector3.Distance(transform.position, sheep.transform.position);

            if (distance > flockingRadius || distance == 0f)
                continue;
            flockSize++;
            Debug.Log(flockSize);
            averagePosition += sheep.transform.position;
            averageVelocity += sheep.GetComponent<NavMeshAgent>().velocity;
            
            if (distance < seperationDistance)
            {
                Vector3 seperationDirection = transform.position - sheep.transform.position;
                seperationDirection.Normalize();
                seperationDirection /= distance;
                flockingDirection += seperationDirection * seperationWeight;
            }
        }
        if (flockSize > 0)
        {
            averagePosition /= flockSize;
            averageVelocity /= flockSize;

            Vector3 alignmentDirection = averageVelocity.normalized;
            Vector3 cohesionDirection = (averagePosition - transform.position).normalized;

            flockingDirection += alignmentDirection * alignmentWeight;
            flockingDirection += cohesionDirection * cohesionWeight;
        }
        return flockingDirection;
        

    }
    private IEnumerator SlowDown()
    {
        float timer = 0f;
        float slowDownTime = 0.5f;
        while(timer < slowDownTime)
        {
            timer += Time.deltaTime;
            float t = timer / slowDownTime;
            navMeshAgent.speed = Mathf.Lerp(moveSpeed, 0, t);
            yield return null;
        }
    }
    private void ChangeSpeed()
    {
        float angle = Vector3.Angle(transform.forward, navMeshAgent.steeringTarget - transform.position);
        float speedMultiplier = 1f - 0.9f * angle / 180f;
        navMeshAgent.speed = moveSpeed * speedMultiplier;
    }
    void anim()
    {

    }
}
