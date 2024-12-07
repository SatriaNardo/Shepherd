using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarController : MonoBehaviour
{
    public ScoreStar scoreStar; // Reference to the ScriptableObject
    public Sprite activeStarSprite; // Sprite for an active star
    public Image Star1; // Sprite for an inactive star
    public Image Star2;
    public Image Star3;

    void Start()
    {
        UpdateStars(); // Update the stars at the start
    }

    public void UpdateStars()
    {
        int activeStars = 0;

        // Determine the number of stars based on the current state
        switch (scoreStar.resultState)
        {
            case ScoreStar.GameState.Fail:
                activeStars = 0;
                break;
            case ScoreStar.GameState.Okay:
                activeStars = 1;
                break;
            case ScoreStar.GameState.Great:
                activeStars = 2;
                break;
            case ScoreStar.GameState.Perfect:
                activeStars = 3;
                break;
        }

        // Update the UI to reflect the number of active stars
        if(activeStars == 1)
        {
            Star1.sprite = activeStarSprite;
        }
        if (activeStars == 2)
        {
            Star1.sprite = activeStarSprite;
            Star2.sprite = activeStarSprite;
        }
        if (activeStars == 3)
        {
            Star1.sprite = activeStarSprite;
            Star2.sprite = activeStarSprite;
            Star3.sprite = activeStarSprite;
        }
    }
}