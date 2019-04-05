using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss_Movement : Enemy_Parent
{
    public float speed; //speed field for how fast boss will drop
    public delegate void fireWeapons(); //creates delegate type
    public fireWeapons fireWeaponsDelegate; //creates variable of type fireWeapons

    void Start()
    {
        _health = 450; // sets health from parent class to the appropriate value, depending on the enemy type.
        powerUpDropChance = 0.9f; // probability of power up dropping 
    }

    //handles movement of the enemy
    protected override void Move()
    {
        Vector3 temporaryPosition = position;
        temporaryPosition.y -= 2.5f * Time.deltaTime * _speedFactor;
        position = temporaryPosition;
    }

    void Update()
    {
        //responsible for firing the weapon
        if (fireWeaponsDelegate != null)
        {
            fireWeaponsDelegate();
        }
        base.Update();
    }
}
