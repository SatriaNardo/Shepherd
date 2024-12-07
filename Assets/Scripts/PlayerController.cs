using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private float realSpeed;
    private Rigidbody rb;
    private Vector3 movement;
    Animator ani;
    SpriteRenderer spriteRendered;
    bool running = false;
    public bool shouldFlock = false;
    public bool isBarking = false;
    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip barkSound;
    public KeyCode barkKey = KeyCode.Space; // Key to trigger bark
    public float barkForce = 10f; // Force applied to wolves
    public Collider barkRangeCollider; // Reference to the child collider
    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        spriteRendered = GetComponent<SpriteRenderer>();
        // Make sure the Rigidbody doesn't rotate (to prevent flipping)
        rb.freezeRotation = true;
        realSpeed = moveSpeed;
    }

    void Update()
    {
        // Get input from player
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            ani.SetBool("Moving", true);
        }
        else
        {
            ani.SetBool("Moving", false);
        }
        if(moveX < 0)
        {
            spriteRendered.flipX = true;
        }
        else if (moveX > 0)
        {
            spriteRendered.flipX = false;
        }
        movement = new Vector3(moveX, 0f, moveZ);
        Run();
        if (Input.GetKeyDown(barkKey))
        {
            Bark();
        }
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
        if (Physics.Raycast(rb.position, moveOffset.normalized, out RaycastHit hit, moveOffset.magnitude))
        {
            // Stop at the point of collision
            rb.MovePosition(hit.point);
        }
        else
        {
            // No collision, proceed with movement
            rb.MovePosition(rb.position + moveOffset);
        }
    }
    void Run()
    {
        if(Input.GetButtonDown("Player1_Run") && running == false)
        {
            moveSpeed = moveSpeed * 2f;
            running = true;
        }
        else if(Input.GetButtonUp("Player1_Run"))
        {
            moveSpeed = realSpeed;
            running = false;
        }
    }
    void Bark()
    {
        
        if (audioSource != null && barkSound != null && isBarking == false)
        {
            audioSource.PlayOneShot(barkSound);
            isBarking = true;
        }
        
        // Get all colliders within the bark range
        // Get all colliders within the bark range
        Collider[] hitColliders = Physics.OverlapBox(barkRangeCollider.bounds.center, barkRangeCollider.bounds.extents, barkRangeCollider.transform.rotation);

        foreach (Collider hitCollider in hitColliders)
        {
            // Check if the collider belongs to a wolf
            WolfBehavior wolf = hitCollider.GetComponentInParent<WolfBehavior>();
            if (wolf != null)
            {
                // Trigger the wolf's Flee behavior
                wolf.StartFleeing(transform.position);
            }
        }
        StartCoroutine(ResetBarkAfterSound());
    }
    IEnumerator ResetBarkAfterSound()
    {
        if (audioSource != null && barkSound != null)
        {
            yield return new WaitForSeconds(barkSound.length); // Wait until the sound finishes
        }

        isBarking = false; // Allow barking again
    }
}