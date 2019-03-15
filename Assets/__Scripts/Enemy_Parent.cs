using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//abstract class to serve as parent for all enemies
public abstract class Enemy_Parent : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]


    protected BoundsCheck _bound;

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
            Destroy(gameObject);
        }

    }

    //since this is different for all classes it will be implemented by them
    protected abstract void Move();
}
