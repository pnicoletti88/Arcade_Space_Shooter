using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0_Movement : Enemy_Parent
{
    void Start()
    {
        _health = 50; // sets health from parent class to the appropriate value, depending on the enemy type.
        powerUpDropChance = 0.225f;
    }

    protected override void Move()
    {
        Vector3 temporaryPosition = position;
        temporaryPosition.y -= 10f * Time.deltaTime * _speedFactor;
        position = temporaryPosition;
    }

    protected override void Update()
    {
        base.Update();
    }
}
