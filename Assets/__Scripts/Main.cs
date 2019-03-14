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

    private bool _spawnEnemies = true;
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
        _boundM = GetComponent<BoundsCheck>();
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
        float xMinimum = -_boundM.camWidth + enemyPad;
        float xMaximum = _boundM.camWidth - enemyPad;
        
        if(randEnemy == 2) // due to the sinusoidal pattern of enemy 2, shortening the horizontal bounds helps ensure more enemies reach the bottom before destruction.
        {
            xMinimum = xMinimum / 2;
            xMaximum = xMaximum / 2;
        }

        startPos.x = Random.Range(xMinimum, xMaximum);
        startPos.y = _boundM.camHeight + enemyPad;
        spawned.transform.position = startPos;

        _allEnemiesList.Add(spawned);

        if (_spawnEnemies)
        {
            Invoke("SpawnEnemy", 1f / enemySpawnRate);
        }
        else
        {
            DeleteAllEnemies();
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

    public void RemoveEnemyList(GameObject enemyToDelete)
    {
        _allEnemiesList.Remove(enemyToDelete);
    }

    public void DeleteAllEnemies()
    {
        _spawnEnemies = false;
        foreach(GameObject item in _allEnemiesList)
        {
            Destroy(item);
        }
        _allEnemiesList = new List<GameObject>(); //C# garbage collection will remove of old list
    }
}
