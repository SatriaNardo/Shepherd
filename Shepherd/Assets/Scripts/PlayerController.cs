using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float realSpeed = 5f;
    private Rigidbody rb;
    private Vector3 movement;
    bool running = false;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        // Make sure the Rigidbody doesn't rotate (to prevent flipping)
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Get input from player
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        
        // Create movement vector
        movement = new Vector3(moveX, 0f, moveZ);
        Run();
    }

    void FixedUpdate()
    {
        // Apply movement in FixedUpdate for physics consistency
        MoveCharacter(movement);
        
    }

    void MoveCharacter(Vector3 direction)
    {
        // Move the player in the desired direction
        Vector3 moveOffset = direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveOffset);
    }
    void Run()
    {
        if(Input.GetButtonDown("Run") && running == false)
        {
            moveSpeed = moveSpeed * 2;
            running = true;
        }
        else if(Input.GetButtonUp("Run"))
        {
            moveSpeed = realSpeed;
            running = false;
        }
    }    

}