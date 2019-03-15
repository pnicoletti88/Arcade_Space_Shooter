using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main scriptReference; //Singleton

    [Header("Set in Inspector")]
    public GameObject[] preFabEnemies = new GameObject[3];
    public float enemySpawnRate = 1;
    public float enemyPadding = 1.5f;

    public bool spawnEnemies { get; set; } = true; //auto property used

    private BoundsCheck _boundM;
    private List<GameObject> _allEnemiesList = new List<GameObject>();



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
        _boundM = GetComponent<BoundsCheck>(); //gets the bounds chech component
        Invoke("SpawnEnemy", 1f / enemySpawnRate); //this start the enemies spawning
    }

 
    public void SpawnEnemy()
    {
        int randEnemy = Random.Range(0, preFabEnemies.Length); //randomly find which enemy to generate
        GameObject spawned = Instantiate<GameObject>(preFabEnemies[randEnemy]);

        float enemyPad = enemyPadding;
        if(spawned.GetComponent<BoundsCheck>() != null)
        {
            enemyPad = Mathf.Abs(spawned.GetComponent<BoundsCheck>().radius); //factors in the radius of the enemy for bounds check
        }

        Vector3 startPos = Vector3.zero;
        float xMinimum = -_boundM.camWidth + enemyPad;
        float xMaximum = _boundM.camWidth - enemyPad;
        
        if(randEnemy == 2) // due to the sinusoidal pattern of enemy 2, shortening the horizontal bounds helps ensure more enemies reach the bottom before destruction.
        {
            xMinimum = xMinimum / 2;
            xMaximum = xMaximum / 2;
        }

        //set the enemy to start somewhere just above the top of the screen
        startPos.x = Random.Range(xMinimum, xMaximum);
        startPos.y = _boundM.camHeight + enemyPad;
        spawned.transform.position = startPos;

        //adds the enemy into the list of all enemies
        _allEnemiesList.Add(spawned);

        //this stop enemies from spawning
        if (spawnEnemies)
        {
            Invoke("SpawnEnemy", 1f / enemySpawnRate); //invokes the function to run again
        }
        else //handles the asynchrous aspect kills an enenmy that was being construted if spawn is now fasle
        {
            _allEnemiesList.Remove(spawned);
            Destroy(spawned);
        }
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

    //removes a single enemy from the enemy list (gets called from Hero_Script when enemies are going to be destroyed
    public void RemoveEnemyList(GameObject enemyToDelete)
    {
        _allEnemiesList.Remove(enemyToDelete);
    }

    //this function deletes all enemies and resets the enemy list - used to remove all enemies from screen
    public void DeleteAllEnemies()
    {
        foreach(GameObject item in _allEnemiesList)
        {
            Destroy(item);
        }
        _allEnemiesList = new List<GameObject>(); //C# garbage collection will remove of old list from memory
    }

    
}
