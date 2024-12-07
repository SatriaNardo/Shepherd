using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfEatTrigger : MonoBehaviour
{
    private WolfBehavior wolfBehavior;

    void Start()
    {
        // Get reference to the parent WolfBehavior script
        wolfBehavior = GetComponentInParent<WolfBehavior>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sheep"))
        {
            Debug.Log("Sheep entered eat range.");
            wolfBehavior.EatSheep(other.gameObject);
        }
    }
}
