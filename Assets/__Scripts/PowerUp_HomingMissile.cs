using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_HomingMissile : PowerUp_Parent
{
    protected override void Move()
    {
        Vector3 temporaryPosition = position;
        temporaryPosition.y -= 10f * Time.deltaTime;
        position = temporaryPosition; //update position
    }

    protected override void PowerUp()
    {
        activeWeapons.Push(WeaponType.homing);
    }
}
