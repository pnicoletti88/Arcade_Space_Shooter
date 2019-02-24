using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Move : Enemy_Parent
{
    public bool directionFlag; //why is this public?
    
    void Start()
    {
        directionFlag = (Random.Range(0, 2) == 0);
    }

    protected override void Move()
    {
        Vector3 temporaryPosition = position;
        if (directionFlag)
        {
            temporaryPosition.x -= 10f * Time.deltaTime;
        }
        else
        {
            temporaryPosition.x += 10f * Time.deltaTime;
        }
        temporaryPosition.y -= 10f * Time.deltaTime;
        position = temporaryPosition;
    }
}
