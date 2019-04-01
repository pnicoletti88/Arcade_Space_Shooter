using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Score : MonoBehaviour
{
    public static Score scoreControllerReference = null;

    public bool isDeath;


    //Score 
    public Text scoreText;
    public int score;

    //HighScore
    public Text highscoreText;
    public int highscore;

    //Load HighScore
    private int LoadPlayerProgress()
    {
        if (!isDeath)
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
        else
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
        
        if (!isDeath)
        {
            score = 0;
            PlayerPrefs.SetInt("currScore", score);
            UpdateScore();
            highscore = LoadPlayerProgress();
            UpdateHighScore();
        }
        else
        {
            score = LoadPlayerProgress();
            UpdateScore();
        }
    }
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }
    void UpdateHighScore()
    {
        highscoreText.text = "highscore: " + highscore;
    }
    void UpdateScore()
    {
        if (!isDeath)
        {
            scoreText.text = "score: " + score;
        }
        else
        {
            scoreText.text = "final score " + score;
        }
    }
    public void SavePlayerProgress()
    {
        PlayerPrefs.SetInt("highestScore", highscore);
        PlayerPrefs.SetInt("currScore", score);
        PlayerPrefs.Save();
    }


}
