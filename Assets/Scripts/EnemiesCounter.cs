using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemiesCounter : MonoBehaviour
{
    public TextMeshProUGUI sheepCounterText; // Reference to the TextMeshProUGUI component

    void Update()
    {
        // Find all active GameObjects with the tag "enemies"
        GameObject[] sheep = GameObject.FindGameObjectsWithTag("Sheep");

        // Update the text to display the count
        sheepCounterText.text = $"{sheep.Length}";
    }
}