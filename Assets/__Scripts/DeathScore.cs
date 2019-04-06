using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScore : Score
{
    void Start()
    {
        // DeathScore is inherited from Score, which acts as the score controller in the main game scene - minor differences here just account for text differences, and setting the initial score to the player's most recent final score instead of 0.
        score = LoadCurrScore();
        highscore = LoadPlayerProgress();
        UpdateScore();
        UpdateHighScore();
    }
    // additional function to set the score to the most recent final score
    private int LoadCurrScore()
    {
        if (PlayerPrefs.HasKey("currScore"))
        {
            return PlayerPrefs.GetInt("currScore");
        }
        else
        {
            return 0;
        }
    }

    public override void UpdateScore()
    {
        // function override to add the word "final"
        scoreText.text = "final score: " + score;
    }
}
