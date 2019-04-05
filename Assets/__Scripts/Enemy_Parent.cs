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
        
        //constructor
        public ObjectColourPairs(GameObject objIn)
        {
            _gameObj = objIn;
            if (_gameObj.GetComponent<Renderer>() != null) //determine if it has a render
            {
                //only store colour is object has a render
                _containsRender = true;
                _originalColour = _gameObj.GetComponent<Renderer>().material.color;
            }
        }

        //getter
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

        //indicate if object has a colour
        public bool ContainsRenderer()
        {
            return _containsRender;
        }
    }




    [Header("Set in Inspector: Enemy")]

    private List<ObjectColourPairs> _allGameObjectsInObject;
    private float _colourChangeTime = 0;//holds time that object got damaged
    private float _onFireTime = 0;//holds time that object got set on fire
    private float _onFireColourLastChageTime = 0; //for changing shades of red when enemy is on fire
    private float _timeToRemainColourChange = 0.15f; //how long enemy stays red for when it gets shot
    private float _timeToRemainOnFire = 1.5f; //how long enemy remains on fire when it gets hit with flame
    private bool _higherColourFireCycleCounter = true; //this is a boolean to control shades of when on fire
    private float _healthDamageOnNextUpdate = 0; //this is damage to be done to enemy on next update call - for rapid collisions
    private float _frozenTime = 0; //holds time enemy got set of fire
    private float _timeToRemainFrozen = 2.5f; //time enemy is to remain frozen

    
    protected BoundsCheck _bound; //bounds check class
    protected float _health = 0; //set in child class
    protected float _speedFactor = 1f; //this is the speed factor - used to slow down the enemy
    public float powerUpDropChance = 0.2f;
    public bool _isAlive = true; //used to stop multiple powerups from spawning
    public float health; //health of enemies


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
        _health = health;
    }

    protected virtual void Update()
    {

        Move(); //calls move which is defined in the child class

        //destroy enemy if its off screen
        if (_bound != null && (_bound.offScreenDown || _bound.offScreenLeft || _bound.offScreenRight))
        {
            Main_MainScene.scriptReference.DestroyEnemy(gameObject);
        }

        //deals damage if there is some to do on update
        if (_healthDamageOnNextUpdate != 0)
        {
            _health -= _healthDamageOnNextUpdate;
            _healthDamageOnNextUpdate = 0;
            CheckHealth();
        }

        //this hanldes what happens when the enemy is on fire
        if (_onFireTime != 0)
        {
            //determines if it is time to stop being on fire
            if ((Time.time - _onFireTime) > _timeToRemainOnFire)
            {
                ChangeColour(true); //resets colour
                _onFireTime = 0;
            }

            else
            {
                _health -= Main_MainScene.GetWeaponDefinition(WeaponType.plasmaThrower).damage * Time.deltaTime; //damage the enemy while it is on fire,  damage is per second
                CheckHealth();

                //cycles the colour
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

        //this handles what happens when the enemy is on frozen
        if (_frozenTime != 0)
        {
            //stop being frozen
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

        //resets colour after regular damage done
        if (_colourChangeTime != 0 && (Time.time - _colourChangeTime) > _timeToRemainColourChange && _onFireTime == 0 && _frozenTime == 0)
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
    }

    //function for collision with a projectile
    void OnCollisionEnter(Collision coll)
    {
        GameObject otherColl = coll.gameObject;
        
        if (otherColl.tag == "ProjectileHero" || otherColl.tag == "homing")
        {
            Projectile p = otherColl.GetComponent<Projectile>();
            Destroy(otherColl);
            ChangeColour(false, 50, -75, -75,false); //changes colour to show the damage
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
            if (_isAlive)
            {
                Main_MainScene.scriptReference.ShipDestroyed(this);
            }
            _isAlive = false;
            Main_MainScene.scriptReference.DestroyEnemy(gameObject,true);
        }
    }

    //updates the user score according to the type of enemy
    

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
                    //resets colour 
                    pair.GetGameObject().GetComponent<Renderer>().material.color = pair.GetColor();
                }
                else
                {

                    float rValue = col.r;
                    float gValue = col.g;
                    float bValue = col.b;

                    Color newCol = new Color();//note rgb is scaled to be between 0 and 1 in unity (instead of 0 - 255)

                    //following blocks changes rbg value keeping it within 0 and 1 through min and max
                    //colour can never be outside of 0 and 1 which is why min and max is used
                    //changing R
                    if (changeInR >= 0)
                    {
                        newCol.r = Mathf.Min(255, (rValue * 255 + changeInR)) / 255;
                    }
                    else
                    {
                        newCol.r = Mathf.Max(0, (rValue * 255 + changeInR)) / 255;
                    }
                    //Changing G
                    if (changeInG >= 0)
                    {
                        newCol.g = Mathf.Min(255, (gValue * 255 + changeInG)) / 255;
                    }
                    else
                    {
                        newCol.g = Mathf.Max(0, (gValue * 255 + changeInG)) / 255;
                    }
                    //Changing B
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
