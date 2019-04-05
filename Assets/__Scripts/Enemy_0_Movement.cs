using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0_Movement : Enemy_Parent
{

    void Start()
    {
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
