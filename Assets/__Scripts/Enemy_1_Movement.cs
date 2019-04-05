using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Movement : Enemy_Parent
{
    private int _directionFlag; //determine if it moves in negatie or positive x direction

    void Start()
    {
        //randomly set direction
        powerUpDropChance = 0.3f;
        if (Random.Range(0, 2) == 0)
        {
            _directionFlag = -1;
        }
        else
        {
            _directionFlag = 1;
        } 
    }

    protected override void Move()
    {
        Vector3 temporaryPosition = position;
        temporaryPosition.x += 10f * Time.deltaTime * _directionFlag * _speedFactor; //direction flag is either positive or negative
        temporaryPosition.y -= 10f * Time.deltaTime * _speedFactor;
        position = temporaryPosition; //update position
    }


}
