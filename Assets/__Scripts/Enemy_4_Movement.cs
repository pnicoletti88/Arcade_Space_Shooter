using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_4_Movement : Enemy_0_Movement
{

    public delegate void fireWeapons(); //creates delegate type
    public fireWeapons fireWeaponsDelegate; //creates variable of type fireWeapons

    void Start()
    {
        powerUpDropChance = 0.4f;
    }

    protected override void Update()
    {
        if (fireWeaponsDelegate != null)
        {
            fireWeaponsDelegate();
        }
        base.Update();
    }

}
