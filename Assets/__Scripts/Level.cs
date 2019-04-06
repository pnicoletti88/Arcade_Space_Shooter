using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    static public Level scriptRef;
    public Text levelText;
    public Text newLevelText;
    public int level = 1;
    public int randRange = 1;
    static public float eSpawnRate = 0.5f;
    public int levelThreshold=75; //threshold for each level
    public bool rot = false;
    public bool boss = false; // field that tells main if a boss should spawn
    public int scoreToUpdate = 75; // field is the score needed for next level;
    // Start is called before the first frame update
    void Awake()
    {
        if (scriptRef == null)
        {
            scriptRef = this; //sets up singleton so that only 1 main script can be created.
        }
        else
        {
            Debug.LogError("Attempted Creation of Second Level Script");
        }
        eSpawnRate = 1f;
        level = 1;
        randRange = 1;
        UpdateLevel(1);
    }

    void Update()
    {
        // new levels occur every levelThreshold amount
        if (Score.scoreControllerReference.score >= scoreToUpdate)
        {
            int numLevelsToUpdate = 0;
            while (Score.scoreControllerReference.score >= scoreToUpdate)
            {
                numLevelsToUpdate++;
                scoreToUpdate += levelThreshold;
            }
            UpdateLevel(level + numLevelsToUpdate);
        }
    }

    //this function is called to set the parameters for each new level
    void UpdateLevel(int newLevel)
    {
        //If its a new level that is not level one, display new level in newLevelText UI element
        
        //first four levels result in one more enemy dropping, randRange increases with every new level
        if (newLevel < 5) { 
            level = newLevel;
            randRange = newLevel;
        }
        else if (newLevel >= 5)
        {
            //every 5 levels a boss is spawned at a rate of 1f
            if (newLevel % 5 == 0)
            {
                boss = true;
                level = newLevel;
                eSpawnRate = 1f;
            }
            else
            {
                //if the level is not a multiple of 5, spawn rate is increased by .1% each time
                boss = false;
                level = newLevel;
                randRange = 4;
                eSpawnRate += 0.3f;
            }
        }
        if (newLevel != 1)
        {
            if (boss)
            {
                newLevelText.text = "boss level";
            }
            else
            {
                newLevelText.text = "next level";
            }
            Invoke("ClearNewLevel", 2);
        }

        //text field is updated with new level
        levelText.text = "Level: " + newLevel;
    }
    //Clears text from newLevelText UI element
    void ClearNewLevel()
    {
        newLevelText.text = "";
    }
}
