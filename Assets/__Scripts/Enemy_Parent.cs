using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//abstract class to serve as parent for all enemies
public abstract class Enemy_Parent : MonoBehaviour
{
    //class to hold game object and its original colour - allows for switching back to original
    private class ObjectColourPairs
    {

        private GameObject _gameObj;
        private Color _originalColour;
        private bool _containsRender = false; //not all gameObject will have a render
        

        public ObjectColourPairs(GameObject objIn)
        {
            _gameObj = objIn;
            if (_gameObj.GetComponent<Renderer>() != null) //determine if it has a render
            {
                _containsRender = true;
                _originalColour = _gameObj.GetComponent<Renderer>().material.color;
            }
        }

        public GameObject GetGameObject()
        {
            return _gameObj;
        }

        //safe return - needs to have a colour
        public Color GetColor()
        {
            if (_containsRender)
            {
                return _originalColour;
            }
            Debug.LogError("Trying to get colour with no renderer");
            return new Color();
        }

        public bool ContainsRenderer()
        {
            return _containsRender;
        }
    }




    [Header("Set in Inspector: Enemy")]

    private List<ObjectColourPairs> _allGameObjectsInObject;
    private float _colourChangeTime = 0;//holds time that object got damaged
    private float _onFireTime = 0;//holds time that object got set on fire
    private float _onFireColourLastChageTime = 0;
    private float _timeToRemainColourChange = 0.15f;
    private float _timeToRemainOnFire = 1.5f;
    private bool _higherColourFireCycleCounter = true;
    private float _healthDamageOnNextUpdate = 0;
    private float _frozenTime = 0;
    private float _timeToRemainFrozen = 2.5f;
    private bool _flyAway = false;
    

    protected BoundsCheck _bound;
    protected float _health = 0; //set in child class
    protected float _speedFactor = 1f;
    public float powerUpDropChance = 0.2f;
    public bool _isAlive = true;


    //property to get and set the position of the enemy objects
    public Vector3 position
    {
        get
        {
            return (transform.position);
        }
        set
        {
            transform.position = value;
        }
    }

    protected void Awake()
    {
        _bound = GetComponent<BoundsCheck>();
        _allGameObjectsInObject = new List<ObjectColourPairs>();
        getAllChildren(this.gameObject.transform);
        _onFireColourLastChageTime = 0;
        _onFireTime = 0;
        _colourChangeTime = 0;
    }

    protected virtual void Update()
    {

        Move(); //calls move which is defined in the child class
        if (_bound != null && ((_bound.offScreenDown || _bound.offScreenLeft || _bound.offScreenRight)||(_bound.offScreenUp && _flyAway)))
        {
            Main_MainScene.scriptReference.DestroyEnemy(gameObject);
        }

        if (_healthDamageOnNextUpdate != 0)
        {
            _health -= _healthDamageOnNextUpdate;
            _healthDamageOnNextUpdate = 0;
            CheckHealth();
        }

        if (_onFireTime != 0)
        {
            if ((Time.time - _onFireTime) > _timeToRemainOnFire)
            {
                ChangeColour(true);
                _onFireTime = 0;
            }
            else
            {
                _health -= Main_MainScene.GetWeaponDefinition(WeaponType.plasmaThrower).damage * Time.deltaTime; //damage the enemy while it is on fire,  damage is per second
                CheckHealth();
                if ((Time.time - _onFireColourLastChageTime) > 0.1f)
                {
                    if (_higherColourFireCycleCounter)
                    {
                        ChangeColour(false, -15, 20, 20, true);
                    }
                    else
                    {
                        ChangeColour(false, 15, -20, -20, true);
                    }
                    _higherColourFireCycleCounter = !_higherColourFireCycleCounter;
                    _onFireColourLastChageTime = Time.time;
                }
            }
        }

        if (_frozenTime != 0)
        {
            if ((Time.time - _frozenTime) > _timeToRemainFrozen)
            {
                _frozenTime = 0;
                _speedFactor = 1f;
                ChangeColour(true);
            }
            else
            {
                _health -= Main_MainScene.GetWeaponDefinition(WeaponType.freezeGun).damage * Time.deltaTime; //damage the enemy while it is on fire,  damage is per second
                CheckHealth();
            }
        }
        else if (_colourChangeTime != 0 && (Time.time - _colourChangeTime) > _timeToRemainColourChange)
        {
            ChangeColour(true);
            _colourChangeTime = 0;
        }
    }


    //since this is different for all classes it will be implemented by them
    protected abstract void Move();


    //this function damages the enemy when they collide with a projectile.
    void OnParticleCollision(GameObject otherColl)
    {
        Debug.Log(otherColl.tag);
        if (otherColl.tag == "plasmaThrower")
        {
            if (_onFireTime == 0)
            {
                ChangeColour(false, 50, -75, -75, false);
            }
            _onFireTime = Time.time;
        }
        else if (otherColl.tag == "freezeGun")
        {
            if (_frozenTime == 0)
            {
                ChangeColour(false, -75, -75, 50, false);
                _speedFactor = 0.2f;
            }
            _frozenTime = Time.time;
            Destroy(otherColl);
        }
        else if (otherColl.tag == "moab")
        {
            _health = 0;
            CheckHealth();
        }
    }

    //function for collision with a projectile
    void OnCollisionEnter(Collision coll)
    {
        GameObject otherColl = coll.gameObject;
        
        if (otherColl.tag == "ProjectileHero" || otherColl.tag == "homing")
        {
            Projectile p = otherColl.GetComponent<Projectile>();
            Destroy(otherColl);
            ChangeColour(false, 50, -75, -75,false);
            _colourChangeTime = Time.time;

            if (_bound.onScreen)
            {
                if (otherColl.tag == "homing")
                {
                    //missile collides many time with the enemy when it hits due to how it moves
                    //in order to deal with this it deals its damage during the next update (as this is after the spur of collisions)
                    //to the user this is not noticeable due to the speed of the frameRate
                    _healthDamageOnNextUpdate = Main_MainScene.GetWeaponDefinition(p.type).damage;
                }
                else
                {
                    _health -= Main_MainScene.GetWeaponDefinition(p.type).damage; //update health
                    CheckHealth();
                }
                
            }    
        }
    }

    //determines if the enemy should die
    void CheckHealth()
    {
        if (_health <= 0)
        {
            UpdateScore(gameObject);
            if (_isAlive)
            {
                Main_MainScene.scriptReference.ShipDestroyed(this);
            }
            _isAlive = false;
            Main_MainScene.scriptReference.DestroyEnemy(gameObject);
        }
    }

    //updates the user score according to the type of enemy
    public static void UpdateScore(GameObject gA)
    {
        Debug.Log(gA.tag);
        switch(gA.tag)
        {
            case "Enemy0":
                Score.scoreControllerReference.AddScore(5);
                break;
            case "Enemy1":
                Score.scoreControllerReference.AddScore(10);
                break;
            case "Enemy2":
                Score.scoreControllerReference.AddScore(15);
                break;
            case "Enemy4":
                Score.scoreControllerReference.AddScore(15);
                break;
            case "EnemyBoss":
                Score.scoreControllerReference.AddScore(75);
                break;
            default:
                break;
        }
    }

    //use to show the enemy taking damage or being on plasma fire
    void ChangeColour(bool reset, float changeInR=0, float changeInG=0, float changeInB=0, bool useCurrentColor=false)
    {
        foreach(ObjectColourPairs pair in _allGameObjectsInObject)
        {
            if (pair.ContainsRenderer())//checks to see if object can have colour changed
            {
                Color col;
                if (useCurrentColor)
                {
                    col = pair.GetGameObject().GetComponent<Renderer>().material.color; //gets the current colour
                }
                else
                {
                    col = pair.GetColor();
                }

                //changes colour if equal
                if (reset)
                {
                    //resets colour if not equal
                    
                    pair.GetGameObject().GetComponent<Renderer>().material.color = pair.GetColor();
                    

                }
                else
                {

                    float rValue = col.r;
                    float gValue = col.g;
                    float bValue = col.b;

                    Color newCol = new Color();//note rgb is scaled to be between 0 and 1 in unity (instead of 0 - 255)

                    //following blocks changes rbg value keeping it within 0 and 1 through min and max
                    if (changeInR >= 0)
                    {
                        newCol.r = Mathf.Min(255, (rValue * 255 + changeInR)) / 255;
                    }
                    else
                    {
                        newCol.r = Mathf.Max(0, (rValue * 255 + changeInR)) / 255;
                    }

                    if (changeInG >= 0)
                    {
                        newCol.g = Mathf.Min(255, (gValue * 255 + changeInG)) / 255;
                    }
                    else
                    {
                        newCol.g = Mathf.Max(0, (gValue * 255 + changeInG)) / 255;
                    }

                    if (changeInB >= 0)
                    {
                        newCol.b = Mathf.Min(255, (bValue * 255 + changeInB)) / 255;
                    }
                    else
                    {
                        newCol.b = Mathf.Max(0, (bValue * 255 + changeInB)) / 255;
                    }

                    //updating the colour of the object
                    pair.GetGameObject().GetComponent<Renderer>().material.color = newCol;
                    
                }
            }
        }
    }

    //gets all of the children and the original object in a game object
    //gets children all the way down the tree (so all parts will change color there is many layers of game objects)
    void getAllChildren(Transform parent)
    {
        _allGameObjectsInObject.Add(new ObjectColourPairs(parent.gameObject));
        foreach (Transform child in parent)
        {
            if (child.childCount > 0)
            {
                getAllChildren(child);
            }
            else
            {
                _allGameObjectsInObject.Add(new ObjectColourPairs(child.gameObject));
            }
        }
    }

}
