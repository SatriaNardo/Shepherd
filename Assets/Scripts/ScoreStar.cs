using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStateObject", menuName = "ScriptableObjects/GameState", order = 1)]
public class ScoreStar : ScriptableObject
{
    public enum GameState
    {
        NotPlayed,
        Okay,
        Great,
        Perfect,
        Fail
    }
    public GameState resultState;
}
