using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//abstract class to serve as parent for all enemies
public abstract class Enemy_Parent : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]


    protected BoundsCheck _bound;


    public Vector3 position
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    protected void Awake()
    {
        _bound = GetComponent<BoundsCheck>();
    }

    protected void Update()
    {
        Move();
        if (_bound != null && _bound.offScreenDown)
        {
            Destroy(gameObject);
        }

    }

    //since this is different for all classes it will be implemented by them
    protected abstract void Move();
}
