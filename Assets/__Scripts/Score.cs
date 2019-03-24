using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Score : MonoBehaviour
{
    public static Score scoreControllerReference = null;


    //Score 
    public Text scoreText;
    public int score;

    //HighScore
    public Text highscoreText;
    public int highscore;

    //Load HighScore
    private int LoadPlayerProgress()
    {
        if (PlayerPrefs.HasKey("highestScore"))
        {
            return PlayerPrefs.GetInt("highestScore");
        }
        else
        {

            return 0;
        }
    }

    void Awake()
    {
        if (scoreControllerReference == null)
        {
            scoreControllerReference = this;
        }
        else
        {
            Debug.LogError("Attempted creation of second Score Controller");
        }

    }

    //Load Scoreboard
    void Start()
    {
        score = 0;
        highscore = LoadPlayerProgress();
        UpdateScore();
        UpdateHighScore();
    }
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }
    void UpdateHighScore()
    {
        highscoreText.text = "HighScore: " + highscore;
    }
    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
    public void SavePlayerProgress()
    {
        PlayerPrefs.SetInt("highestScore", highscore);
        PlayerPrefs.Save();
    }


}
