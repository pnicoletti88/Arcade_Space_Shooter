using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Main_MainScene : MonoBehaviour
{

    static public Main_MainScene scriptReference; //Singleton

    [Header("Set in Inspector")]
    public Vector3 lastPos;
    public GameObject[] preFabEnemies = new GameObject[4];
    public float enemySpawnRate = 0.5f;
    public float enemyPadding = 1.5f;
    public int level = 1; //level field

    public GameObject particleExplosion;
    public GameObject prefabPowerUp;

    //This Array holds the references to the different weapon types, and one is randomly assigned when a powerup object is created. The larger the
    //number of instances in this array, the higher probability that that type of powerup will drop. (e.g. triple is the most common)
    private WeaponType[] _powerUpFrequency = new WeaponType[] { WeaponType.shield, WeaponType.shield, WeaponType.shield,
        WeaponType.homing, WeaponType.homing,
        WeaponType.freezeGun, 
        WeaponType.moab,
        WeaponType.plasmaThrower, 
        WeaponType.triple, WeaponType.triple, WeaponType.triple };


    public WeaponDefinition[] weaponDefn;

    private bool _spawnEnemies = true; //should enmies be spawned
    private BoundsCheck _boundM;

    public bool spawnEnemies
    {
        get { return _spawnEnemies; }
        set
        {
            _spawnEnemies = value;
            if (!_spawnEnemies)
            {
                CancelInvoke("SpawnEnemy"); //sto spawn calls that are invoked
            }
            else if (_spawnEnemies && !IsInvoking("SpawnEnemy"))
            {
                Invoke("SpawnEnemy", 1f); //start up spawn calls
            }
        }
    }

    //list of all active enemies - will be needed for more complex weapons so it was set up now to avoid large refactor later
    private List<GameObject> _allEnemiesList = new List<GameObject>();
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
        _boundM = GetComponent<BoundsCheck>(); //gets the bounds check component
        Invoke("SpawnEnemy", 1f / enemySpawnRate); //this start the enemies spawning
       
        
        if (PowerUp.allPossibleColors == null)
        {
            PowerUp.allPossibleColors = new List<Color>();
        }

        //adds the weapon definitions into the dictionary so they can be easily looked up later
        foreach (WeaponDefinition def in weaponDefn)
        {
            _weaponDictionary.Add(def.type, def); //adds the definition for the weapons into the dictionary for easy look up later
            PowerUp.allPossibleColors.Add(def.color);
        }

    }
    


    public void SpawnEnemy()
    {
        int randEnemy = Random.Range(0, Level.scriptRef.randRange); //random find which enemy to generate within the range specified by level class
        
        bool boss = Level.scriptRef.boss; //get field to see if boss should be spawned

        GameObject spawned;

        //get different pre fab if it is the boss level
        if (boss)
        {
            Level.scriptRef.boss = false;
            spawned = Instantiate<GameObject>(preFabEnemies[4]);
        }
        else
        {
            spawned = Instantiate<GameObject>(preFabEnemies[randEnemy]);
        }


        enemySpawnRate = Level.eSpawnRate; //update spawn rate to match level class
        level = Level.scriptRef.level; //update level to match current level

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

        //adds the enemy into the list of all enemies
        _allEnemiesList.Add(spawned);

        //center boss in game
        if (boss)
        {
            startPos.x = 0;
        }

        spawned.transform.position = startPos;

        //this stop enemies from spawning
        if (_spawnEnemies)
        {
            if (!boss) //stops spawning of any enemy after one boss has been spawned
            {
                Invoke("SpawnEnemy", 1f / enemySpawnRate); //invokes the function to run again
            }

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


    //get target for homing missile
    public GameObject getClosestsEnemy()
    {
        if (_allEnemiesList.Count == 0)
        {
            return null; //no enemies to target
        }
        else
        {
            GameObject SixtyDegree = null; //Ship within 60 degee view of hero
            GameObject NintyDegree = null; //Ship within 90 degee view of hero

            float ThirtyDegreeXOverY = 0.577f;
            float SixtyDegreeXOverY = 1.732f;


            /*Goal of this code is to find "best" enemy for homing missile to lock onto
             * first tries to find an enemey within 30 degreee field of view
             * then tries 60 degree
             * then 90
             * then returns a random enemy if all enemies are below the ship
             */
            foreach (GameObject obj in _allEnemiesList)
            {
                float deltaEnemyMissileY = obj.transform.position.y - Hero_Script.heroScriptReference.gameObject.transform.position.y;
                float deltaEnemyMissileX = Mathf.Abs(obj.transform.position.x - Hero_Script.heroScriptReference.gameObject.transform.position.x); //side of axis does not matter - abs is easier
                deltaEnemyMissileX = Mathf.Min(deltaEnemyMissileX, deltaEnemyMissileX - 0.5f);//account for ship size
                
                if (deltaEnemyMissileY > 0.0f)
                {
                    if (deltaEnemyMissileY * ThirtyDegreeXOverY > deltaEnemyMissileX)
                    {
                        return obj;
                    }
                    else if(SixtyDegree == null && deltaEnemyMissileY * SixtyDegreeXOverY > deltaEnemyMissileX)
                    {
                        SixtyDegree = obj;
                    }
                    else if (SixtyDegree == null && NintyDegree == null)
                    {
                        NintyDegree = obj;
                    }
                }
            }

            if (SixtyDegree != null)
            {
                return SixtyDegree;
            }
            else if(NintyDegree != null)
            {
                return NintyDegree;
            }
            return _allEnemiesList[_allEnemiesList.Count - 1];
        }
    }

    //this function deletes all enemies and resets the enemy list - used to remove all enemies from screen
    public void DeleteAllEnemies()
    {
        foreach(GameObject item in _allEnemiesList)
        {
            DestroyEnemy(item,true,false);
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
    public void DestroyEnemy(GameObject enemyToDestroy, bool updateScore = false, bool removeFromList = true)
    {
        if (removeFromList)
        {  
            _allEnemiesList.Remove(enemyToDestroy);
        }
        if (updateScore)
        {
            Score.scoreControllerReference.UpdateScore(enemyToDestroy.tag);
        }
        GameObject explos = Instantiate(particleExplosion);
        explos.transform.position = enemyToDestroy.transform.position;
        Destroy(enemyToDestroy);
        //invokes SpawnEnemy function again after boss is destroyed
        if (enemyToDestroy.tag == "EnemyBoss")
        {
            Invoke("SpawnEnemy", 1 / enemySpawnRate);
        }
        
    }
    
    // this function is called when an enemy ship is destroyed - depending on the ship (weaker ones have less probability of dropping a powerup)
    // the random function will determine if a powerup should drop. Then, another element of randomness determines which powerup should be dropped.
    public void ShipDestroyed(Enemy_Parent e)
    {
        if(Random.value <= e.powerUpDropChance)
        {
            int ndx = Random.Range(0, _powerUpFrequency.Length);
            WeaponType powerUpType = _powerUpFrequency[ndx];
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp powerUp = go.GetComponent<PowerUp>();

            bool rand = false;

            float randVal = Random.Range(0.0f,1.0f);

            if (randVal < 0.2f && powerUpType != WeaponType.shield) { rand = true; }
            
            powerUp.SetType(powerUpType,rand);
            powerUp.transform.position = e.transform.position;
        }
    }
}
