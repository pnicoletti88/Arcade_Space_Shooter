using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScore : Score
{
    // Start is called before the first frame update
    void Start()
    {
        score = LoadCurrScore();
        highscore = LoadPlayerProgress();
        UpdateScore();
        UpdateHighScore();
    }

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
        scoreText.text = "final score: " + score;
    }
}
