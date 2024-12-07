using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfEatTriggerTrap : MonoBehaviour
{
    private WolfTraps parentWolf;

    private void Start()
    {
        // Get the parent wolf script
        parentWolf = GetComponentInParent<WolfTraps>();
        if (parentWolf == null)
        {
            Debug.LogError("No StationaryWolf script found on parent!");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Sheep"))
        {
            // Try to eat the sheep through the parent wolf script
            parentWolf.TryEatSheep(other.gameObject);
        }
    }
}
