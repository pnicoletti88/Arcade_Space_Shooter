using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Main_MainScene : MonoBehaviour
{

    static public Main_MainScene scriptReference; //Singleton

    [Header("Set in Inspector")]
    public GameObject[] preFabEnemies = new GameObject[3];
    public GameObject[] preFabPickUps = new GameObject[3];
    public float enemySpawnRate = 1f;
    public float enemyPadding = 1.5f;
    public int level = 1; //level field

    public GameObject particleExplosion;

    public GameObject pickUpAnimation;

    public WeaponDefinition[] weaponDefn;

    private bool _spawnEnemies = true; 
    public bool spawnPickUps { get; set; } = true; //auto property used
    private BoundsCheck _boundM;
    private Level _level;

    public bool spawnEnemies
    {
        get { return _spawnEnemies; }
        set
        {
            _spawnEnemies = value;
            if (!_spawnEnemies)
            {
                CancelInvoke("SpawnEnemy");
            }
            else if (_spawnEnemies && !IsInvoking("SpawnEnemy"))
            {
                Invoke("SpawnEnemy", 1f);
            }
        }
    }



    //list of all active enemies - will be needed for more complex weapons so it was set up now to avoid large refactor later
    private List<GameObject> _allEnemiesList = new List<GameObject>();
    private List<GameObject> _allPickUpList = new List<GameObject>();
    static private Dictionary<WeaponType, WeaponDefinition> _weaponDictionary = new Dictionary<WeaponType, WeaponDefinition>();


    void Awake()
    {
        _weaponDictionary = new Dictionary<WeaponType, WeaponDefinition>(); //reset dictionary on awake
        _allEnemiesList = new List<GameObject>(); //reset list of enemies on awake
        if (scriptReference == null)
        {
            scriptReference = this; //sets up singleton so that only 1 main script can be created.
        }
        else
        {
            Debug.LogError("Attempted Creation of Second Main Script");
        }
        _boundM = GetComponent<BoundsCheck>(); //gets the bounds chech component
        _level = GetComponent<Level>(); //gets the level component
        Invoke("SpawnEnemy", 1f / enemySpawnRate); //this start the enemies spawning
        Invoke("SpawnPickUps", 10);
        //adds the weapon definitions into the dictionary so they can be easily looked up later
        foreach (WeaponDefinition def in weaponDefn)
        {
            _weaponDictionary.Add(def.type, def); //adds the definition for the weapons into the dictionary for easy look up later
        }
    }

    public void SpawnPickUps()
    {
        int randPickUp = Random.Range(0, preFabPickUps.Length);
        GameObject toSpawn = Instantiate<GameObject>(preFabPickUps[randPickUp]);
        float pickUpPad = 1f;

        if (toSpawn.GetComponent<BoundsCheck>() != null)
        {
            pickUpPad = Mathf.Abs(toSpawn.GetComponent<BoundsCheck>().radius); //factors in the radius of the pickup for bounds check
        }
        Vector3 startPos = Vector3.zero;
        float xMinimum = -_boundM.camWidth + pickUpPad;
        float xMaximum = _boundM.camWidth - pickUpPad;

        //set the pickup to start somewhere just above the left of the screen
        startPos.x = Random.Range(xMinimum, xMaximum);
        startPos.y = _boundM.camHeight + pickUpPad;
        toSpawn.transform.position = startPos;
        toSpawn.transform.position = startPos;
        _allPickUpList.Add(toSpawn);
        //this stop enemies from spawning
        if (spawnPickUps)
        {
            Invoke("SpawnPickUps", 10); //invokes the function to run again
        }
        else //handles the asynchrous aspect kills an enenmy that was being construted if spawn is now fasle
        {
            DestroyPickup(toSpawn);
        }
    }

    public void SpawnEnemy()
    {
        //int randEnemy = Random.Range(0, preFabEnemies.Length); //randomly find which enemy to generate
        int randEnemy = Random.Range(0, _level.randRange); //random find which enemy to generate within the range specified by level class
        GameObject spawned = Instantiate<GameObject>(preFabEnemies[randEnemy]);

        enemySpawnRate = Level.eSpawnRate; //update spawn rate to match level class
        level = _level.level; //update level to match current level

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
        if (_spawnEnemies)
        {
            Invoke("SpawnEnemy", 1f / enemySpawnRate); //invokes the function to run again
        }
        else //handles the asynchrous aspect kills an enenmy that was being construted if spawn is now fasle
        {
            DestroyEnemy(spawned);
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
        SceneManager.LoadScene("Death_Screen");
    }


    //needs to be coded at some point to do actual function - just gives last Enemy for now
    //used by the homing missile function - PLEASE IGNORE FOR NOW :)
    public GameObject getClosestsEnemy()
    {
        if (_allEnemiesList.Count == 0)
        {
            return null;
        }
        else
        {
            return _allEnemiesList[0];
        }
    }

    //this function deletes all enemies and resets the enemy list - used to remove all enemies from screen
    public void DeleteAllEnemies()
    {
        foreach(GameObject item in _allEnemiesList)
        {
            Enemy_Parent.UpdateScore(item);
            DestroyEnemy(item,false);
        }
        _allEnemiesList = new List<GameObject>(); //C# garbage collection will remove of old list from memory
    }

    //looks up weapon in dictionary
    static public WeaponDefinition GetWeaponDefinition(WeaponType weaponIn)
    {
        if (_weaponDictionary.ContainsKey(weaponIn))
        {
            return _weaponDictionary[weaponIn];
        }
        return new WeaponDefinition(); //returns new weapon if weapon cannot be found
    }

    //this method will be used to destroy individual enemies...DO NOT destroy enemy without using this method!
    //Destroys and removes enemies from the list of enemies
    public void DestroyEnemy(GameObject enemyToDestroy, bool removeFromList = true)
    {
        if (removeFromList)
        {  
            _allEnemiesList.Remove(enemyToDestroy);
        }
        GameObject explos = Instantiate(particleExplosion);
        explos.transform.position = enemyToDestroy.transform.position;
        Destroy(enemyToDestroy);
    }

    public void DestroyPickup(GameObject pickUpToDestroy)
    {
       
        _allPickUpList.Remove(pickUpToDestroy);
        GameObject explos = Instantiate(particleExplosion);
        explos.transform.position = pickUpToDestroy.transform.position;
        Destroy(pickUpToDestroy);
    }
}
