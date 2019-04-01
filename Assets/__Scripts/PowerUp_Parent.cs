using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp_Parent : MonoBehaviour
{
    //Stack will be used for referencing current powerups
    
    protected private BoundsCheck _bound;
    //Check bounds for moving powerups
    protected void Awake()
    {
        _bound = GetComponent<BoundsCheck>();
    }
    //Remove powerup if it is in breach of bounds
    public virtual void Update()
    {
        if (_bound != null && (_bound.offScreenDown || _bound.offScreenLeft || _bound.offScreenRight))
        {
            Main_MainScene.scriptReference.DestroyPickup(gameObject);
        }
        Move();
    }
    //Property field for position
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
    
   
    protected abstract void Move();
}
