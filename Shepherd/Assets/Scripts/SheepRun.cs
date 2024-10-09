using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SheepRun : MonoBehaviour
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float runRange = 5f;
    [Header("Flockings")]
    [SerializeField] private float flockingRadius = 5f;
    [SerializeField] private float seperationDistance = 2f;
    [SerializeField] private float seperationWeight = 1f;
    [SerializeField] private float cohesionWeight = 1f;
    [SerializeField] private float alignmentWeight = 1f;

    [SerializeField] private bool shouldFlock = true;

    [SerializeField] private Transform playerPosition;

    private GameObject[] sheeps;

    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        sheeps = GameObject.FindGameObjectsWithTag("Sheep");
    }

    // Update is called once per frame
    void Update()
    {
        if(playerPosition != null && Vector3.Distance(transform.position, playerPosition.position) < detectionRange)
        {
            Vector3 runDirection = transform.position - playerPosition.position;
            Vector3 targetPosition = transform.position + runDirection.normalized * runRange;
            if (shouldFlock)
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
        shouldFlock = Input.GetMouseButtonDown(1);
        
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
}
