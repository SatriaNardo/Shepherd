using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTraps : MonoBehaviour
{
    public int sheepEaten = 0; // Counter for sheep eaten by this wolf
    public float eatCooldown = 2f; // Cooldown time between eating sheep
    private float eatTimer = 0f;
    public AudioSource audioSource;
    public AudioClip wolfSound;
    private void Start()
    {
        // Ensure cooldown starts at 0 to allow immediate interaction
        eatTimer = 0f;
    }

    private void Update()
    {
        // Decrease the timer if it's above zero
        if (eatTimer > 0)
        {
            eatTimer -= Time.deltaTime;
        }
    }

    public void TryEatSheep(GameObject sheep)
    {
        if (eatTimer <= 0 && sheep.activeSelf)
        {
            sheep.SetActive(false); // Deactivate the sheep
            sheepEaten++; // Increment the sheep eaten count
            Debug.Log($"Stationary Wolf ate a sheep! Total eaten: {sheepEaten}");
            audioSource.PlayOneShot(wolfSound);
            eatTimer = eatCooldown; // Reset the cooldown timer
        }
    }
}