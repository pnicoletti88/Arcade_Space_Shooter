using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0_Movement : Enemy_Parent
{

    void Start()
    {
        powerUpDropChance = 0.225f;
    }

    //handles the movement of Enemy 0
    protected override void Move()
    {
        Vector3 temporaryPosition = position;
        temporaryPosition.y -= 10f * Time.deltaTime * _speedFactor;
        position = temporaryPosition;
    }

    //calls the base update function - this is done so that children of this class can call enemy parent update
    protected override void Update()
    {
        base.Update();
    }
}
