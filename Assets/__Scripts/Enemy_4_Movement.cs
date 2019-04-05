using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_4_Movement : Enemy_0_Movement
{

    public delegate void fireWeapons(); //creates delegate type
    public fireWeapons FireWeaponsDelegate; //creates variable of type fireWeapons

    void Start()
    {
        powerUpDropChance = 0.4f;
    }

    protected override void Update()
    {
        //this ship fires bullets - call fire every update but it only fire when gun is reloaded due to weapon
        if (FireWeaponsDelegate != null)
        {
            FireWeaponsDelegate();
        }
        base.Update();
    }

}
