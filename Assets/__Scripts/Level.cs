using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    static public Level scriptRef;
    public Text levelText;
    public int level = 1;
    public int randRange = 1;
    static public float eSpawnRate = 1f;
    private Score _score;
    public bool rot = false;
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
        _score = GetComponent<Score>();
        UpdateLevel(1);
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_score.score % 20 >= 0 && _score.score % 20 < 20) {
            UpdateLevel(_score.score / 20+1);
          }

    }

    void UpdateLevel(int newLevel)
    {
        if (newLevel != level)
            levelText.text = "NEW LEVEL!";
        if (newLevel < 5) { 
        level = newLevel;
        randRange = newLevel;
    }
        else if (newLevel >= 5)
        {
            level = newLevel;
            randRange = 4;
            eSpawnRate = eSpawnRate*1.001f;
        }
        levelText.text = "Level: " + newLevel;
    }

}
