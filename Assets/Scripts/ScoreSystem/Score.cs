using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]


public class Score 
{
    public int ScoreValue;
    public string PlayerName;
    public Score(int scoreValue, string playerName)
    {
        ScoreValue = scoreValue;
        PlayerName = playerName;
    }
}
