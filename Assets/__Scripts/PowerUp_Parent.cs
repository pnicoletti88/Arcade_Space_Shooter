using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp_Parent : MonoBehaviour
{
    //Stack will be used for referencing current powerups
    public Stack<WeaponType> activeWeapons;
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
            Main_MainScene.scriptReference.DestroyEnemy(gameObject);
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
    //Mechanic for implementing what happens when the hero collects the pickup
    void OnTriggerEnter(Collider otherColl)
    {
        print(otherColl.gameObject.name);

    }
    protected abstract void PowerUp();
    protected abstract void Move();
}
