using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//abstract class to serve as parent for all enemies
public abstract class Enemy_Parent : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]


    protected BoundsCheck _bound;
    protected float _health = 0; //set in child class

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
    }

    public virtual void Update()
    {
        Move(); //calls move which is defined in the child class
        if (_bound != null && (_bound.offScreenDown || _bound.offScreenLeft || _bound.offScreenRight))
        {
            Main.scriptReference.DestroyEnemy(gameObject);
        }

    }

    //since this is different for all classes it will be implemented by them
    protected abstract void Move();

    //this function damages the enemy when they collide with a projectile.
    void OnParticleCollision(GameObject otherColl)
    {
        if (otherColl.tag == "ProjectileHero")
        {
            _health -= Main.GetWeaponDefinition(WeaponType.plasmaThrower).damage * Time.deltaTime;//damage is per second
            CheckHealth();
        }

    }
    void OnCollisionEnter(Collision coll)
    {
        GameObject otherColl = coll.gameObject;
        if(otherColl.tag == "ProjectileHero")
        {
            if (_bound.onScreen)
            {
                Projectile p = otherColl.GetComponent<Projectile>();
                _health -= Main.GetWeaponDefinition(p.type).damage;
                CheckHealth();
            }
            Destroy(otherColl);
        }
    }

    void CheckHealth()
    {
        if (_health <= 0)
        {
            UpdateScore(gameObject);
            Main.scriptReference.DestroyEnemy(gameObject);
        }
    }

    void UpdateScore(GameObject gA)
    {
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
            default:
                break;
        }
    }
}
