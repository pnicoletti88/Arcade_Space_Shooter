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
    public GameObject preFabPickUp;
    public float enemySpawnRate = 0.5f;
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

    //Create Pickups
    public PickUp Homing = new PickUp(Color.red, WeaponType.homing, "H", 20, "homing");
    public PickUp Moab = new PickUp(Color.blue, WeaponType.homing, "M", 20, "moab");
    public PickUp Freeze = new PickUp(Color.yellow, WeaponType.homing, "F", 20, "freezeGun");
    public PickUp Plasma = new PickUp(Color.cyan, WeaponType.homing, "P", 20, "plasmaThrower");
    public PickUp Triple = new PickUp(Color.magenta, WeaponType.homing, "T", 20, "triple");
    public PickUp Shield = new PickUp(Color.white, WeaponType.none, "S", 0, "shield");

    



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
    public void DropAPickUp(Vector3 pos)
    {
        PickUp[] pickUps = { Triple, Homing, Plasma, Freeze, Moab };
        int loadRand = Random.Range(0, pickUps.Length);
        PickUp tmp = pickUps[loadRand];
        GameObject toSpawn = Instantiate<GameObject>(preFabPickUp);
        Renderer rend = toSpawn.gameObject.GetComponent<Renderer>();
        rend.material.color = tmp.color;
        TextMesh label = toSpawn.GetComponentInChildren<TextMesh>();
        label.text = tmp.text;
        toSpawn.gameObject.tag = pickUps[loadRand].tag;
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
        toSpawn.transform.position = pos;
        _allPickUpList.Add(toSpawn);


    }
    public void SpawnPickUps()
    {
        PickUp[] pickUps = { Triple, Homing, Plasma, Freeze, Moab };
        int loadRand = Random.Range(0, pickUps.Length);
        PickUp tmp = pickUps[loadRand];
        GameObject toSpawn = Instantiate<GameObject>(preFabPickUp);
        Renderer rend = toSpawn.gameObject.GetComponent<Renderer>();
        rend.material.color = tmp.color;
        TextMesh label = toSpawn.GetComponentInChildren<TextMesh>();
        label.text = tmp.text;
        toSpawn.gameObject.tag = pickUps[loadRand].tag;
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
        
        bool boss = _level.boss;
        GameObject spawned;

        if (boss)
        {

            spawned = Instantiate<GameObject>(preFabEnemies[4]);
        }
        else
        {
            spawned = Instantiate<GameObject>(preFabEnemies[randEnemy]);
        }


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

        //adds the enemy into the list of all enemies
        _allEnemiesList.Add(spawned);

        if (boss)
        {
            startPos.x = 10;
        }

        spawned.transform.position = startPos;

        //this stop enemies from spawning
        if (_spawnEnemies)
        {
            if (!boss)
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
            //this function attemps to find an enemy which is in front of the hero, and also cloe from an x position
            //it uses a simple approach with if statements (it is by no means a perfect algorithm) but it is good enough
            //simple approached used as it gets called a lot and don't want to affect frame rate
            foreach(GameObject obj in _allEnemiesList)
            {
                float deltaEnemyMissileY = obj.transform.position.y - Hero_Script.heroScriptReference.gameObject.transform.position.y;
                float deltaEnemyMissileX = Mathf.Abs(obj.transform.position.x - Hero_Script.heroScriptReference.gameObject.transform.position.x);
                if (obj.transform.position.y > Hero_Script.heroScriptReference.gameObject.transform.position.y)
                {
                    if (deltaEnemyMissileY > 0.0f && deltaEnemyMissileY < 4.0f && deltaEnemyMissileX < 2.0f)
                    {
                        return obj;
                    }
                    if (deltaEnemyMissileY > 4.0f && deltaEnemyMissileY < 8.0f && deltaEnemyMissileX < 4.0f)
                    {
                        return obj;
                    }
                    if(deltaEnemyMissileY > 10.0f)
                    {
                        return obj;
                    }
                }
            }
            return _allEnemiesList[_allEnemiesList.Count - 1];
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
        if (enemyToDestroy.tag == "EnemyBoss")
            Invoke("SpawnEnemy", 1 / enemySpawnRate);
    }

    public void DestroyPickup(GameObject pickUpToDestroy)
    {
       
        _allPickUpList.Remove(pickUpToDestroy);
        GameObject explos = Instantiate(pickUpAnimation);
        explos.transform.position = pickUpToDestroy.transform.position;
        Destroy(pickUpToDestroy);
    }
}
