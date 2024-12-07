using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public CountdownTimer countdownTimer; // Reference to the CountdownTimer script
    private bool isGamePaused = false; // Track if the game is paused
    public GameObject pausePanel;
    void Update()
    {
        // Toggle pause with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0; // Pause the game
        if (countdownTimer != null)
        {
            countdownTimer.isPaused = true; // Pause the timer
        }
        if (!pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1; // Resume the game
        if (countdownTimer != null)
        {
            countdownTimer.isPaused = false; // Resume the timer
        }
        if (pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
        }
    }
}
