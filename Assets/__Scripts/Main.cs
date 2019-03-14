using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main scriptReference;
    [Header("Set in Inspector")]
    public GameObject[] preFabEnemies = new GameObject[3];
    public float enemySpawnRate = 1;
    public float enemyPadding = 1.5f;

    private BoundsCheck boundM;

    void Awake()
    {
        if (scriptReference == null)
        {
            scriptReference = this; //sets up singleton so that only 1 main script can be created.
        }
        else
        {
            Debug.LogError("Attempted Creation of Second Main Script");
        }
        boundM = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnRate);
    }

 
    public void SpawnEnemy()
    {
        int randEnemy = Random.Range(0, preFabEnemies.Length);
        GameObject spawned = Instantiate<GameObject>(preFabEnemies[randEnemy]);

        float enemyPad = enemyPadding;
        if(spawned.GetComponent<BoundsCheck>() != null)
        {
            enemyPad = Mathf.Abs(spawned.GetComponent<BoundsCheck>().radius);
        }

        Vector3 startPos = Vector3.zero;
        float xMinimum = -boundM.camWidth + enemyPad;
        float xMaximum = boundM.camWidth - enemyPad;
        
        if(randEnemy == 2) // due to the sinusoidal pattern of enemy 2, shortening the horizontal bounds helps ensure more enemies reach the bottom before destruction.
        {
            xMinimum = xMinimum / 2;
            xMaximum = xMaximum / 2;
        }

        startPos.x = Random.Range(xMinimum, xMaximum);
        startPos.y = boundM.camHeight + enemyPad;
        spawned.transform.position = startPos;
        
        Invoke("SpawnEnemy", 1f / enemySpawnRate);
    }
    //function invokes restart with the given delay time
    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }
    //function loads the scene after DelayedRestart is called
    public void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }
}
