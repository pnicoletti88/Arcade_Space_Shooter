using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0_Movement : Enemy_Parent
{
    void Start()
    {
        _health = 50;
    }

    protected override void Move()
    {
        Vector3 temporaryPosition = position;
        temporaryPosition.y -= 10f * Time.deltaTime;
        position = temporaryPosition;
    }
}
