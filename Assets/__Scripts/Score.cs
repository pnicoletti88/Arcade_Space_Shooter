using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Score controller class to deal with updating the Text UI elements in the main game.
    public static Score scoreControllerReference = null;

    public int bossScore=150;

    //Score 
    public Text scoreText;
    public int score;

    //HighScore
    public Text highscoreText;
    public int highscore;

    //Load HighScore
    protected int LoadPlayerProgress()
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

    //Set up score singleton
    protected void Awake()
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
    //updates score by adding newScoreValue then calls UpdateScore
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }
    //updates high score text field in game
    public void UpdateHighScore()
    {
        highscoreText.text = "highscore: " + highscore;
    }
    //updates score text field in game
    public virtual void UpdateScore()
    {
        scoreText.text = "score: " + score;
    }
    //saves high score for player
    public void SavePlayerProgress()
    {
        PlayerPrefs.SetInt("highestScore", highscore);
        PlayerPrefs.Save();
    }
    //saves most recent score
    public void SavePlayerMostRecentScore()
    {
        PlayerPrefs.SetInt("currScore", score);
        PlayerPrefs.Save();
    }
    //updates score with value based on which enemy has been destroyed
    public void UpdateScore(string enemyTag)
    {
        switch (enemyTag)
        {
            case "Enemy0":
                AddScore(5);
                break;
            case "Enemy1":
                AddScore(10);
                break;
            case "Enemy2":
                AddScore(15);
                break;
            case "Enemy4":
                AddScore(15);
                break;
            case "EnemyBoss":
                AddScore(bossScore);
                Level.scriptRef.levelThreshold += 75;
                bossScore += 75;
                break;
            default:
                break;
        }
    }
}