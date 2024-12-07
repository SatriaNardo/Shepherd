using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public GameObject resultPanel;
    public Image resultText;
    public Image resultPictures;
    public Image Star1;
    public Image Star2;
    public Image Star3;
    public Sprite resultTextWin;
    public Sprite resultTextLose;
    public Sprite resultPicturesPerfect;
    public Sprite resultPicturesGood;
    public Sprite resultPicturesBad;
    public Sprite starActive;
    public ScoreStar result;
    public int totalSheepEaten;
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component
    public int minutes = 5; // Starting minutes
    public int seconds = 0; // Starting seconds

    public AudioClip CompleteMusic;
    public AudioClip failMusic;
    public AudioSource audioSource;
    private bool levelFinished;
    public string resultMessage;
    private float timeRemaining;
    //private bool isRunning = true;
    public bool isPaused = false;
    private bool timerStopped = false;
    void Start()
    {
        // Convert minutes and seconds to total seconds
        timeRemaining = (minutes * 60) + seconds;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (!isPaused && !timerStopped)
        {
            CheckSheepStatus(); // Check if all sheep are inactive

            if (timeRemaining > 0)
            {
                if(levelFinished == false)
                {
                    timeRemaining -= Time.deltaTime;
                }
                UpdateTimerDisplay();
            }
            else if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                UpdateTimerDisplay();
                StopTimer();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        // Convert timeRemaining to minutes and seconds
        int displayMinutes = Mathf.FloorToInt(timeRemaining / 60);
        int displaySeconds = Mathf.FloorToInt(timeRemaining % 60);

        // Update the TextMeshProUGUI text
        timerText.text = $"{displayMinutes:00}:{displaySeconds:00}";
    }

    private void CheckSheepStatus()
    {
        GameObject[] sheep = GameObject.FindGameObjectsWithTag("Sheep");
        if (sheep.Length == 0)
        {
            StopTimer();
        }
    }
    private void StopTimer()
    {
        timerStopped = true;
        isPaused = true;
        CalculateResult();
        Debug.Log($"Timer stopped with {timeRemaining:F2} seconds remaining. Result: {resultMessage}");
        
        resultPanel.SetActive(true);
    }
    private void CalculateResult()
    {
        WolfBehavior[] wolves = FindObjectsOfType<WolfBehavior>();
        WolfTraps[] stationaryWolves = FindObjectsOfType<WolfTraps>();
        totalSheepEaten = 0;
        levelFinished = true;
        foreach (WolfBehavior wolf in wolves)
        {
            totalSheepEaten += wolf.sheepEaten;
        }
        foreach (WolfBehavior wolf in wolves)
        {
            totalSheepEaten += wolf.sheepEaten;
        }
        if (timeRemaining > (minutes * 60) * 0.75f && totalSheepEaten < 5) // More than 75% time left
        {
            resultMessage = "Perfect";
            resultText.sprite = resultTextWin;
            resultPictures.sprite = resultPicturesPerfect;
            Star1.sprite = starActive;
            Star2.sprite = starActive;
            Star3.sprite = starActive;
            result.resultState = ScoreStar.GameState.Perfect;
            PlayResultMusic(CompleteMusic);
        }
        else if (timeRemaining > (minutes * 60) * 0.5f && totalSheepEaten < 8) // More than 50% time left
        {
            resultMessage = "Great";
            resultText.sprite = resultTextWin;
            resultPictures.sprite = resultPicturesGood;
            Star1.sprite = starActive;
            Star2.sprite = starActive;
            if(result.resultState != ScoreStar.GameState.Perfect)
            {
                result.resultState = ScoreStar.GameState.Great;
            }
            PlayResultMusic(CompleteMusic);
        }
        else if (timeRemaining > 0 || totalSheepEaten <= 11) // Some time left
        {
            resultMessage = "Okay";
            resultText.sprite = resultTextWin;
            resultPictures.sprite = resultPicturesGood;
            Star1.sprite = starActive;
            if (result.resultState != ScoreStar.GameState.Perfect || result.resultState != ScoreStar.GameState.Great)
            {
                result.resultState = ScoreStar.GameState.Great;
            }
            PlayResultMusic(CompleteMusic);
        }
        else if (timeRemaining == 0 || totalSheepEaten > 10)
        {
            resultMessage = "Fail";
            resultText.sprite = resultTextLose;
            resultPictures.sprite = resultPicturesBad;
            if (result.resultState != ScoreStar.GameState.Perfect || result.resultState != ScoreStar.GameState.Great || result.resultState != ScoreStar.GameState.Okay)
            {
                result.resultState = ScoreStar.GameState.Great;
            }
            PlayResultMusic(failMusic);
        }
    }
    private void PlayResultMusic(AudioClip music)
    {
        if (audioSource != null && music != null)
        {
            audioSource.Stop(); // Stop any currently playing audio
            audioSource.clip = music;
            audioSource.loop = false;
            audioSource.Play();
        }
    }
}
