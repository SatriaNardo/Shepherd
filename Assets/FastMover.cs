using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastMover : MonoBehaviour
{
    public LayerMask collisionMask;

    void Update()
    {
        PlayerController playerController = GetComponent<PlayerController>();

        Vector3 movement = transform.forward * playerController.moveSpeed * Time.deltaTime;

        // Perform a raycast in the direction of movement
        if (Physics.Raycast(transform.position, movement.normalized, out RaycastHit hit, movement.magnitude, collisionMask))
        {
            // Stop at the point of collision
            transform.position = hit.point;

            // Handle collision
            Debug.Log("Hit: " + hit.collider.name);
        }
        else
        {
            // No collision, move as usual
            transform.position += movement;
        }
    }
}
