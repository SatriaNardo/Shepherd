using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        // Quit the application
        Application.Quit();

        // Log a message (useful for debugging in the editor)
        Debug.Log("Game is exiting...");
    }
}
